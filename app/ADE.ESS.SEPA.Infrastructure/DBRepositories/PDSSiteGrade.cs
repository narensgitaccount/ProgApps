using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class PDSSiteGrade
{
    public int SiteGradeID { get; set; }

    public int SiteID { get; set; }

    public int GradeID { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public virtual LookupGrade Grade { get; set; } = null!;

    public virtual PDSApplicationSite Site { get; set; } = null!;
}
