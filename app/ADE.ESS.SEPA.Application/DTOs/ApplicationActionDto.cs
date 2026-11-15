namespace ADE.ESS.SEPA.Application.DTOs
{
    public class ApplicationActionDto
    {
        public int ActionID { get; set; }
        public string Description { get; set; } = string.Empty;
        public int TargetStatusID { get; set; }
        public bool RequiresConfirmation { get; set; }
    }
}