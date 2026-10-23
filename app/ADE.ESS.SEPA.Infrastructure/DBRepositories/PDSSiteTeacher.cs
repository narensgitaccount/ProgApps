using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class PDSSiteTeacher
{
    public int SiteTeacherID { get; set; }

    public int SiteID { get; set; }

    public int TeacherID { get; set; }

    public DateTime? PDSSiteStartDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public virtual PDSApplicationSite Site { get; set; } = null!;

    public virtual TeachersStaff Teacher { get; set; } = null!;
}
