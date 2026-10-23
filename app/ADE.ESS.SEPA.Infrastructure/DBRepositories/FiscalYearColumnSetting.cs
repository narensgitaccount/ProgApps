using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class FiscalYearColumnSetting
{
    public int Id { get; set; }

    public int? FicsalYear { get; set; }

    public int? EntityId { get; set; }

    public string? ColumnName { get; set; }

    public string? TableName { get; set; }

    public bool HasBeenSet { get; set; }

    public DateTime CreateDate { get; set; }

    public string? ModifyUserName { get; set; }
}
