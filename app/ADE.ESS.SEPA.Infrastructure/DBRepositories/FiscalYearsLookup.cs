using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class FiscalYearsLookup
{
    public int Id { get; set; }

    public int FiscalYear { get; set; }

    public bool IsActive { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public virtual ICollection<EDPApplication> EDPApplications { get; set; } = new List<EDPApplication>();

    public virtual ICollection<PrivateSchoolApplication> PrivateSchoolApplications { get; set; } = new List<PrivateSchoolApplication>();
}
