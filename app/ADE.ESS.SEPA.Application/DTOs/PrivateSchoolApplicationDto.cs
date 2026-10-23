using System;
using System.Collections.Generic;
using System.Text;

namespace ADE.ESS.SEPA.Application.DTOs
{
    public partial class PrivateSchoolApplicationDto
    {
        public int ApplicationID { get; set; }

        public int UserID { get; set; }

        public int FiscalYear { get; set; }

        public int? EntityID { get; set; }

        public string CorporateName { get; set; } = null!;

        public string CorporateMailingAddress { get; set; } = null!;

        public string? CorporateMailingAddress2 { get; set; }

        public string? CorporateMailingCity { get; set; }

        public int? CorporateMailingStateId { get; set; }

        public string? CorporateMailingPostalCode { get; set; }

        public string CorporateContactName { get; set; } = null!;

        public string? CorporateContactFirstName { get; set; }

        public string? CorporateContactLastName { get; set; }

        public string CorporateContactTitle { get; set; } = null!;

        public string? CorporateContactPhone { get; set; }

        public string CorporateContactEmail { get; set; } = null!;

        public string? CorporateContactFax { get; set; }

        public string SchoolAdminContactFirstName { get; set; } = null!;

        public string SchoolAdminContactTitle { get; set; } = null!;

        public string SchoolAdminContactPhone { get; set; } = null!;

        public string SchoolAdminContactEmail { get; set; } = null!;

        public string? SchoolAdminContactFax { get; set; }

        public string FCCCardNumber { get; set; } = null!;

        public long PublicEducatorId { get; set; }

        public int CertificationType { get; set; }

        public string? ExperienceComments { get; set; }

        public bool PolicyByUsMail { get; set; }

        public string PolicyLink { get; set; } = null!;

        public bool CurriculumByUsMail { get; set; }

        public string CurriculumLink { get; set; } = null!;

        public string PointOfContactName { get; set; } = null!;

        public string PointOfContactFirstName { get; set; } = null!;

        public string PointOfContactLastName { get; set; } = null!;

        public string PointOfContactTitle { get; set; } = null!;

        public string PointOfContactPhone { get; set; } = null!;

        public string PointOfContactEmail { get; set; } = null!;

        public string? PointOfContactFax { get; set; }

        public int Status { get; set; }

        public string LastModifiedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public string SchoolAdminContactLastName { get; set; } = null!;

        public string DhsLicenseNumber { get; set; } = null!;

        public string? SignedBy { get; set; }

        public string? SignerTitle { get; set; }

        public DateTime? SignDate { get; set; }

        public string? PDSStatementAssurance { get; set; }

        public string? EDPStatementAssurance { get; set; }

        public int? PriorApplicationID { get; set; }

        public bool HasBeenApproved { get; set; }

        public bool FilesAdded { get; set; }

        public bool FilesDeleted { get; set; }

        public DateTime? LastCertifiedDate { get; set; }

        public int? CrisisInterventionResponsiblePerson { get; set; }

        public string? CrisisInterventionFirstName { get; set; }

        public string? CrisisInterventionLastName { get; set; }

        public string? CrisisInterventionTitle { get; set; }

        public string? CrisisInterventionEmailAddress { get; set; }

        public string? CrisisInterventionPhoneNumber { get; set; }

        public string? CrisisInterventionFax { get; set; }

        public bool IsForProfit { get; set; }

        public bool IsStudentDemo { get; set; }

        public int? IsStudentDemoKto12 { get; set; }

        public int? IsStudentDemo3to5 { get; set; }

        public int? IsStudentDemoMET { get; set; }

        public string ProgramInfoIntakeProcess { get; set; } = null!;

        public string ProgramInfoBehavioralData { get; set; } = null!;

        public string ProgramInfoExitCriteria { get; set; } = null!;

        public string ProgramInfoCollaborationProcess { get; set; } = null!;

        public string? ResidentialSchoolStatementAssurance { get; set; }

        public string? RtcWithEdpStatementAssurance { get; set; }

        public int? ApplicationKey { get; set; }

        public bool? IsCurrent { get; set; }

        //public virtual StateAbbreviation? CorporateMailingState { get; set; }

        //public virtual FiscalYearsLookup FiscalYearNavigation { get; set; } = null!;

        //public virtual User User { get; set; } = null!;
    }
}
