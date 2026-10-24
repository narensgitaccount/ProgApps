using System.Globalization;
using System.Security.Claims;
using ADE.ESS.SEPA.Common.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;

namespace ADE.ESS.SEPA.Client.Auth;

public sealed class ESSUserSecurityManager(
    IHttpContextAccessor httpAccessor,
    AuthenticationStateProvider authStateProvider,
    IOptions<AdeConnectOptions> options) : IESSUserSecurityManager
{
    private readonly AdeConnectOptions _options = options.Value;

    private ClaimsPrincipal? _cachedUser;
    private IReadOnlyList<ESSUserPermission> _cachedPermissions = [];

    public string GetEmail() => GetClaim(_options.EmailAddressClaim);

    public string GetFirstName() => GetClaim(_options.FirstNameClaim);

    public string GetLastName() => GetClaim(_options.LastNameClaim);

    public string GetDepartment() => GetClaim(_options.DepartmentClaim);

    public string GetFullName() => $"{GetFirstName()} {GetLastName()}".Trim();

    public IList<ESSUserPermission> GetEntityPermissions(int edOrg = 0) =>
        [.. GetPermissions().Where(p => MatchesEntity(p, edOrg))];

    public bool IsUserAuthorized(int edOrgId, ESSUserPermissionsEnum permission) =>
        IsUserAuthorized(edOrgId, [permission]);

    public bool IsUserAuthorized(int edOrgId, IReadOnlyCollection<ESSUserPermissionsEnum> permissions) =>
        HasAnyPermission(AdeConstants.AdeEntityId, permissions) || HasAnyPermission(edOrgId, permissions);

    public bool IsUserUIAuthorized(int edOrgId, IReadOnlyCollection<ESSUserPermissionsEnum> permissions) =>
        IsUserAuthorized(edOrgId, permissions);

    private bool HasAnyPermission(int edOrgId, IReadOnlyCollection<ESSUserPermissionsEnum> permissions) =>
        GetPermissions().Any(p => MatchesEntity(p, edOrgId) && permissions.Contains(p.Permission));

    private static bool MatchesEntity(ESSUserPermission permission, int edOrgId) =>
        edOrgId == 0 || permission.EntityId == edOrgId;

    private IReadOnlyList<ESSUserPermission> GetPermissions()
    {
        var user = GetCurrentUser();
        if (user is null)
            return [];

        if (!ReferenceEquals(user, _cachedUser))
        {
            _cachedPermissions = ParsePermissions(user);
            _cachedUser = user;
        }

        return _cachedPermissions;
    }

    private List<ESSUserPermission> ParsePermissions(ClaimsPrincipal user)
    {
        var permissions = new List<ESSUserPermission>();
        foreach (var value in GetClaimValues(user, _options.EntityPermissionClaim).Distinct())
        {
            if (value.Split(',') is [var entity, var description]
                && int.TryParse(entity.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var entityId)
                && EnumExtensions.TryParseEnumDescription(description.Trim(), out ESSUserPermissionsEnum permission))
            {
                permissions.Add(new ESSUserPermission { EntityId = entityId, Permission = permission });
            }
        }

        return permissions;
    }

    private string GetClaim(string claimType) =>
        GetCurrentUser()?.FindFirst(c => IsClaimType(c, claimType))?.Value ?? string.Empty;

    private static IEnumerable<string> GetClaimValues(ClaimsPrincipal user, string claimType) =>
        user.FindAll(c => IsClaimType(c, claimType)).Select(c => c.Value);

    private static bool IsClaimType(Claim claim, string claimType) =>
        claim.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase);

    private ClaimsPrincipal? GetCurrentUser()
    {
        try
        {
            var authState = authStateProvider.GetAuthenticationStateAsync();
            if (authState.IsCompletedSuccessfully)
                return authState.Result.User;
        }
        catch (InvalidOperationException)
        {
        }

        return httpAccessor.HttpContext?.User;
    }
}
