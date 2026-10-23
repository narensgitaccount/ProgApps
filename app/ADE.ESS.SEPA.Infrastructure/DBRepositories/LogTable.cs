using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class LogTable
{
    public int LogId { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Origin { get; set; }

    public string? LogLevel { get; set; }

    public string? Message { get; set; }

    public string? Exception { get; set; }

    public string? StackTrace { get; set; }
}
