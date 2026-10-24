using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.Extensions.Options;

namespace ADE.ESS.SEPA.Client.Auth;

public sealed class AdeConnectAuthMiddleware(
    RequestDelegate next,
    IOptions<AdeConnectOptions> options,
    bool useFakeAuth)
{
    private const string SignOutPath = "/signout";
    private const string CookieScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments(SignOutPath, StringComparison.OrdinalIgnoreCase))
        {
            await SignOutAsync(context);
            return;
        }

        if (useFakeAuth)
            await EnsureFakeUserSignedInAsync(context);

        if (!useFakeAuth && !context.User.Identities.Any(identity => identity.IsAuthenticated))
        {
            await context.ChallengeAsync();
            return;
        }

        await next(context);
    }

    private async Task SignOutAsync(HttpContext context)
    {
        await context.SignOutAsync(CookieScheme);

        foreach (var cookie in context.Request.Cookies)
        {
            context.Response.Cookies.Delete(cookie.Key);
        }

        if (useFakeAuth)
        {
            context.Response.Redirect("/");
            return;
        }

        await context.SignOutAsync(WsFederationDefaults.AuthenticationScheme, new AuthenticationProperties
        {
            RedirectUri = options.Value.SignoutUrl
        });
    }

    private async Task EnsureFakeUserSignedInAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
            return;

        var principal = FakeAuthDefaults.CreatePrincipal(options.Value, CookieScheme);
        await context.SignInAsync(CookieScheme, principal);
        context.User = principal;
    }

}
