namespace ADE.ESS.SEPA.Client.Auth;

public sealed class FakeUserSecurityManager : IESSUserSecurityManager
{
    private static readonly ESSUserPermission[] Permissions =
    [
        new() { EntityId = FakeAuthDefaults.EntityId, Permission = FakeAuthDefaults.Permission }
    ];

    public string GetEmail() => FakeAuthDefaults.Email;
    public string GetFirstName() => FakeAuthDefaults.FirstName;
    public string GetLastName() => FakeAuthDefaults.LastName;
    public string GetFullName() => $"{FakeAuthDefaults.FirstName} {FakeAuthDefaults.LastName}";
    public string GetDepartment() => FakeAuthDefaults.Department;

    public bool IsUserAuthorized(int edOrgId, ESSUserPermissionsEnum permission) =>
        IsUserAuthorized(edOrgId, [permission]);

    public bool IsUserAuthorized(int edOrgId, IReadOnlyCollection<ESSUserPermissionsEnum> permissions) =>
        GetEntityPermissions(edOrgId).Any(p => permissions.Contains(p.Permission));

    public bool IsUserUIAuthorized(int edOrgId, IReadOnlyCollection<ESSUserPermissionsEnum> permissions) =>
        IsUserAuthorized(edOrgId, permissions);

    public IList<ESSUserPermission> GetEntityPermissions(int edOrg = 0) =>
        [.. Permissions.Where(p => edOrg == 0 || p.EntityId == edOrg)];
}
