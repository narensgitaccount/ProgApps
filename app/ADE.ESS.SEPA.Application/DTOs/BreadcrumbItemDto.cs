namespace ADE.ESS.SEPA.Application.DTOs
{
    public class BreadcrumbItemDto
    {
        public string Text { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool IsCurrent { get; set; }
    }

}
