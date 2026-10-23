using Serilog.Context;

namespace ADE.ESS.SEPA.Client.ErrorHandling
{
    public class LogEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;

        public LogEnrichmentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userName = context.User?.Identity?.Name ?? "Anonymous";
            var requestPath = context.Request?.Path.HasValue == true
                ? context.Request.Path.Value
                : "/Unknown";
            var remoteIp = context.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";

            using (LogContext.PushProperty("UserName", userName))
            using (LogContext.PushProperty("RequestPath", requestPath))
            using (LogContext.PushProperty("RemoteIpAddress", remoteIp))
            {
                await _next(context);
            }
        }
    }
}
