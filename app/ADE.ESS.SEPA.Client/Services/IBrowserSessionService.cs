namespace ADE.ESS.SEPA.Client.Services;

public interface IBrowserSessionService
{
    ValueTask<bool> ResetIfNewSessionAsync();
}
