using ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies;
using ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies.Handlers;
using ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace ADE.ESS.SEPA.Client.Auth;

public static class ServiceAuthExtension
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicyConstants.LEAApprover, policy =>
                policy.Requirements.Add(new AuthorizedPermissionsRequirement(ESSUserPermissionsEnum.ESSPostSchoolOutcomes)));
        });
        services.AddScoped<IAuthorizationHandler, AuthorizedPermissionsHandler>();

        return services;
    }
}
