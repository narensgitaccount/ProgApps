namespace ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies.Requirements;

public sealed class AuthorizedPermissionsUIRequirement : AuthorizedPermissionsRequirementBase
{
    public AuthorizedPermissionsUIRequirement(ESSUserPermissionsEnum permission) : base(permission)
    {
    }

    public AuthorizedPermissionsUIRequirement(IReadOnlyCollection<ESSUserPermissionsEnum> permissions) : base(permissions)
    {
    }
}
