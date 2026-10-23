namespace ADE.ESS.SEPA.Client.Auth
{
    public interface IESSUserSecurityManager
    {
        bool IsUserAuthorized();

        bool IsUserAuthorized(int edOrgId, ESSUserPermissionsEnum permission);

        bool IsUserAuthorized(int edOrgId, List<ESSUserPermissionsEnum> permissions);

        string GetFullName();

        string GetDepartment();

        IList<ESSUserPermission> GetEntityPermissions(int edOrg = 0);
        string GetFirstName();

        string GetLastName();
        string GetEmail();

        bool IsUserUIAuthorized(int edOrgId, List<ESSUserPermissionsEnum> permissions);
    }
}
