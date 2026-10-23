using System.ComponentModel;

namespace ADE.ESS.SEPA.Client.Auth
{
    public enum ESSUserPermissionsEnum
    {
        [Description("NoPermission")] NoPermission = 1,
        [Description("ESS Admin")] ESSAdmin = 2,
        [Description("ESS Post School Outcomes")] ESSPostSchoolOutcomes = 3,
        [Description("Read Only")] ReadOnly = 4

    }
}
