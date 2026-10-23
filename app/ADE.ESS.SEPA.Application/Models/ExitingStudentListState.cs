namespace ADE.ESS.SEPA.Application.Models;

public class ExitingStudentListState
{
    public short FiscalYear { get; set; }
    public int? LastFocusedSaisId { get; set; }
    public bool ShowIncompleteOnly { get; set; }
    public HashSet<int> ExpandedSchools { get; set; } = new();
    public Dictionary<int, StudentGridState> SchoolGrids { get; set; } = new();

    public int ViewMode { get; set; }
    public string SearchTerm { get; set; } = "";
    public string AllSurveysStatusFilter { get; set; } = "";
    public string AllSurveysExitFilter { get; set; } = "";
    public Dictionary<int, string> StatusFilter { get; set; } = new();
    public Dictionary<int, string> ExitReasonFilter { get; set; } = new();
}

public class StudentGridState
{
    public int Page { get; set; } = 1;
    public string? SortField { get; set; }
    public string? SortDirection { get; set; }
}
