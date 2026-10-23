using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class FiscalYearShowHideColumn
{
    public int Id { get; set; }

    public int? FicsalYearThresholdId { get; set; }

    public string? ColumnName { get; set; }

    public string? ApplicationType { get; set; }

    public bool IsShowColumn { get; set; }

    public virtual FiscalYearThreshold? FicsalYearThreshold { get; set; }
}
