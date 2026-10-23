using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class LookupManagementValue
{
    public int LookUpManagementValueId { get; set; }

    public string LookupName { get; set; } = null!;

    public string LookupText { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool Status { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime LastModifiedDate { get; set; }
}
