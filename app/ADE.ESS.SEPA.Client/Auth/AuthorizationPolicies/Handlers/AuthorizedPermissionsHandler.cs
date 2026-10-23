using ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies.Handlers
{
    public class AuthorizedPermissionsHandler : AuthorizationHandler<AuthorizedPermissionsRequirementBase>
    {
        private readonly IESSUserSecurityManager _securityManager;

        private readonly IHttpContextAccessor _httpaccessor;
        public AuthorizedPermissionsHandler(IESSUserSecurityManager securityManager, IHttpContextAccessor httpaccessor)
        {
            _securityManager = securityManager;
            _httpaccessor = httpaccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizedPermissionsRequirementBase requirement)
        {
            var httpContext = _httpaccessor.HttpContext;

            int entityId = httpContext?.Session.GetInt32("DefaultedEntityId") ?? 0;

            if (requirement is AuthorizedPermissionsUIRequirement)
            {
                if (!_securityManager.IsUserUIAuthorized(entityId, requirement._permissionsEnum))
                {
                    context.Fail();
                    return Task.CompletedTask;
                }
            }
            if (!_securityManager.IsUserAuthorized(entityId, requirement._permissionsEnum))
            {
                context.Succeed(requirement);
                if (httpContext != null && !httpContext.Response.HasStarted)
                {
                    var accessDeniedPath = string.Empty;
                    if (httpContext.Request.Host.Host.Contains("localhost"))
                    {
                        accessDeniedPath = httpContext.Request.PathBase.Value;
                    }
                    httpContext.Response.Redirect($"{accessDeniedPath}/Error?errorMessageConstant=AccessDenied");
                }

                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
