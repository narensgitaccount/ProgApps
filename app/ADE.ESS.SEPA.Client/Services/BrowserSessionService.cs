using Microsoft.JSInterop;

namespace ADE.ESS.SEPA.Client.Services;

public sealed class BrowserSessionService(IJSRuntime js, ILogger<BrowserSessionService> logger)
    : IBrowserSessionService, IAsyncDisposable
{
    private const string ModulePath = "./js/browser-session.js";

    private IJSObjectReference? _module;

    public async ValueTask<bool> ResetIfNewSessionAsync()
    {
        _module ??= await js.InvokeAsync<IJSObjectReference>("import", ModulePath);

        var isNewSession = await _module.InvokeAsync<bool>("isNewBrowserSession");
        if (!isNewSession)
            return false;

        logger.LogInformation("New browser session detected, clearing stale local storage");
        await _module.InvokeVoidAsync("clearLocalStorageKeys", [AppStorageKeys.All]);
        return true;
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is null)
            return;

        try
        {
            await _module.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
        }
    }
}
