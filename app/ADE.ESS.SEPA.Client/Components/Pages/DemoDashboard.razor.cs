using ADE.ESS.SEPA.Application.DTOs;
using ADE.ESS.SEPA.Application.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ADE.ESS.SEPA.Client.Components.Pages
{
    public partial class DemoDashboard
    {
        private ClaimsPrincipal? CurrentUser;
        private PrivateSchoolApplicationDto? application;
        private bool IsLoading = true;
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            CurrentUser = authenticationState.User;
            try
            {
                application = await DashboardService.GetPrivateSchoolApplication(1139);
            }
            finally
            {
                IsLoading = false;
            }

        }
        private string GetClaim(string claimType)
        {
            return CurrentUser?.FindFirst(claimType)?.Value ?? "(not found)";
        }
    }
}
