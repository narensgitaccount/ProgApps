using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class PDSAttachment
{
    public int AttachmentID { get; set; }

    public int? ApplicationID { get; set; }

    public int? EDPApplicationID { get; set; }

    public string AttachmentType { get; set; } = null!;

    public string AttachmentName { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string UploadedBy { get; set; } = null!;

    public string DocumentType { get; set; } = null!;

    public bool AnnualRollover { get; set; }
}
