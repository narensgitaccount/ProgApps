using ADE.ESS.SEPA.Application.DTOs;

namespace ADE.ESS.SEPA.Application.Interfaces
{
    public interface IDashboardRepository
    {
        public Task<PrivateSchoolApplicationDto> GetPrivateSchoolApplication(int applicationId);
        public Task<List<int>> GetFiscalYearsAsync(int count);
        public Task<int> GetActiveFiscalYearAsync();
        public Task<string> GetLookupManagementValue(string lookupname);
        public Task<List<ApplicationListDto>> GetApplicationListAsync(int fiscalYear);
        public Task<List<StatusTypeDto>> GetStatusTypesAsync();
        public Task<string> GetStatusDescriptionAsync(int statusId);
        public Task<List<SiteApplicationCardDto>> GetSiteApplicationList(int applicationKey);

        public Task<ApplicationListDto> GetCorpAppDashboard(int applicationId);

    }

}
