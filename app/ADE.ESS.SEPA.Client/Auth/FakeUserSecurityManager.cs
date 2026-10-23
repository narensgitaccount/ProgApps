namespace ADE.ESS.SEPA.Client.Auth
{
    public class FakeUserSecurityManager : IESSUserSecurityManager
    {
        public string GetEmail() => "fake.user@azed.gov";
        public string GetFirstName() => "Fake";
        public string GetLastName() => "User";
        public string GetFullName() => "Fake User";
        public string GetDepartment() => "Exceptional Student Services";

        public bool IsUserAuthorized() => true;

        public bool IsUserAuthorized(int edOrgId, ESSUserPermissionsEnum permission)
        {
            var permissions = GetEntityPermissions(edOrgId);
            return permissions.Any(p => p.Permission == permission);
        }

        public bool IsUserAuthorized(int edOrgId, List<ESSUserPermissionsEnum> permissions)
        {
            var userPermissions = GetEntityPermissions(edOrgId);
            return userPermissions.Select(p => p.Permission).Intersect(permissions).Any();
        }

        public bool IsUserUIAuthorized(int edOrgId, List<ESSUserPermissionsEnum> permissions)
        {
            return IsUserAuthorized(edOrgId, permissions);
        }

        public IList<ESSUserPermission> GetEntityPermissions(int edOrg = 0)
        {
            var permissions = new List<ESSUserPermission>
            {
                new() { EntityId = 4235, Permission = ESSUserPermissionsEnum.ESSPostSchoolOutcomes }
            };

            return edOrg != 0
                ? permissions.Where(p => p.EntityId == edOrg).ToList()
                : permissions;
        }
    }
}
