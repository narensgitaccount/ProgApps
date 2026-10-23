namespace ADE.ESS.SEPA.Client.ErrorHandling
{
    public class ErrorService : IErrorService
    {
        private readonly ILogger<ErrorService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ErrorService(ILogger<ErrorService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> LogErrorAsync(Exception exception, HttpContext? httpContext = null, string? additionalInfo = null)
        {
            var correlationId = GenerateCorrelationId();
            var context = httpContext ?? _httpContextAccessor.HttpContext;

            using var scope = _logger.BeginScope(BuildScopeData(correlationId, context));
            _logger.LogError(exception,
                "Error occurred. CorrelationId: {CorrelationId}. Additional Info: {AdditionalInfo}",
                correlationId, additionalInfo ?? "None");

            return correlationId;
        }

        public async Task<string> LogErrorAsync(string message, string? additionalInfo = null)
        {
            var correlationId = GenerateCorrelationId();
            var context = _httpContextAccessor.HttpContext;

            using var scope = _logger.BeginScope(BuildScopeData(correlationId, context));
            _logger.LogError("Manual error log. CorrelationId: {CorrelationId}. Message: {Message}. Additional Info: {AdditionalInfo}",
                correlationId, message, additionalInfo ?? "None");

            return correlationId;
        }

        private static string GenerateCorrelationId() =>
            Guid.NewGuid().ToString("N")[..12];

        private static Dictionary<string, object> BuildScopeData(string correlationId, HttpContext? context)
        {
            return new Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId,
                ["UserName"] = context?.User?.Identity?.Name ?? "Anonymous",
                ["RequestPath"] = context?.Request?.Path.HasValue == true
                    ? context.Request.Path.Value
                    : "/Unknown",
                ["UserAgent"] = context?.Request?.Headers["User-Agent"].FirstOrDefault() ?? "Unknown",
                ["RemoteIpAddress"] = context?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown"
            };
        }
    }
}

