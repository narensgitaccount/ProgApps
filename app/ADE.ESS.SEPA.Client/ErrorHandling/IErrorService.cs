namespace ADE.ESS.SEPA.Client.ErrorHandling
{
    public interface IErrorService
    {
        Task<string> LogErrorAsync(Exception exception, HttpContext? httpContext = null, string? additionalInfo = null);
        Task<string> LogErrorAsync(string message, string? additionalInfo = null);
    }
}
