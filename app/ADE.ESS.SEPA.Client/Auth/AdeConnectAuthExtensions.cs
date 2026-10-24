using ADE.ESS.SEPA.Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;

namespace ADE.ESS.SEPA.Client.Auth;

public static class AdeConnectAuthExtensions
{
    public const string UseFakeAuthSetting = "UseFakeAuth";

    private const string AccessDeniedPath = "/Error?errorMessageConstant=AccessDenied";

    public static IServiceCollection AddAdeConnectAuth(
        this IServiceCollection services,
        IConfiguration configuration,
        bool useFakeAuth)
    {
        services.AddOptions<AdeConnectOptions>()
            .BindConfiguration(AdeConnectOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHttpContextAccessor();
        services.AddScoped<IEntityContextResolver, EntityContextResolver>();
        services.AddScoped<IBrowserSessionService, BrowserSessionService>();

        if (useFakeAuth)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.AccessDeniedPath = AccessDeniedPath);
            services.AddScoped<IESSUserSecurityManager, FakeUserSecurityManager>();
        }
        else
        {
            AddWsFederation(services, configuration);
            services.AddScoped<IESSUserSecurityManager, ESSUserSecurityManager>();
        }

        return services.AddAuthorizationPolicies();
    }

    public static IApplicationBuilder UseAdeConnectAuth(this IApplicationBuilder app, bool useFakeAuth) =>
        app.UseMiddleware<AdeConnectAuthMiddleware>(useFakeAuth);

    private static void AddWsFederation(IServiceCollection services, IConfiguration configuration)
    {
        var adeConnect = configuration.GetSection(AdeConnectOptions.SectionName).Get<AdeConnectOptions>();
        if (string.IsNullOrWhiteSpace(adeConnect?.Wtrealm) || string.IsNullOrWhiteSpace(adeConnect.MetadataAddress))
        {
            throw new InvalidOperationException(
                $"{AdeConnectOptions.SectionName}:Wtrealm and {AdeConnectOptions.SectionName}:MetadataAddress are required when {UseFakeAuthSetting} is false.");
        }

        services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignOutScheme = WsFederationDefaults.AuthenticationScheme;
            })
            .AddWsFederation(options =>
            {
                options.Wtrealm = adeConnect.Wtrealm;
                options.MetadataAddress = adeConnect.MetadataAddress;
                options.SignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.TokenHandlers.Clear();
                options.TokenHandlers.Add(new CustomSamlSecurityTokenHandler());
            })
            .AddCookie(options => options.AccessDeniedPath = AccessDeniedPath);
    }
}
