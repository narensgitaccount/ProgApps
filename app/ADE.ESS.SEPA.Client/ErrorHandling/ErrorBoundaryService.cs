namespace ADE.ESS.SEPA.Client.ErrorHandling
{
    public class ErrorBoundaryService
    {
        private readonly IErrorService _errorService;

        public ErrorBoundaryService(IErrorService errorService)
        {
            _errorService = errorService;
        }

        public async Task<string> HandleErrorAsync(Exception exception, string componentName)
        {
            return await _errorService.LogErrorAsync(exception, additionalInfo: $"Component: {componentName}");
        }
    }
}
