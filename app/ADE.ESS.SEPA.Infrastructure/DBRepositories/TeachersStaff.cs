using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class TeachersStaff
{
    public int TeacherID { get; set; }

    public int? ApplicationID { get; set; }

    public int? EDPApplicationID { get; set; }

    public long EducatorID { get; set; }

    public string FccCardNumber { get; set; } = null!;

    public string? FCCCardStatus { get; set; }

    public DateTime? FCCCardExpiryDate { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public DateTime LastModifiedDate { get; set; }

    public int StaffType { get; set; }

    public string LicenseFilename { get; set; } = null!;

    public int LicenseFileID { get; set; }

    public int? PriorTeacherID { get; set; }

    public string RecordStatus { get; set; } = null!;

    public bool FilesAdded { get; set; }

    public bool FilesDeleted { get; set; }

    public string? AffectedSiteIDs { get; set; }

    public string? AffectedSiteNames { get; set; }

    public bool? DeleteCausesIncomplete { get; set; }

    public string? StaffTypeOther { get; set; }

    public int? ApplicationKey { get; set; }

    public virtual ICollection<PDSSiteTeacher> PDSSiteTeachers { get; set; } = new List<PDSSiteTeacher>();

    public virtual ICollection<TeachersStaffAttachment> TeachersStaffAttachments { get; set; } = new List<TeachersStaffAttachment>();

    public virtual ICollection<TeachersStaffTraining> TeachersStaffTrainings { get; set; } = new List<TeachersStaffTraining>();
}
