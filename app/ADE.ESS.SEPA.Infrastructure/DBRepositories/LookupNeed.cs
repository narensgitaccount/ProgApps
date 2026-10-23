using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class LookupNeed
{
    public int NeedID { get; set; }

    public string NeedCode { get; set; } = null!;

    public string? NeedDescription { get; set; }

    public int OrderID { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public string DisplayClass { get; set; } = null!;

    public virtual ICollection<PDSSiteNeed> PDSSiteNeeds { get; set; } = new List<PDSSiteNeed>();
}
