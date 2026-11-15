using ADE.ESS.SEPA.Application.DTOs;
using ADE.ESS.SEPA.Application.Interfaces;
using ADE.ESS.SEPA.Infrastructure.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly EssSepaDbContext _context;
        private ILogger<SiteApplicationCardDto> Logger { get; set; } = default!;

        public DashboardRepository(EssSepaDbContext context, ILogger<SiteApplicationCardDto> logger)
        {
            _context = context;
            Logger = logger;
        }

        public async Task<PrivateSchoolApplicationDto> GetPrivateSchoolApplication(int applicationId)
        {
            var appData = _context.PrivateSchoolApplications.Where(x => x.ApplicationID == applicationId).FirstOrDefault();

            var result = appData.Adapt<PrivateSchoolApplicationDto>();

            return result;
        }

        public async Task<List<int>> GetFiscalYearsAsync(int count)
        {
            return await _context.FiscalYearsLookups.AsNoTracking().OrderByDescending(x => x.FiscalYear).Take(count).Select(x => x.FiscalYear).ToListAsync();
        }

        public async Task<int> GetActiveFiscalYearAsync()
        {
            return await _context.FiscalYearsLookups.AsNoTracking().Where(x => x.IsActive == true).Select(x => x.FiscalYear).FirstOrDefaultAsync();
        }
        public async Task<List<ApplicationListDto>> GetApplicationListAsync(int fiscalYear)
        {

            var statusLookup = await _context.StatusTypes.AsNoTracking().ToDictionaryAsync(x => x.StatusID, x => x.Description);

            var privateApplist = await _context.PrivateSchoolApplications.AsNoTracking().Where(x => x.FiscalYear == fiscalYear && x.IsCurrent == true).Select(x => new ApplicationListDto
            {
                ApplicationID = x.ApplicationID,
                Status = x.Status,
                StatusDescription = statusLookup.GetValueOrDefault(x.Status, string.Empty),
                EntityInfo = !string.IsNullOrWhiteSpace(x.SchoolAdminContactFirstName) && !string.IsNullOrWhiteSpace(x.SchoolAdminContactLastName),
                ProgramInfo = true,
                IsQueue = true, /*x.Status ==2,*/
                Sites = 2,
                EntityID = x.EntityID,
                CorporateName = x.CorporateName,
                SOASigned = x.SignDate.HasValue,
                FiscalYear = x.FiscalYear,
                ProgramType = "Private",
                AppType = 1
            }).ToListAsync();

            var publicApplist = await _context.EDPApplications.AsNoTracking().Where(x => x.FiscalYear == fiscalYear && x.IsCurrent == true).Select(x => new ApplicationListDto
            {
                ApplicationID = x.ApplicationId,
                Status = x.StatusID,
                StatusDescription = statusLookup.GetValueOrDefault(x.StatusID, string.Empty),
                EntityInfo = true,
                ProgramInfo = true,
                IsQueue = true,
                Sites = 2,
                EntityID = x.EntityId,
                CorporateName = x.EntityName ?? "",
                SOASigned = x.SignatureDate.HasValue,
                FiscalYear = x.FiscalYear,
                ProgramType = "Public",
                AppType = 2
            }).ToListAsync();

            privateApplist.AddRange(publicApplist);

            return privateApplist;
        }

        public async Task<string> GetLookupManagementValue(string lookupname)
        {
            var lookupText = await _context.LookupManagementValues
                .Where(x => x.LookupName == lookupname)
                .Select(x => x.LookupText)
                .FirstOrDefaultAsync(); // Works perfectly now!

            return lookupText ?? "No Value Found";
        }

        public async Task<List<StatusTypeDto>> GetStatusTypesAsync()
        {
            return await _context.StatusTypes.AsNoTracking().Select(x => new StatusTypeDto { StatusID = x.StatusID, Description = x.Description }).ToListAsync();
        }

        public async Task<string> GetStatusDescriptionAsync(int statusId)
        {
            try
            {
                return await _context.StatusTypes.AsNoTracking().Where(x => x.StatusID == statusId).Select(x => x.Description).FirstOrDefaultAsync() ?? string.Empty;
            }
            catch (Exception ex)
            {
                var testingmessage = ex.Message;
            }
            return "";
        }

        public async Task<List<SiteApplicationCardDto>> GetSiteApplicationList(int applicationKey)
        {
            if (applicationKey <= 0) { return []; }

            // 1. Load all current sites for this application.
            var sites = await _context.PDSApplicationSites.AsNoTracking().Where(site => site.ApplicationKey == applicationKey && site.IsCurrent == true).ToListAsync();

            if (sites.Count == 0) { return []; }

            var siteIds = sites.Select(s => s.SiteID).Distinct().ToList();

            // 2. Load needs for every site in one query.
            var siteNeeds = await (
                from siteNeed in _context.PDSSiteNeeds.AsNoTracking()
                join need in _context.LookupNeeds.AsNoTracking()
                    on siteNeed.NeedID equals need.NeedID
                where siteIds.Contains(siteNeed.SiteID)
                select new
                {
                    siteNeed.SiteID,
                    need.NeedCode
                })
                .ToListAsync();

            // 3. Load grades for every site in one query.
            var siteGrades = await (
                from siteGrade in _context.PDSSiteGrades.AsNoTracking()
                join grade in _context.LookupGrades.AsNoTracking()
                    on siteGrade.GradeID equals grade.GradeID
                where siteIds.Contains(siteGrade.SiteID)
                select new
                {
                    siteGrade.SiteID,
                    grade.GradeShortName
                })
                .ToListAsync();

            // 4. Load all status descriptions in one query.
            var statusIds = sites.Select(site => site.Status).Distinct().ToList();

            var statusLookup = await _context.StatusTypes.AsNoTracking().Where(status => statusIds.Contains(status.StatusID)).Select(status => new
            {
                status.StatusID,
                status.Description
            }).ToDictionaryAsync(
                    status => status.StatusID,
                    status => status.Description);

            // 5. Group related values in memory.
            var needsBySite = siteNeeds.GroupBy(item => item.SiteID).ToDictionary(group => group.Key, group => group
                        .Select(item => item.NeedCode).Where(code => code != null).Distinct().OrderBy(code => code).ToList());

            var gradesBySite = siteGrades.GroupBy(item => item.SiteID).ToDictionary(group => group.Key, group => group
                        .Select(item => item.GradeShortName).Where(grade => grade != null).Distinct().OrderBy(grade => grade).ToList());

            // 6. Build the card DTOs.
            var result = sites
                .Select(site =>
                {
                    needsBySite.TryGetValue(site.SiteID, out var programs);
                    gradesBySite.TryGetValue(site.SiteID, out var grades);
                    statusLookup.TryGetValue(site.Status ?? 0, out var statusDescription);
                    return new SiteApplicationCardDto
                    {
                        SiteID = site.SiteID,
                        SiteName = site.SiteName ?? string.Empty,
                        Status = site.Status,
                        StatusDescription = statusDescription ?? string.Empty,
                        ContactName = site.ContactName ?? string.Empty,
                        ContactEmailAddress = site.ContactEmailAddress ?? string.Empty,
                        ContactPhone = site.ContactPhone ?? string.Empty,
                        NeedPrograms = programs ?? [],
                        Grades = string.Join(", ", grades ?? [])
                    };
                }).OrderBy(site => site.SiteName).ToList();

            return result;
        }


        public async Task<ApplicationListDto> GetCorpAppDashboard(int applicationId)
        {
            var statusLookup = await _context.StatusTypes.AsNoTracking().ToDictionaryAsync(x => x.StatusID, x => x.Description);
            return await _context.PrivateSchoolApplications.AsNoTracking().Where(x => x.ApplicationID == applicationId).Select(x => new ApplicationListDto
            {
                ApplicationID = x.ApplicationID,
                CorporateName = x.CorporateName,
                EntityID = x.EntityID,
                Status = x.Status,
                StatusDescription = statusLookup.GetValueOrDefault(x.Status, string.Empty),
                ApplicationKey = x.ApplicationKey,
                SOASigned = x.SignDate.HasValue,
                FiscalYear = x.FiscalYear,
                ProgramType = "Private",
                AppType = 1
            }).FirstOrDefaultAsync() ?? new ApplicationListDto();

        }
    }


}