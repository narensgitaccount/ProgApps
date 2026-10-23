using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class EmailDeliveryLog
{
    public int LogId { get; set; }

    public int? TemplateId { get; set; }

    public int? EntityId { get; set; }

    public int? ApplicationId { get; set; }

    public string? ApplicationName { get; set; }

    public string? ToAddress { get; set; }

    public string? Subject { get; set; }

    public string? MessageBody { get; set; }

    public string? TriggeredBy { get; set; }

    public string Status { get; set; } = null!;

    public string? ErrorMessage { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Comments { get; set; }

    public int? AppType { get; set; }

    public int? FiscalYear { get; set; }

    public string? CorporateName { get; set; }

    public string? SiteDetails { get; set; }

    public string? CorporateContact { get; set; }

    public int? SiteEntityId { get; set; }

    public int? SiteAppId { get; set; }

    public string? SiteName { get; set; }
}
