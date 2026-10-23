using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class VwSchool
{
    public int? FiscalYear { get; set; }

    public int DistrictID { get; set; }

    public string? DistrictName { get; set; }

    public string? DistrictCTDS { get; set; }

    public int SchoolId { get; set; }

    public string? SchoolName { get; set; }

    public string? SchoolCTDS { get; set; }

    public int IsApprovedPrivate { get; set; }
}
