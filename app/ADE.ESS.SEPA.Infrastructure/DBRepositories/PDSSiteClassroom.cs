using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class PDSSiteClassroom
{
    public int SiteClassroomID { get; set; }

    public int SiteID { get; set; }

    public string ClassRoomName { get; set; } = null!;

    public string AgeRangeDescription { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public DateTime LastModifiedDate { get; set; }

    public virtual PDSApplicationSite Site { get; set; } = null!;
}
