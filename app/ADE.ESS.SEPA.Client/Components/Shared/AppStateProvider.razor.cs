using ADE.ESS.SEPA.Application.Interfaces;
using ADE.ESS.SEPA.Application.Models;
using ADE.ESS.SEPA.Client.Auth;
using ADE.ESS.SEPA.Client.ErrorHandling;
using ADE.ESS.SEPA.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace ADE.ESS.SEPA.Client.Components.Shared;

public sealed partial class AppStateProvider : IDisposable
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IOptions<AdeConnectOptions> Options { get; set; } = default!;
    [Inject] private IAppStateService AppState { get; set; } = default!;
    [Inject] private IEntityContextResolver EntityResolver { get; set; } = default!;
    [Inject] private IBrowserSessionService BrowserSession { get; set; } = default!;
    [Inject] private IErrorService ErrorService { get; set; } = default!;
    [Inject] private ILogger<AppStateProvider> Logger { get; set; } = default!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    private bool _isReady;
    private string? _errorMessage;
    private string? _errorCorrelationId;

    protected override async Task OnInitializedAsync()
    {
        AppState.StateChanged += OnStateChanged;
        AppState.UserStateChanged += OnUserStateChanged;
        AppState.EdOrgStateChanged += OnEdOrgStateChanged;

        var entityId = 0;
        try
        {
            await BrowserSession.ResetIfNewSessionAsync();

            var resolution = await EntityResolver.ResolveAsync(Navigation.ToAbsoluteUri(Navigation.Uri));
            entityId = resolution.EntityId;

            if (!resolution.IsResolved)
            {
                RedirectToAdeConnect();
                return;
            }

            await InitializeStateAsync(resolution);

            _isReady = true;
            Logger.LogInformation("AppStateProvider initialization completed for EntityId: {EntityId}", entityId);
        }
        catch (UnauthorizedAccessException ex)
        {
            await ShowErrorAsync(ex, $"EntityId: {entityId}", "You do not have permission to access this application.");
        }
        catch (Exception ex)
        {
            await ShowErrorAsync(ex, "AppStateProvider initialization", "An unexpected error occurred during initialization. Please try again.");
        }
    }

    private Task InitializeStateAsync(EntityResolution resolution) =>
        resolution.Source == EntityResolutionSource.ExistingState
            ? AppState.RefreshStateAsync()
            : AppState.InitializeAppStateAsync(resolution.EntityId);

    private void RedirectToAdeConnect()
    {
        var adeConnectUrl = Options.Value.AdeConnectUrl;
        if (string.IsNullOrWhiteSpace(adeConnectUrl))
        {
            Logger.LogError("No EntityID available and WsFederation:ADEConnectURL is not configured");
            _errorMessage = "No EntityID available and ADE Connect URL not configured.";
            return;
        }

        Logger.LogWarning("No EntityID found in query string, storage, or claims, redirecting to ADE Connect");
        Navigation.NavigateTo(adeConnectUrl, forceLoad: true);
    }

    private async Task ShowErrorAsync(Exception exception, string additionalInfo, string userMessage)
    {
        _errorCorrelationId = await ErrorService.LogErrorAsync(exception, additionalInfo: additionalInfo);
        _errorMessage = userMessage;
    }

    private void OnStateChanged(AppStateStore _) => Refresh();

    private void OnUserStateChanged(UserState _) => Refresh();

    private void OnEdOrgStateChanged(EdOrgState _) => Refresh();

    private void Refresh() => _ = InvokeAsync(StateHasChanged);

    public void Dispose()
    {
        AppState.StateChanged -= OnStateChanged;
        AppState.UserStateChanged -= OnUserStateChanged;
        AppState.EdOrgStateChanged -= OnEdOrgStateChanged;
    }
}
