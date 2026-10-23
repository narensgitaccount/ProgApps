using ADE.ESS.SEPA.Application.Interfaces;
using ADE.ESS.SEPA.Application.Models;
using ADE.ESS.SEPA.Application.Services;
using ADE.ESS.SEPA.Client.Auth;
using ADE.ESS.SEPA.Client.Components;
using ADE.ESS.SEPA.Client.Configurations;
using ADE.ESS.SEPA.Infrastructure.Context;
using ADE.ESS.SEPA.Infrastructure.DBRepositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddTelerikBlazor();

var configbuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
IConfiguration _configuration = configbuilder.Build();
var useFakeAuth = _configuration.GetValue<bool>("UseFakeAuth");
var realm = _configuration.GetSection("WsFederation:Wtrealm").Value;
var metadataAddress = _configuration.GetSection("WsFederation:MetadataAddress").Value;

var conString = builder.Configuration.GetConnectionString("ESSPrivateEDPEntities");
var sqlConnectionConfiguration = new SqlConnectionConfiguration(value: conString ?? string.Empty);
builder.Services.AddDbContext<EssSepaDbContext>(options =>
{
    options.UseSqlServer(conString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

}, ServiceLifetime.Transient);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<AppStateStore>();
if (useFakeAuth)
{
    builder.Services.AddScoped<IAppStateService, ADE.ESS.SEPA.Client.Services.FakeAppStateService>();
}
else
{
    builder.Services.AddScoped<IAppStateService, ADE.ESS.SEPA.Client.Services.AppStateService>();
}

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    // Make the session cookie essential
    options.Cookie.IsEssential = true;
});

if (useFakeAuth)
{
    // Fake auth: no WS-Federation, just cookie auth with auto-sign-in
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie();
    builder.Services.AddTransient<IESSUserSecurityManager, FakeUserSecurityManager>();
}
else
{
    builder.Services.AddAuthentication(sharedOptions =>
    {
        sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
        sharedOptions.DefaultSignOutScheme = WsFederationDefaults.AuthenticationScheme;
    })
                    .AddWsFederation(options =>
                    {
                        options.Wtrealm = realm;
                        options.MetadataAddress = metadataAddress;
                        options.SignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.TokenHandlers.Clear();
                        options.TokenHandlers.Add(new CustomSamlSecurityTokenHandler());
                        options.SignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    })
                    .AddCookie();
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;
    });
    builder.Services.AddTransient<IESSUserSecurityManager, ESSUserSecurityManager>();
}
var app = builder.Build();
//test db connection
using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EssSepaDbContext>();
    var canconnect = await db.Database.CanConnectAsync();

}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseSession();
//ADEConnect
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/signout", StringComparison.OrdinalIgnoreCase))
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

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
            RedirectUri = _configuration.GetSection("WsFederation:SignoutURL").Value
        });

        return;
    }
    if (useFakeAuth)
    {
        // Fake auth: auto-sign in with a dummy identity if not authenticated
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            var claims = new List<System.Security.Claims.Claim>
            {
                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "fake.user@azed.gov"),
                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "Fake"),
                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname", "User"),
                new("http://www.azed.gov/claims/Department", "Exceptional Student Services"),
                new("http://www.azed.gov/claims/entitypermission", "4235,ESS Post School Outcomes")
            };
            var identity = new System.Security.Claims.ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new System.Security.Claims.ClaimsPrincipal(identity);
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            context.User = principal;
        }

        // Set default entity ID for fake auth
        bool isQueryStringId = int.TryParse(context.Request.Query["EntityID"].ToString(), out int qsEdOrgID);
        if (isQueryStringId)
        {
            context.Session.SetInt32("DefaultedEntityId", qsEdOrgID);
        }
        else if (context.Session.GetInt32("DefaultedEntityId") == null)
        {
            context.Session.SetInt32("DefaultedEntityId", 4235);
        }

        await next.Invoke(context);
    }

    else
    {
        var user = context.User;
        bool isQueryStringId = int.TryParse(context.Request.Query["EntityID"].ToString(), out int qsEdOrgID);
        if (isQueryStringId)
        {
            context.Session.SetInt32("DefaultedEntityId", qsEdOrgID);
        }
        if (user?.Identities.Any(identity => identity.IsAuthenticated) != true)
            await context.ChallengeAsync();
        else
            await next.Invoke(context);
    }
    //else
    //{
    //    if (context.Request.Path.Equals("/signout"))
    //    {
    //        // Redirects
    //        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    //        foreach (var cookie in context.Request.Cookies)
    //        {
    //            context.Response.Cookies.Delete(cookie.Key);
    //        }
    //        await context.SignOutAsync(WsFederationDefaults.AuthenticationScheme, new AuthenticationProperties()
    //        {
    //            RedirectUri = _configuration.GetSection("WsFederation:SignoutURL").Value
    //        });
    //    }
    //    var user = context.User;
    //    bool isQueryStringId = int.TryParse(context.Request.Query["EntityID"].ToString(), out int qsEdOrgID);
    //    if (isQueryStringId)
    //    {
    //        context.Session.SetInt32("DefaultedEntityId", qsEdOrgID);
    //    }
    //    if (user?.Identities.Any(identity => identity.IsAuthenticated) != true)
    //        await context.ChallengeAsync();
    //    else
    //        await next.Invoke(context);
    //}
});

app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
