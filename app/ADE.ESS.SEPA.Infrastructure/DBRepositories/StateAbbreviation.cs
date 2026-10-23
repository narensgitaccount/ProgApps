using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class StateAbbreviation
{
    public int StateAbbreviationId { get; set; }

    public string Description { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual ICollection<PrivateSchoolApplication> PrivateSchoolApplications { get; set; } = new List<PrivateSchoolApplication>();
}
