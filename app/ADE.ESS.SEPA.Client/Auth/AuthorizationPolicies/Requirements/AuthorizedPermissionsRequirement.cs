namespace ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies.Requirements;

public sealed class AuthorizedPermissionsRequirement : AuthorizedPermissionsRequirementBase
{
    public AuthorizedPermissionsRequirement(ESSUserPermissionsEnum permission) : base(permission)
    {
    }

    public AuthorizedPermissionsRequirement(IReadOnlyCollection<ESSUserPermissionsEnum> permissions) : base(permissions)
    {
    }
}
