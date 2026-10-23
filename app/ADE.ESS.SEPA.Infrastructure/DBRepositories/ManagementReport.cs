using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class ManagementReport
{
    public int ReportId { get; set; }

    public string ReportName { get; set; } = null!;

    public string ReportDisplayName { get; set; } = null!;

    public string ReportDescription { get; set; } = null!;

    public bool ReportActive { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public int? SeqNbr { get; set; }
}
