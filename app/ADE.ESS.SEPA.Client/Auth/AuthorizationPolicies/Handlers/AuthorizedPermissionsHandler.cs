using ADE.ESS.SEPA.Application.Interfaces;
using ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies.Handlers;

public sealed class AuthorizedPermissionsHandler(
    IESSUserSecurityManager securityManager,
    IAppStateService appState) : AuthorizationHandler<AuthorizedPermissionsRequirementBase>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AuthorizedPermissionsRequirementBase requirement)
    {
        var entityId = appState.CurrentState.EdOrg.EntityId;

        var isAuthorized = requirement is AuthorizedPermissionsUIRequirement
            ? securityManager.IsUserUIAuthorized(entityId, requirement.Permissions)
            : securityManager.IsUserAuthorized(entityId, requirement.Permissions);

        if (isAuthorized)
            context.Succeed(requirement);
        else
            context.Fail();

        return Task.CompletedTask;
    }
}
