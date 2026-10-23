using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class ChangeHistory
{
    public int Id { get; set; }

    public int? EntityID { get; set; }

    public int? PDSType { get; set; }

    public int? FiscalYear { get; set; }

    public int? ApplicationID { get; set; }

    public int? EDPApplicationID { get; set; }

    public string? TableName { get; set; }

    public int? TableRecordID { get; set; }

    public string? FieldName { get; set; }

    public string? DisplayName { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string? Action { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? SiteID { get; set; }
}
