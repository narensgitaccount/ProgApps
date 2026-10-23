using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class StatusType
{
    public int StatusID { get; set; }

    public string Description { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public virtual ICollection<EDPApplication> EDPApplications { get; set; } = new List<EDPApplication>();
}
