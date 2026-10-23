using ADE.ESS.SEPA.Application.Interfaces;
using ADE.ESS.SEPA.Application.Models;
using ADE.ESS.SEPA.Client.Auth;

namespace ADE.ESS.SEPA.Client.Services
{
    public class FakeAppStateService : IAppStateService
    {
        private readonly ILogger<FakeAppStateService> _logger;
        private AppStateStore _currentState;

        public AppStateStore CurrentState => _currentState;
        public bool IsInitialized => _currentState.IsInitialized;

        public event Action<AppStateStore>? StateChanged;
        public event Action<UserState>? UserStateChanged;
        public event Action<EdOrgState>? EdOrgStateChanged;

        public FakeAppStateService(ILogger<FakeAppStateService> logger, AppStateStore appStateStore)
        {
            _logger = logger;
            _currentState = appStateStore;
            LoadFakeState();
        }

        private void LoadFakeState()
        {
            _currentState.User = new UserState
            {
                Email = "fake.user@azed.gov",
                FirstName = "Fake",
                LastName = "User",
                Department = "Exceptional Student Services",
                IsAuthenticated = true,
                IsESSAdmin = true,
                IsReadOnly = false,
                AuthorizedEntityIds = new List<int> { 4235 },
                Permissions = new List<UserPermission>
                {
                    new()
                    {
                        EntityId = 4235,
                        Permission = ESSUserPermissionsEnum.ESSPostSchoolOutcomes.ToString(),
                        PermissionDescription = "ESS Post School Outcomes",
                        GrantedDate = DateTime.UtcNow
                    }
                }
            };

            _currentState.EdOrg = new EdOrgState
            {
                EntityId = 4235,
                EntityName = "Mesa Unified District",
                EntityType = "District",
                CTDS = "070248000",
                Address = "63 E Main St, Mesa, AZ 85201",
                County = "Maricopa",
                IsActive = true,
                LastAccessed = DateTime.Now,
                IsDistrict = true,
                IsSchool = false,
                IsADE = false,
                OwnerId = 0
            };

            _currentState.IsInitialized = true;
            _currentState.LastUpdated = DateTime.UtcNow;

            _logger.LogInformation("Fake app state loaded for development");
        }

        public Task InitializeAppStateAsync(int entityId)
        {
            if (!_currentState.IsInitialized)
                LoadFakeState();
            return Task.CompletedTask;
        }

        public Task RefreshStateAsync()
        {
            LoadFakeState();
            return Task.CompletedTask;
        }

        public Task InitializeUserAsync()
        {
            UserStateChanged?.Invoke(_currentState.User);
            return Task.CompletedTask;
        }

        public Task<UserState> GetUserStateAsync() => Task.FromResult(_currentState.User);

        public Task UpdateUserStateAsync(UserState userState)
        {
            _currentState.User = userState;
            _currentState.LastUpdated = DateTime.UtcNow;
            UserStateChanged?.Invoke(userState);
            StateChanged?.Invoke(_currentState);
            return Task.CompletedTask;
        }

        public Task ClearUserState()
        {
            _currentState.User = new UserState();
            UserStateChanged?.Invoke(_currentState.User);
            return Task.CompletedTask;
        }

        public Task InitializeEdOrgAsync(int entityId)
        {
            EdOrgStateChanged?.Invoke(_currentState.EdOrg);
            return Task.CompletedTask;
        }

        public Task<EdOrgState> GetEdOrgStateAsync() => Task.FromResult(_currentState.EdOrg);

        public Task UpdateEdOrgStateAsync(EdOrgState edOrgState)
        {
            _currentState.EdOrg = edOrgState;
            _currentState.LastUpdated = DateTime.UtcNow;
            EdOrgStateChanged?.Invoke(edOrgState);
            StateChanged?.Invoke(_currentState);
            return Task.CompletedTask;
        }

        public Task ClearEdOrgState()
        {
            _currentState.EdOrg = new EdOrgState();
            EdOrgStateChanged?.Invoke(_currentState.EdOrg);
            return Task.CompletedTask;
        }

        public void ClearAllState()
        {
            _currentState = new AppStateStore();
            StateChanged?.Invoke(_currentState);
        }

        public void SaveStateToSession() { }

        public Task SaveStateToStorageAsync() => Task.CompletedTask;

        public Task<bool> LoadStateFromSessionAsync() => Task.FromResult(true);

        public Task ClearAllStateAsync()
        {
            ClearAllState();
            return Task.CompletedTask;
        }

        public Task<bool> ValidateEntityAccessAsync(int entityId)
        {
            return Task.FromResult(_currentState.User.AuthorizedEntityIds.Contains(entityId));
        }

        public Task<string> GetEntityNameAsync(int entityId)
        {
            if (_currentState.EdOrg.EntityId == entityId)
                return Task.FromResult(_currentState.EdOrg.EntityName);
            return Task.FromResult($"Entity {entityId}");
        }

        public Task SetImpersonatedPeaAsync(int peaEntityId)
        {
            _currentState.ImpersonatedPeaId = peaEntityId;
            _currentState.ImpersonatedSchoolId = null;
            StateChanged?.Invoke(_currentState);
            return Task.CompletedTask;
        }

        public Task ClearImpersonationAsync()
        {
            _currentState.ImpersonatedPeaId = null;
            _currentState.ImpersonatedSchoolId = null;
            _currentState.ImpersonationReturnUrl = null;
            StateChanged?.Invoke(_currentState);
            return Task.CompletedTask;
        }
    }
}
