using ADE.ESS.SEPA.Application.Interfaces;
using ADE.ESS.SEPA.Application.Models;
using ADE.ESS.SEPA.Client.Auth;
using ADE.ESS.SEPA.Common;
using ADE.ESS.SEPA.Infrastructure.Context;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace ADE.ESS.SEPA.Client.Services
{
    public class AppStateService : IAppStateService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly IESSUserSecurityManager _userSecurityManager;
        private readonly EssSepaDbContext _dbContext;
        private readonly ILogger<AppStateService> _logger;
        private readonly ProtectedLocalStorage _protectedLocalStorage;

        private AppStateStore _currentState;
        private const string StateStorageKey = AppStorageKeys.State;
        private const string EntityIdStorageKey = AppStorageKeys.EntityId;
        private const string EntityNameStorageKey = AppStorageKeys.EntityName;

        public AppStateStore CurrentState => _currentState;
        public bool IsInitialized => _currentState.IsInitialized;

        // Events for reactive state management
        public event Action<AppStateStore>? StateChanged;
        public event Action<UserState>? UserStateChanged;
        public event Action<EdOrgState>? EdOrgStateChanged;

        public AppStateService(
            IHttpContextAccessor httpContextAccessor,
            AuthenticationStateProvider authStateProvider,
            IESSUserSecurityManager userSecurityManager,
            EssSepaDbContext dbContext,
            ILogger<AppStateService> logger,
            ProtectedLocalStorage protectedLocalStorage,
            AppStateStore appStateStore)
        {
            _httpContextAccessor = httpContextAccessor;
            _authStateProvider = authStateProvider;
            _userSecurityManager = userSecurityManager;
            _dbContext = dbContext;
            _logger = logger;
            _protectedLocalStorage = protectedLocalStorage;
            _currentState = appStateStore;
        }

        #region Initialization
        public async Task InitializeAppStateAsync(int entityId)
        {
            try
            {
                _logger.LogInformation("Initializing app state for EntityId: {EntityId}", entityId);

                // Try to load from session first
                if (await LoadStateFromSessionAsync())
                {
                    // Update EntityId if different
                    if (_currentState.EdOrg.EntityId != entityId)
                    {
                        await InitializeEdOrgAsync(entityId);
                    }

                    _logger.LogInformation("App state loaded from session");
                    return;
                }

                // Initialize fresh state
                await InitializeUserAsync();
                await InitializeEdOrgAsync(entityId);

                _currentState.IsInitialized = true;
                _currentState.LastUpdated = DateTime.UtcNow;

                await SaveStateToStorageAsync();
                StateChanged?.Invoke(_currentState);

                _logger.LogInformation("App state initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing app state for EntityId: {EntityId}", entityId);
                throw;
            }
        }

        public async Task RefreshStateAsync()
        {
            if (_currentState.EdOrg.EntityId > 0)
            {
                await InitializeAppStateAsync(_currentState.EdOrg.EntityId);
            }
        }
        #endregion

        #region User State Management
        public async Task InitializeUserAsync()
        {
            try
            {
                var authState = await _authStateProvider.GetAuthenticationStateAsync();

                var userState = new UserState
                {
                    Email = _userSecurityManager.GetEmail() ?? "",
                    FirstName = _userSecurityManager.GetFirstName() ?? "",
                    LastName = _userSecurityManager.GetLastName() ?? "",
                    Department = _userSecurityManager.GetDepartment() ?? "",
                    IsAuthenticated = authState.User?.Identity?.IsAuthenticated ?? false
                };

                // Get user permissions
                var permissions = _userSecurityManager.GetEntityPermissions();
                userState.Permissions = permissions.Where(p => p.EntityId.HasValue && p.EntityId.Value > 0).Select(p => new UserPermission
                {
                    EntityId = p.EntityId!.Value,
                    Permission = p.Permission.ToString(),
                    PermissionDescription = p.Permission.ToString(),
                    GrantedDate = DateTime.UtcNow
                }).ToList();

                userState.AuthorizedEntityIds = permissions.Where(p => p.EntityId.HasValue && p.EntityId.Value > 0).Select(p => p.EntityId!.Value).Distinct().ToList();

                // Check admin and read-only status
                try
                {
                    var entityId = _currentState.EdOrg.EntityId;
                    userState.IsESSAdmin = _userSecurityManager.IsUserAuthorized(entityId, ESSUserPermissionsEnum.ESSAdmin);
                    userState.IsReadOnly = _userSecurityManager.IsUserAuthorized(entityId, ESSUserPermissionsEnum.ReadOnly)
                        && !userState.IsESSAdmin;
                    await _protectedLocalStorage.SetAsync(AppStorageKeys.IsEssAdmin,userState.IsESSAdmin);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not determine admin/read-only status");
                    userState.IsESSAdmin = false;
                    userState.IsReadOnly = false;
                }

                _currentState.User = userState;
                UserStateChanged?.Invoke(userState);

                _logger.LogInformation("User state initialized for: {UserEmail}", userState.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing user state");
                throw;
            }
        }

        public async Task<UserState> GetUserStateAsync()
        {
            if (!_currentState.IsInitialized)
            {
                await InitializeUserAsync();
            }
            return _currentState.User;
        }

        public async Task UpdateUserStateAsync(UserState userState)
        {
            _currentState.User = userState;
            _currentState.LastUpdated = DateTime.UtcNow;

            await SaveStateToStorageAsync();
            UserStateChanged?.Invoke(userState);
            StateChanged?.Invoke(_currentState);

            await Task.CompletedTask;
        }

        public async Task ClearUserState()
        {
            _currentState.User = new UserState();
            UserStateChanged?.Invoke(_currentState.User);
            await Task.CompletedTask;
        }
        #endregion

        #region EdOrg State Management
        public async Task InitializeEdOrgAsync(int entityId)
        {
            try
            {
                if (entityId <= 0)
                {
                    _logger.LogWarning("Invalid EntityId provided: {EntityId}", entityId);
                    return;
                }

                // Validate entity access
                if (!await ValidateEntityAccessAsync(entityId))
                {
                    _logger.LogWarning("User does not have access to EntityId: {EntityId}", entityId);
                    throw new UnauthorizedAccessException($"Access denied to EntityId: {entityId}");
                }

                // Get entity information from database
                //var educationOrg = await _dbContext.EducationOrganizations
                //    .FirstOrDefaultAsync(e => e.EducationOrganizationId == entityId);

                var edOrgState = new EdOrgState
                {
                    EntityId = entityId,
                    EntityName = "educationOrg?.EducationOrganizationName" ?? $"Entity {entityId}",
                    CTDS = "educationOrg?.Ctds" ?? "",
                    IsADE = entityId == AdeConstants.AdeEntityId,
                    IsActive = true,
                    LastAccessed = DateTime.UtcNow
                };

                // Determine entity type using spPsoNgEntitiesGet (OwnerId logic from older codebase)
                if (edOrgState.IsADE)
                {
                    edOrgState.EntityType = "ADE";
                }
                else
                {
                    try
                    {
                        //var entityInfo = await _dbContext.Procedures
                        //    .spPsoNgEntitiesGetAsync(null, entityId, null);
                        //var match = entityInfo.FirstOrDefault(e => e.EntityId == entityId);
                        //if (match != null)
                        //{
                            edOrgState.OwnerId = 1234;
                            edOrgState.IsDistrict = 1 == 0;
                            edOrgState.IsSchool = 1 != 0;
                            edOrgState.EntityType = edOrgState.IsSchool ? "School" : "District";
                        //}
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not determine entity type for EntityId: {EntityId}", entityId);
                        edOrgState.EntityType = "Unknown";
                    }
                }

                //Get address information if available              
                //var address = await _dbContext.EducationOrganizationAddresses
                //    .FirstOrDefaultAsync(a => a.EducationOrganizationId == entityId);

                //if (address != null)
                //{
                //edOrgState.Address = $"{address.StreetNumberName}, {address.City}, {address.StateAbbreviationTypeKey} {address.PostalCode}";
                edOrgState.Address = "Some random address";
               //}

               _currentState.EdOrg = edOrgState;

                // Store in ProtectedLocalStorage for persistence
                try
                {
                    await _protectedLocalStorage.SetAsync(EntityIdStorageKey, entityId);
                    await _protectedLocalStorage.SetAsync(EntityNameStorageKey, edOrgState.EntityName);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not save entity info to protected storage");
                }

                EdOrgStateChanged?.Invoke(edOrgState);

                _logger.LogInformation("EdOrg state initialized for EntityId: {EntityId}, Name: {EntityName}",
                    entityId, edOrgState.EntityName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing EdOrg state for EntityId: {EntityId}", entityId);
                throw;
            }
        }

        public async Task<EdOrgState> GetEdOrgStateAsync()
        {
            if (!_currentState.IsInitialized || _currentState.EdOrg.EntityId == 0)
            {
                // Try to get from ProtectedLocalStorage
                try
                {
                    var entityIdResult = await _protectedLocalStorage.GetAsync<int>(EntityIdStorageKey);
                    if (entityIdResult.Success && entityIdResult.Value > 0)
                    {
                        await InitializeEdOrgAsync(entityIdResult.Value);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not read EntityId from protected storage");
                }
            }
            return _currentState.EdOrg;
        }

        public async Task UpdateEdOrgStateAsync(EdOrgState edOrgState)
        {
            _currentState.EdOrg = edOrgState;
            _currentState.LastUpdated = DateTime.UtcNow;

            // Update ProtectedLocalStorage
            try
            {
                await _protectedLocalStorage.SetAsync(EntityIdStorageKey, edOrgState.EntityId);
                await _protectedLocalStorage.SetAsync(EntityNameStorageKey, edOrgState.EntityName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not update entity info in protected storage");
            }

            await SaveStateToStorageAsync();
            EdOrgStateChanged?.Invoke(edOrgState);
            StateChanged?.Invoke(_currentState);
        }

        public async Task ClearEdOrgState()
        {
            _currentState.EdOrg = new EdOrgState();

            try
            {
                await _protectedLocalStorage.DeleteAsync(EntityIdStorageKey);
                await _protectedLocalStorage.DeleteAsync(EntityNameStorageKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not clear entity info from protected storage");
            }

            EdOrgStateChanged?.Invoke(_currentState.EdOrg);
        }
        #endregion

        #region ProtectedLocalStorage Management
        public async Task SaveStateToStorageAsync()
        {
            try
            {
                await _protectedLocalStorage.SetAsync(StateStorageKey, _currentState);
                _logger.LogDebug("App state saved to protected storage");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving state to protected storage");
            }
        }

        // Legacy method for backward compatibility - now calls async version
        public void SaveStateToSession()
        {
            // This will be called asynchronously where possible
            _ = Task.Run(async () => await SaveStateToStorageAsync());
        }

        public async Task<bool> LoadStateFromSessionAsync()
        {
            try
            {
                var stateResult = await _protectedLocalStorage.GetAsync<AppStateStore>(StateStorageKey);

                if (stateResult.Success && stateResult.Value != null && stateResult.Value.IsInitialized)
                {
                    _currentState = stateResult.Value;
                    _logger.LogDebug("App state loaded from protected storage");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading state from protected storage");
                return false;
            }
        }

        public async Task ClearAllStateAsync()
        {
            _currentState = new AppStateStore();

            try
            {
                await _protectedLocalStorage.DeleteAsync(StateStorageKey);
                await _protectedLocalStorage.DeleteAsync(EntityIdStorageKey);
                await _protectedLocalStorage.DeleteAsync(EntityNameStorageKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not clear all state from protected storage");
            }

            StateChanged?.Invoke(_currentState);
            _logger.LogInformation("All app state cleared");
        }

        // Legacy method for backward compatibility
        public void ClearAllState()
        {
            _ = Task.Run(async () => await ClearAllStateAsync());
        }
        #endregion

        #region Entity Operations
        public async Task<bool> ValidateEntityAccessAsync(int entityId)
        {
            try
            {
                // Check if user is admin (has access to all entities)
                if (_currentState.User.IsESSAdmin)
                {
                    return true;
                }

                // Check if user has permissions for this entity
                if (_currentState.User.AuthorizedEntityIds.Contains(entityId))
                {
                    return true;
                }

                // Fallback: check permissions via security manager
                var permissions = _userSecurityManager.GetEntityPermissions(entityId);
                return permissions.Any();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating entity access for EntityId: {EntityId}", entityId);
                return false;
            }
        }

        public async Task<string> GetEntityNameAsync(int entityId)
        {
            try
            {
                if (_currentState.EdOrg.EntityId == entityId)
                {
                    return _currentState.EdOrg.EntityName;
                }

                //var entity = await _dbContext.EducationOrganizations
                //    .FirstOrDefaultAsync(e => e.EducationOrganizationId == entityId);

                //return entity?.EducationOrganizationName ?? $"Entity {entityId}";
                ///To Do 
                return "Need to work on this after DB connection";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting entity name for EntityId: {EntityId}", entityId);
                return $"Entity {entityId}";
            }
        }
        #endregion

        #region Impersonation
        public async Task SetImpersonatedPeaAsync(int peaEntityId)
        {
            if (!_currentState.User.IsESSAdmin && !_currentState.User.IsReadOnly)
                throw new UnauthorizedAccessException("Only admin users can impersonate.");

            _currentState.ImpersonatedPeaId = peaEntityId;
            _currentState.ImpersonatedSchoolId = null;

            await SaveStateToStorageAsync();
            StateChanged?.Invoke(_currentState);

            _logger.LogInformation("Admin impersonating PEA EntityId: {PeaEntityId}", peaEntityId);
        }

        public async Task ClearImpersonationAsync()
        {
            _currentState.ImpersonatedPeaId = null;
            _currentState.ImpersonatedSchoolId = null;
            _currentState.ImpersonationReturnUrl = null;

            await SaveStateToStorageAsync();
            StateChanged?.Invoke(_currentState);

            _logger.LogInformation("Admin impersonation cleared");
        }
        #endregion
    }
}