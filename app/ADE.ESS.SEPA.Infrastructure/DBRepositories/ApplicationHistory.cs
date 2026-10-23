using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class ApplicationHistory
{
    public int LogID { get; set; }

    public DateTime LogDtm { get; set; }

    public string ChangedBy { get; set; } = null!;

    public string ApplicationType { get; set; } = null!;

    public int? ApplicationID { get; set; }

    public int? EDPApplicationID { get; set; }

    public int? SiteID { get; set; }

    public string ChangeType { get; set; } = null!;

    public string? ChangeDetail { get; set; }

    public string? Comment { get; set; }
}
