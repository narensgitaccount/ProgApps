using ADE.ESS.SEPA.Application.DTOs;
using ADE.ESS.SEPA.Application.Interfaces;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using System.Security.Claims;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ADE.ESS.SEPA.Client.Components.Pages;

public partial class CorpAppDashboard
{
    // ------------------------------------------------------------
    // Services
    // ------------------------------------------------------------

    [Inject]
    private IDashboardService DashboardService { get; set; } = default!;
    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject]
    private NavigationManager Navigation { get; set; } = default!;
    //[Inject]
    //private ILogger<CorpAppDashboard> Logger { get; set; } = default!;


    // ------------------------------------------------------------
    // Route parameters
    // ------------------------------------------------------------

    [Parameter]
    [SupplyParameterFromQuery(Name = "appType")]
    public int? AppType { get; set; }
    [Parameter]
    [SupplyParameterFromQuery(Name = "applicationId")]
    public int? ApplicationID { get; set; }

    // ------------------------------------------------------------
    // User
    // ------------------------------------------------------------

    private ClaimsPrincipal? CurrentUser;
    private int EntityId;
    private bool IsAdmin;
    private bool IsLeaUser;

    // ------------------------------------------------------------
    // Application
    // ------------------------------------------------------------

    private PrivateSchoolApplicationDto? Application1;
    private ApplicationListDto? Application;
    private string CorporateName = string.Empty;
    private string ApplicationStatus = string.Empty;
    private string ApplicationTypeName = string.Empty;

    // ------------------------------------------------------------
    // Loading / error
    // ------------------------------------------------------------

    private bool IsLoading = true;
    private bool IsHistoryLoading;
    private bool HasError;
    private string ErrorMessage = string.Empty;


    // ------------------------------------------------------------
    // Fiscal year
    // ------------------------------------------------------------
    private int ActiveFiscalYear;
    private int SelectedFiscalYear;
    private bool IsActiveFiscalYear;

    // ------------------------------------------------------------
    // Sites
    // ------------------------------------------------------------
    private List<SiteApplicationCardDto> SiteApplicationCardList = [];
    private List<SiteApplicationCardDto> FilteredSiteApplicationCardList = [];
    private string SiteSearchText = string.Empty;
    private int SitePageSize = 12;
    private int SiteCurrentPage = 1;
    private string SiteSortField = "SiteName";
    private bool SiteSortDescending;

    // ------------------------------------------------------------
    // Application history
    // ------------------------------------------------------------
    private List<ApplicationHistoryDto> ApplicationHistory = [];
    private bool HasApplicationHistory;

    // ------------------------------------------------------------
    // Actions
    // ------------------------------------------------------------
    private List<ApplicationActionDto> AvailableActions = [];
    private ApplicationActionDto? SelectedApplicationAction;

    // ------------------------------------------------------------
    // Assurance
    // ------------------------------------------------------------

    private string AssuranceStatus = "Not Completed";

    // ------------------------------------------------------------
    // Documents
    // ------------------------------------------------------------
    private readonly List<string> AllowedDocumentExtensions = [".pdf", ".doc", ".docx", ".xls", ".xlsx"];

    // ------------------------------------------------------------
    // Lifecycle
    // ------------------------------------------------------------

    protected override async Task OnInitializedAsync()
    {
        BuildBreadcrumbs();
        await LoadApplicationAsync();
    }


    // ------------------------------------------------------------
    // Load application
    // ------------------------------------------------------------

    private async Task LoadApplicationAsync()
    {
        IsLoading = true;
        HasError = false;
        ErrorMessage = string.Empty;
        Application = null;
        SiteApplicationCardList = [];
        FilteredSiteApplicationCardList = [];
        AvailableActions = [];
        try
        {
            // Authentication
            //var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            //CurrentUser = authenticationState.User;
            EntityId = GetClaimValue("EntityId");
            // Validate application ID
            if (ApplicationID is null or <= 0)
            {
                throw new InvalidOperationException("Application ID was not provided.");
            }
            // Load application
            switch (AppType)
            {
                case 1:
                    Application = await DashboardService.GetCorpAppDashboard(ApplicationID.Value);
                    ApplicationTypeName = "Private School";
                    break;

                case 2:

                    // TODO:
                    // Connect public school application
                    // when its service method is available.
                    throw new NotSupportedException("Public school application is not implemented.");

                default:

                    throw new InvalidOperationException($"Invalid application type: {AppType}");
            }


            // Validate application
            if (Application is null)
            {
                throw new InvalidOperationException("Application was not found.");
            }
            // Application information
            CorporateName = Application.CorporateName ?? string.Empty;
            EntityId = Application.EntityID ?? 0;
            ApplicationStatus = Application.Status.ToString();
            // Fiscal year
            ActiveFiscalYear = await DashboardService.GetActiveFiscalYearAsync();
            SelectedFiscalYear = ActiveFiscalYear;
            IsActiveFiscalYear = SelectedFiscalYear == ActiveFiscalYear;
            // Load current sites
            var applicationKey = Application.ApplicationKey ?? 0;
            if (applicationKey > 0)
            {
                SiteApplicationCardList = await DashboardService.GetSiteApplicationList(applicationKey);
            }
            // Initialize site search
            ApplySiteFilter();
            // User role:
            // TODO:
            // Replace these with your actual claims/role names.
            IsAdmin = CurrentUser?.IsInRole("Admin") ?? false;

            IsLeaUser = CurrentUser?.IsInRole("LEAUser") ?? false;
            IsLeaUser = true; //for testing TODO remove

            // Build actions
            BuildAvailableActions();
            // History will be connected when the history service
            // method is available.
        }
        catch (Exception ex)
        {
            //HasError = true;
            ErrorMessage = "The application could not be loaded. " + "Please try again.";
            //Logger.LogError(ex, "Failed to load CorpAppDashboard. Applicationid: {ApplicationID}, AppType: {AppType}, EntityID: {EntityId}", ApplicationID, AppType, EntityId);
            // TODO:
            // Log the exception using your existing logging service.
            // Do not expose database exception details to the UI.
        }
        finally
        {
            IsLoading = false;
        }
    }


    // ------------------------------------------------------------
    // Claims
    // ------------------------------------------------------------

    private string GetClaim(string claimType)
    {
        return CurrentUser?.FindFirst(claimType)?.Value ?? string.Empty;
    }

    private int GetClaimValue(string claimType)
    {
        var claimValue = GetClaim(claimType);
        return int.TryParse(claimValue, out var value) ? value : 0;
    }


    // ------------------------------------------------------------
    // Site search
    // ------------------------------------------------------------

    //private void ApplySiteFilter()
    //{
    //    if (string.IsNullOrWhiteSpace(SiteSearchText))
    //    {
    //        FilteredSiteApplicationCardList =                SiteApplicationCardList.ToList();

    //        return;
    //    }

    //    var search = SiteSearchText.Trim();

    //    FilteredSiteApplicationCardList =
    //        SiteApplicationCardList
    //            .Where(site =>
    //                ContainsIgnoreCase(
    //                    site.SiteName,
    //                    search) ||

    //                ContainsIgnoreCase(
    //                    site.SiteID.ToString(),
    //                    search) ||

    //                ContainsIgnoreCase(
    //                    site.StatusDescription,
    //                    search) ||

    //                ContainsIgnoreCase(
    //                    site.ContactName,
    //                    search) ||

    //                ContainsIgnoreCase(
    //                    site.ContactEmailAddress,
    //                    search) ||

    //                ContainsIgnoreCase(
    //                    site.ContactPhone,
    //                    search) ||

    //                ContainsIgnoreCase(
    //                    site.Grades,
    //                    search) ||

    //                site.NeedPrograms.Any(program =>
    //                    ContainsIgnoreCase(
    //                        program,
    //                        search)))
    //            .ToList();
    //}

    private static bool ContainsIgnoreCase(string? source, string search)
    {
        return !string.IsNullOrWhiteSpace(source) && source.Contains(search, StringComparison.OrdinalIgnoreCase);
    }

    //private void OnSiteSearchChanged(string value)
    //{
    //    SiteSearchText = value;

    //    ApplySiteFilter();
    //}


    // ------------------------------------------------------------
    // Actions
    // ------------------------------------------------------------

    private void BuildAvailableActions()
    {
        AvailableActions = [];
        if (!IsActiveFiscalYear)
        {
            return;
        }

        if (Application is null)
        {
            return;
        }

        var status = Application.Status;

        if (IsAdmin)
        {
            BuildAdminActions(status);
            return;
        }

        if (IsLeaUser)
        {
            BuildLeaActions(status);
        }
    }

    private void BuildAdminActions(int status)
    {
        switch (status)
        {
            case 2:

                AvailableActions.Add(
                    new ApplicationActionDto
                    {
                        ActionID = 1,
                        Description = "Approve",
                        TargetStatusID = 6,
                        RequiresConfirmation = true
                    });

                AvailableActions.Add(
                    new ApplicationActionDto
                    {
                        ActionID = 2,
                        Description = "Return to User",
                        TargetStatusID = 1,
                        RequiresConfirmation = true
                    });

                break;

            case 6:

                AvailableActions.Add(
                    new ApplicationActionDto
                    {
                        ActionID = 3,
                        Description = "Revoke Approval",
                        TargetStatusID = 11,
                        RequiresConfirmation = true
                    });

                break;
        }
    }

    private void BuildLeaActions(int status)
    {
        switch (status)
        {
            case 1:

                AvailableActions.Add(
                    new ApplicationActionDto
                    {
                        ActionID = 4,
                        Description = "Submit",
                        TargetStatusID = 2,
                        RequiresConfirmation = true
                    });

                if (HasApplicationHistory)
                {
                    AvailableActions.Add(
                        new ApplicationActionDto
                        {
                            ActionID = 5,
                            Description = "Delete",
                            TargetStatusID = 0,
                            RequiresConfirmation = true
                        });
                }

                break;

            case 2:

                AvailableActions.Add(
                    new ApplicationActionDto
                    {
                        ActionID = 6,
                        Description = "Recall",
                        TargetStatusID = 1,
                        RequiresConfirmation = true
                    });

                break;

            case 6:
            case 9:

                AvailableActions.Add(
                    new ApplicationActionDto
                    {
                        ActionID = 7,
                        Description = "Edit",
                        TargetStatusID = status,
                        RequiresConfirmation = false
                    });

                break;
        }
    }

    private async Task OnApplicationActionClicked(int actionID)
    {
        if (actionID is 0)
        {
            return;
        }

        var action = AvailableActions
            .FirstOrDefault(item =>
                item.ActionID == actionID);

        if (action is null)
        {
            return;
        }

        await OnApplicationSubmitActionAsync(action);
    }

    private Task OnApplicationSubmitActionAsync(ApplicationActionDto action)
    {
        SelectedApplicationAction = action;

        // TODO:
        // Implement confirmation and server-side action processing.

        return Task.CompletedTask;
    }


    // ------------------------------------------------------------
    // Breadcrumbs
    // ------------------------------------------------------------

    private List<BreadcrumbItemDto> BreadcrumbItems = [];

    private void BuildBreadcrumbs()
    {
        BreadcrumbItems =
        [
            new()
            {
                Text = "Home",
                Url = "/"
            },

            new()
            {
                Text = "Application Dashboard",
                Url = string.Empty,
                IsCurrent = true
            }
        ];
    }


    // ------------------------------------------------------------
    // Navigation
    // ------------------------------------------------------------

    private void NavigateToStaffList()
    {
        Navigation.NavigateTo(
            $"/StaffList?applicationId={ApplicationID}" +
            $"&appType={AppType}" +
            $"&fiscalYear={SelectedFiscalYear}");
    }

    private void NavigateToAssurance()
    {
        Navigation.NavigateTo(
            $"/StatementOfAssurance?applicationId={ApplicationID}" +
            $"&appType={AppType}" +
            $"&fiscalYear={SelectedFiscalYear}");
    }

    // ------------------------------------------------------------
    // Site Sorting
    // ------------------------------------------------------------

    private int SiteTotalPages =>
    Math.Max(
        1,
        (int)Math.Ceiling(
            FilteredSiteApplicationCardList.Count /
            (double)SitePageSize));

    private List<SiteApplicationCardDto> PagedSiteApplicationCardList =>
        GetPagedSiteApplications();
    private List<SiteApplicationCardDto> GetPagedSiteApplications()
    {
        IEnumerable<SiteApplicationCardDto> sites =
            FilteredSiteApplicationCardList;

        sites = SiteSortField switch
        {
            "SiteName" => SiteSortDescending
                ? sites.OrderByDescending(site => site.SiteName)
                : sites.OrderBy(site => site.SiteName),

            "SiteID" => SiteSortDescending
                ? sites.OrderByDescending(site => site.SiteID)
                : sites.OrderBy(site => site.SiteID),

            "Status" => SiteSortDescending
                ? sites.OrderByDescending(site => site.StatusDescription)
                : sites.OrderBy(site => site.StatusDescription),

            _ => sites.OrderBy(site => site.SiteName)
        };

        return sites
            .Skip((SiteCurrentPage - 1) * SitePageSize)
            .Take(SitePageSize)
            .ToList();
    }
    //Add sorting:
    private void SortSites(string field)
    {
        if (SiteSortField == field)
        {
            SiteSortDescending = !SiteSortDescending;
        }
        else
        {
            SiteSortField = field;
            SiteSortDescending = false;
        }

        SiteCurrentPage = 1;
    }
    //Add paging:
    private void GoToSitePage(int page)
    {
        if (page < 1 || page > SiteTotalPages)
        {
            return;
        }

        SiteCurrentPage = page;
    }

    private void OnSitePageSizeChanged(int pageSize)
    {
        if (pageSize <= 0)
        {
            return;
        }

        SitePageSize = pageSize;
        SiteCurrentPage = 1;
    }
    //Update your search:
    private void OnSiteSearchChanged(string value)
    {
        SiteSearchText = value;

        SiteCurrentPage = 1;

        ApplySiteFilter();
    }

    private void ApplySiteFilter()
    {
        if (string.IsNullOrWhiteSpace(SiteSearchText))
        {
            FilteredSiteApplicationCardList =
                SiteApplicationCardList.ToList();

            SiteCurrentPage = 1;

            return;
        }

        var search = SiteSearchText.Trim();

        FilteredSiteApplicationCardList =
            SiteApplicationCardList
                .Where(site =>
                    ContainsIgnoreCase(site.SiteName, search) ||
                    ContainsIgnoreCase(site.SiteID.ToString(), search) ||
                    ContainsIgnoreCase(site.StatusDescription, search) ||
                    ContainsIgnoreCase(site.ContactName, search) ||
                    ContainsIgnoreCase(site.ContactEmailAddress, search) ||
                    ContainsIgnoreCase(site.ContactPhone, search) ||
                    ContainsIgnoreCase(site.Grades, search) ||
                    site.NeedPrograms.Any(program =>
                        ContainsIgnoreCase(program, search)) == true)
                .ToList();

        SiteCurrentPage = 1;
    }

}
