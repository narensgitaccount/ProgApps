using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class vwEdOrg
{
    public int FiscalYear { get; set; }

    public int EducationOrganizationID { get; set; }

    public string? NameOfInstitution { get; set; }

    public string? CTDS { get; set; }

    public int IsApprovedPrivate { get; set; }
}
