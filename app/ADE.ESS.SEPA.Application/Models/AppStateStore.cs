namespace ADE.ESS.SEPA.Application.Models
{
    public enum RoleLevel
    {
        AdminUser,
        LEAUser,
        SchoolUser
    }

    public class AppStateStore
    {
        public UserState User { get; set; } = new();
        public EdOrgState EdOrg { get; set; } = new();
        public bool IsInitialized { get; set; } = false;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Impersonation state (admin PEA selection)
        public int? ImpersonatedPeaId { get; set; }
        public int? ImpersonatedSchoolId { get; set; }
        public string? ImpersonationReturnUrl { get; set; }

        public ExitingStudentListState? ExitingStudentListSnapshot { get; set; }

        /// <summary>
        /// Gets the access level based on user permissions and entity type.
        /// Mirrors AccesLevel from the older codebase's Custom.cs.
        /// </summary>
        public RoleLevel GetAccessLevel() =>
            User.IsESSAdmin || User.IsReadOnly ? RoleLevel.AdminUser
            : EdOrg.IsSchool ? RoleLevel.SchoolUser
            : RoleLevel.LEAUser;

        /// <summary>
        /// Gets the effective PEA entity ID, accounting for admin impersonation.
        /// Mirrors PeaEntityId() from the older codebase.
        /// </summary>
        public int GetEffectivePeaId()
        {
            if (EdOrg.IsADE)
                return ImpersonatedPeaId ?? 0;
            return EdOrg.IsSchool ? EdOrg.OwnerId : EdOrg.EntityId;
        }

        /// <summary>
        /// Gets the effective school entity ID, accounting for admin impersonation.
        /// Mirrors SchoolEntityId() from the older codebase.
        /// </summary>
        public int GetEffectiveSchoolId()
        {
            if (EdOrg.IsADE)
                return ImpersonatedSchoolId ?? 0;
            return EdOrg.IsSchool ? EdOrg.EntityId : 0;
        }
    }

    public class UserState
    {
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Department { get; set; } = "";
        public string FullName => $"{FirstName} {LastName}".Trim();
        public bool IsESSAdmin { get; set; } = false;
        public bool IsReadOnly { get; set; } = false;
        public List<UserPermission> Permissions { get; set; } = new();
        public List<int> AuthorizedEntityIds { get; set; } = new();
        public bool IsAuthenticated { get; set; } = false;
    }

    public class EdOrgState
    {
        public int EntityId { get; set; } = 0;
        public string EntityName { get; set; } = "";
        public string EntityType { get; set; } = "";
        public string CTDS { get; set; } = "";
        public string Address { get; set; } = "";
        public string County { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public DateTime? LastAccessed { get; set; }

        // Entity type flags (determined by OwnerId from spPsoNgEntitiesGet)
        public bool IsSchool { get; set; } = false;
        public bool IsDistrict { get; set; } = false;
        public bool IsADE { get; set; } = false;
        public int OwnerId { get; set; } = 0;
    }

    public class UserPermission
    {
        public int EntityId { get; set; }
        public string Permission { get; set; } = "";
        public string PermissionDescription { get; set; } = "";
        public DateTime GrantedDate { get; set; }
    }
}
