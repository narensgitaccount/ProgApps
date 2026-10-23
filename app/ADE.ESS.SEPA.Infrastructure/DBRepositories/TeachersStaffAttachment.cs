using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class TeachersStaffAttachment
{
    public int AttachmentID { get; set; }

    public int TeacherID { get; set; }

    public string Filename { get; set; } = null!;

    public DateTime UploadDate { get; set; }

    public string UploadedBy { get; set; } = null!;

    public virtual TeachersStaff Teacher { get; set; } = null!;
}
