using ADE.ESS.SEPA.Application.Interfaces;
using ADE.ESS.SEPA.Application.Models;
using ADE.ESS.SEPA.Application.Services;
using ADE.ESS.SEPA.Client.Auth;
using ADE.ESS.SEPA.Client.Components;
using ADE.ESS.SEPA.Client.Configurations;
using ADE.ESS.SEPA.Client.ErrorHandling;
using ADE.ESS.SEPA.Infrastructure.Context;
using ADE.ESS.SEPA.Infrastructure.DBRepositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddTelerikBlazor();

var useFakeAuth = builder.Configuration.GetValue<bool>(AdeConnectAuthExtensions.UseFakeAuthSetting);

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

builder.Services.AddAdeConnectAuth(builder.Configuration, useFakeAuth);

builder.Services.AddScoped<IErrorService, ErrorService>();
builder.Services.AddScoped<ErrorBoundaryService>();

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
app.UseAdeConnectAuth(useFakeAuth);

app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
