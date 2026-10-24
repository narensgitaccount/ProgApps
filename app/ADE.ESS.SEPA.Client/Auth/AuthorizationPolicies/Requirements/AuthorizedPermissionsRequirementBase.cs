using Microsoft.AspNetCore.Authorization;

namespace ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies.Requirements;

public abstract class AuthorizedPermissionsRequirementBase : IAuthorizationRequirement
{
    protected AuthorizedPermissionsRequirementBase(ESSUserPermissionsEnum permission) =>
        Permissions = [permission];

    protected AuthorizedPermissionsRequirementBase(IReadOnlyCollection<ESSUserPermissionsEnum> permissions) =>
        Permissions = permissions;

    public IReadOnlyCollection<ESSUserPermissionsEnum> Permissions { get; }
}
