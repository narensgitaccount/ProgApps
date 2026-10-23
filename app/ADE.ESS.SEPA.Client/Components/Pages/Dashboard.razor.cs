using ADE.ESS.SEPA.Application.DTOs;
using ADE.ESS.SEPA.Application.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ADE.ESS.SEPA.Client.Components.Pages
{
    public partial class Dashboard
    {
        private ClaimsPrincipal? CurrentUser;
        private PrivateSchoolApplicationDto? application;
        private bool IsLoading = true;
        protected override async Task OnInitializedAsync()
        {
           

        }
        private string GetClaim(string claimType)
        {
            return CurrentUser?.FindFirst(claimType)?.Value ?? "(not found)";
        }
    }
}
