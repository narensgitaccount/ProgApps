using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class PDSApplicationSite
{
    public int SiteID { get; set; }

    public int? ApplicationID { get; set; }

    public int? EDPApplicationID { get; set; }

    public string SiteName { get; set; } = null!;

    public int? SchoolSiteID { get; set; }

    public string StreetAddress { get; set; } = null!;

    public string City { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string ContactName { get; set; } = null!;

    public string ContactPhone { get; set; } = null!;

    public string ContactEmailAddress { get; set; } = null!;

    public int EntityID { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public string DistrictLocation { get; set; } = null!;

    public bool PrivateSchool { get; set; }

    public bool ResidentialSchool { get; set; }

    public bool EmotionalDisabilityPrivate { get; set; }

    public string? StudentCharacter { get; set; }

    public string? ContinuumOfService { get; set; }

    public string? Separation { get; set; }

    public string? MentalHealth { get; set; }

    public string? CaseManagement { get; set; }

    public string? MedicationDispensing { get; set; }

    public string? OutsideAgency { get; set; }

    public string? AgeRange { get; set; }

    public string? EDPComponent { get; set; }

    public int PDSType { get; set; }

    public int? PriorSiteID { get; set; }

    public string RecordStatus { get; set; } = null!;

    public bool FilesAdded { get; set; }

    public bool FilesDeleted { get; set; }

    public int ClassRoomsCount { get; set; }

    public int ClassRoomsWithServicesCount { get; set; }

    public string? ProgramName { get; set; }

    public string? SeclusionRooms { get; set; }

    public string? CounsellingServices { get; set; }

    public string? EDPEntitySiteType { get; set; }

    public string? EDPBuildingLocation { get; set; }

    public string? EDPClassRoomArea { get; set; }

    public string? EDPSupervision { get; set; }

    public string? EDPSeclusionArea { get; set; }

    public string? EDPServices { get; set; }

    public string? EDPSupervisionLevel { get; set; }

    public string? EDPStudentTransferNum { get; set; }

    public string? EDPContactDetails { get; set; }

    public bool? IsValid { get; set; }

    public bool? IsOnPublicSchoolCampus { get; set; }

    public string? DhsLicenseNumberAtSite { get; set; }

    public int? SiteKey { get; set; }

    public int? ApplicationKey { get; set; }

    public bool? IsCurrent { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<PDSSiteClassroom> PDSSiteClassrooms { get; set; } = new List<PDSSiteClassroom>();

    public virtual ICollection<PDSSiteGrade> PDSSiteGrades { get; set; } = new List<PDSSiteGrade>();

    public virtual ICollection<PDSSiteNeed> PDSSiteNeeds { get; set; } = new List<PDSSiteNeed>();

    public virtual ICollection<PDSSiteTeacher> PDSSiteTeachers { get; set; } = new List<PDSSiteTeacher>();

    public virtual ICollection<SiteAttachment> SiteAttachments { get; set; } = new List<SiteAttachment>();
}
