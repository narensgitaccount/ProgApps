using ADE.ESS.SEPA.Application.DTOs;
using ADE.ESS.SEPA.Application.Interfaces;

namespace ADE.ESS.SEPA.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;
        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public Task<PrivateSchoolApplicationDto> GetPrivateSchoolApplication(int applicationId)
        {
            return _repository.GetPrivateSchoolApplication(applicationId);
        }
        public async Task<string> GetLookupManagementValue(string lookupname)
        {
            return await _repository.GetLookupManagementValue(lookupname);
        }
        public Task<List<int>> GetFiscalYearsAsync(int count)
        {
            return _repository.GetFiscalYearsAsync(count);
        }
        public async Task<int> GetActiveFiscalYearAsync()
        {
            return await _repository.GetActiveFiscalYearAsync();
        }
        public Task<List<ApplicationListDto>> GetApplicationListAsync(int fiscalYear)
        {
            return _repository.GetApplicationListAsync(fiscalYear);
        }

        public async Task<List<StatusTypeDto>> GetStatusTypesAsync()
        {
            return await _repository.GetStatusTypesAsync();
        }
        public async Task<string> GetStatusDescriptionAsync(int statusId)
        {
            return await _repository.GetStatusDescriptionAsync(statusId);
        }

        public Task<List<SiteApplicationCardDto>> GetSiteApplicationList(int applicationKey)
        {
            return _repository.GetSiteApplicationList(applicationKey);
        }

        public Task<ApplicationListDto> GetCorpAppDashboard(int applicationId)
        {
            return _repository.GetCorpAppDashboard(applicationId);
        }
    }
}
