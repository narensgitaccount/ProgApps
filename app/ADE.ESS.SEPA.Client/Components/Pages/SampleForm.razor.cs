using ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ADE.ESS.SEPA.Client.Components.Pages;

public partial class SampleForm
{
    private const string InitialReferenceCode = "REF-1001";

    private readonly FormModel _model = new() { ReferenceCode = InitialReferenceCode };
    private string _savedReferenceCode = InitialReferenceCode;
    private bool _canEditReference;
    private string? _message;

    [Inject] private IAuthorizationService AuthorizationService { get; set; } = default!;

    [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    protected override async Task OnInitializedAsync() =>
        _canEditReference = !await IsAdminAsync();

    private async Task SubmitAsync()
    {
        _canEditReference = !await IsAdminAsync();

        if (_canEditReference)
            _savedReferenceCode = _model.ReferenceCode;
        else
            _model.ReferenceCode = _savedReferenceCode;

        _message = $"Saved {_model.Name} with reference code {_savedReferenceCode}.";
    }

    private async Task<bool> IsAdminAsync()
    {
        var user = (await AuthenticationState).User;
        var result = await AuthorizationService.AuthorizeAsync(user, AuthPolicyConstants.ESSAdmin);
        return result.Succeeded;
    }

    private sealed class FormModel
    {
        public string Name { get; set; } = string.Empty;

        public string ReferenceCode { get; set; } = string.Empty;
    }
}
