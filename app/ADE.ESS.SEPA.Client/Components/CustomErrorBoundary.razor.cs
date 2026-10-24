using ADE.ESS.SEPA.Client.ErrorHandling;
using Microsoft.AspNetCore.Components;

namespace ADE.ESS.SEPA.Client.Components;

public partial class CustomErrorBoundary
{
    private string? _correlationId;

    [Inject] private ErrorBoundaryService ErrorBoundaryService { get; set; } = default!;

    protected override async Task OnErrorAsync(Exception exception)
    {
        _correlationId = await ErrorBoundaryService.HandleErrorAsync(exception, nameof(CustomErrorBoundary));
        await base.OnErrorAsync(exception);
    }
}
