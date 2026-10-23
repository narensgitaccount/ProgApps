using ADE.ESS.SEPA.Application.DTOs;
using ADE.ESS.SEPA.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ADE.ESS.SEPA.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;
        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public  Task<PrivateSchoolApplicationDto> GetPrivateSchoolApplication(int applicationId)
        {
            return  _repository.GetPrivateSchoolApplication(applicationId);
        }

        public async Task<string> GetLookupManagementValue(string lookupname)
        {
            return await _repository.GetLookupManagementValue(lookupname);
        }
        public List<int> GetFiscalyears()
        {
            return _repository.GetFiscalyears();
        }

        public int GetActiveFiscalYear()
        {
            return _repository.GetActiveFiscalYear();
        }
     
        //public async Task<PrivateSchoolApplicationDto> GetPrivateSchoolApplication(int applicationId);
        // {

        //     return await _repository.GetPrivateSchoolApplication(applicationId);
        // }
    }
}
