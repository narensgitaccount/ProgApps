using ADE.ESS.SEPA.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADE.ESS.SEPA.Application.Interfaces
{
    public interface IDashboardRepository
    {
        Task<PrivateSchoolApplicationDto> GetPrivateSchoolApplication(int applicationId);
        public List<int> GetFiscalyears();
        public int GetActiveFiscalYear();
        public Task<string> GetLookupManagementValue(string lookupname);


    }
       

}
