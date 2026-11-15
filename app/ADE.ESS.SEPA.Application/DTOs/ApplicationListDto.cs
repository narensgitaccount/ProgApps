namespace ADE.ESS.SEPA.Application.DTOs
{
    public partial class ApplicationListDto
    {
        public int ApplicationID { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; } = "";
        public string? ProgramType { get; set; }
        public bool? SOASigned { get; set; }
        public bool? EntityInfo { get; set; }
        public bool? ProgramInfo { get; set; }
        public bool? IsQueue { get; set; }
        public int? Sites { get; set; }
        public int FiscalYear { get; set; }
        public int? EntityID { get; set; }
        public int? AppType { get; set; }
        public string CorporateName { get; set; } = null!;
        public int? ApplicationKey { get; set; }
    }
}
