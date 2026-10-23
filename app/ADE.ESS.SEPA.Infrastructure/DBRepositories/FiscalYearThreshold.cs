using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class FiscalYearThreshold
{
    public int Id { get; set; }

    public int FicsalYearThreshold { get; set; }

    public virtual ICollection<FiscalYearShowHideColumn> FiscalYearShowHideColumns { get; set; } = new List<FiscalYearShowHideColumn>();
}
