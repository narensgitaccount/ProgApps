using ADE.ESS.SEPA.Application.DTOs;
using ADE.ESS.SEPA.Application.Interfaces;
using ADE.ESS.SEPA.Infrastructure.Context;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly EssSepaDbContext _context;

        public DashboardRepository(EssSepaDbContext context)
        {
            _context = context;
        }

        public async Task<PrivateSchoolApplicationDto> GetPrivateSchoolApplication(int applicationId)
        {


            var appData = _context.PrivateSchoolApplications.Where(x => x.ApplicationID == applicationId).FirstOrDefault();
            //PrivateSchoolApplicationDto result = new PrivateSchoolApplicationDto();
            //result.ApplicationID = applicationId;
            //result.ApplicationID = appData.ApplicationID;
            //result.FiscalYear = appData.FiscalYear;
            //result.EntityID = appData.EntityID;
            //result.CorporateName = appData.CorporateName;
            //result.Status = appData.Status;
            //result.IsCurrent = appData.IsCurrent;        

          
            var result = appData.Adapt<PrivateSchoolApplicationDto>();

            return result;
        }
        public List<int> GetFiscalyears()
        {
           return  _context.FiscalYearsLookups.Select(x=>x.FiscalYear).ToList();
        }

       public int GetActiveFiscalYear()
        {
            return  _context.FiscalYearsLookups.FirstOrDefault(x => x.IsActive).FiscalYear;
        }


        public async Task<string> GetLookupManagementValue(string lookupname)
        {
            var lookupText = await _context.LookupManagementValues
                .Where(x => x.LookupName == lookupname)
                .Select(x => x.LookupText)
                .FirstOrDefaultAsync(); // Works perfectly now!

            return lookupText ?? "No Value Found";
        }






    }
}
