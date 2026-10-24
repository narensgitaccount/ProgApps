namespace ADE.ESS.SEPA.Client.Auth;

public interface IESSUserSecurityManager
{
    bool IsUserAuthorized(int edOrgId, ESSUserPermissionsEnum permission);

    bool IsUserAuthorized(int edOrgId, IReadOnlyCollection<ESSUserPermissionsEnum> permissions);

    bool IsUserUIAuthorized(int edOrgId, IReadOnlyCollection<ESSUserPermissionsEnum> permissions);

    IList<ESSUserPermission> GetEntityPermissions(int edOrg = 0);

    string GetFullName();

    string GetDepartment();

    string GetFirstName();

    string GetLastName();

    string GetEmail();
}
