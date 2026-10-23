using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class LookupGrade
{
    public int GradeID { get; set; }

    public string GradeName { get; set; } = null!;

    public string GradeShortName { get; set; } = null!;

    public string GradeClass { get; set; } = null!;

    public int OrderID { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public virtual ICollection<PDSSiteGrade> PDSSiteGrades { get; set; } = new List<PDSSiteGrade>();
}
