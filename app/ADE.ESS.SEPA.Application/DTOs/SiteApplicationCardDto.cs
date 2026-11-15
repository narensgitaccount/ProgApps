namespace ADE.ESS.SEPA.Application.DTOs
{
    public class SiteApplicationCardDto
    {
        public int SiteID { get; set; }
        //public int SiteApplicationId { get; set; }
        public string SiteName { get; set; } = "";
        public int? Status { get; set; }
        public string StatusDescription { get; set; } = "";
        public string ContactName { get; set; } = "";
        public string ContactEmailAddress { get; set; } = "";
        public string ContactPhone { get; set; } = "";
        public List<string> NeedPrograms { get; set; } = [];
        public string Grades { get; set; } = "";
    }
}
