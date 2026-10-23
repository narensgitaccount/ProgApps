using ADE.ESS.SEPA.Common. Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ADE.ESS.SEPA.Client.Auth
{
    public class ESSUserSecurityManager : IESSUserSecurityManager
    {
        private readonly IHttpContextAccessor _httpaccessor;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly IConfiguration _configuration;

        public ESSUserSecurityManager(IHttpContextAccessor httpaccessor, AuthenticationStateProvider authStateProvider, IConfiguration configuration)
        {
            this._httpaccessor = httpaccessor;
            _authStateProvider = authStateProvider;
            _configuration = configuration;
        }

        private ClaimsPrincipal? GetCurrentUser()
        {
            try
            {
                var authStateTask = _authStateProvider.GetAuthenticationStateAsync();
                if (authStateTask.IsCompletedSuccessfully)
                {
                    return authStateTask.Result.User;
                }
            }
            catch (InvalidOperationException)
            {
            }

            return _httpaccessor.HttpContext?.User;
        }
        public string GetEmail()
        {
            return this.GetClaim(_configuration.GetSection("WsFederation:EmailAddressClaim").Value!);
        }

        public string GetFirstName()
        {
            return this.GetClaim(_configuration.GetSection("WsFederation:FirstNameClaim").Value!);
        }
        public string GetLastName()
        {
            return this.GetClaim(_configuration.GetSection("WsFederation:LastNameClaim").Value!);
        }

        public string GetDepartment()
        {
            return this.GetClaim(_configuration.GetSection("WsFederation:DepartmentClaim").Value!);
        }

        public IList<ESSUserPermission> GetEntityPermissions(int edOrg = 0)
        {
            return this.GetPermissionsForUser(edOrg);
        }
        public bool IsUserAuthorized()
        {
            var ESSUserPermissionsEnums = this.GetPermissionsForUser();
            if (ESSUserPermissionsEnums != null && ESSUserPermissionsEnums.Count > 0)
            {
                //if (!ESSUserPermissionsEnums.Select(p => p.Value).Intersect(allowedPermissons.Select(rp => rp.Value)).Any())
                //{
                //    context.Fail();
                //}
                //else
                //{
                //    context.Fail();
                //}
            }
            else
            {
                //context.Fail();
            }

            return true;
        }
        //TODO: Need to change ADE admin logic 
        public bool IsUserAuthorized(int edOrgId, ESSUserPermissionsEnum permission)
        {
            if (IsADEorSupportAdmin(new List<ESSUserPermissionsEnum> { permission }))
            {
                return true;
            }
            var ESSUserPermissionsEnums = this.GetPermissionsForUser(edOrgId);
            if (ESSUserPermissionsEnums?.Count > 0)
            {
                return ESSUserPermissionsEnums.Count(p => p.Permission == permission) > 0;
            }
            else
            {
                return false;
            }
        }
        //TODO: Need to change ADE admin logic 
        public bool IsUserAuthorized(int edOrgId, List<ESSUserPermissionsEnum> permissions)
        {
            if (IsADEorSupportAdmin(permissions))
            {
                return true;
            }
            var ESSUserPermissionsEnums = this.GetPermissionsForUser(edOrgId);
            if (ESSUserPermissionsEnums?.Count > 0)
            {
                return ESSUserPermissionsEnums.Select(p => p.Permission).Intersect(permissions).Any();
            }
            else
            {
                return false;
            }
        }

        public bool IsUserUIAuthorized(int edOrgId, List<ESSUserPermissionsEnum> permissions)
        {
            if (IsADEAdmin(permissions))
            {
                return true;
            }
            var ESSUserPermissionsEnums = this.GetPermissionsForUser(edOrgId);
            if (ESSUserPermissionsEnums?.Count > 0)
            {
                return ESSUserPermissionsEnums.Select(p => p.Permission).Intersect(permissions).Any();
            }
            else
            {
                return false;
            }
        }
        private bool IsADEorSupportAdmin(List<ESSUserPermissionsEnum> permissions)
        {
            var adePermissions = this.GetPermissionsForUser(79275);
            return adePermissions.Any(x => permissions.Contains(x.Permission));
        }


        private bool IsADEAdmin(List<ESSUserPermissionsEnum> permissions)
        {
            var adePermissions = this.GetPermissionsForUser(79275);
            return adePermissions.Any(x => permissions.Contains(x.Permission));
        }

        private IList<ESSUserPermission> GetPermissionsForUser(int edOrgId = 0)
        {
            var ESSUserPermissions = new List<ESSUserPermission>();
            var claims = GetClaims(_configuration.GetSection("WsFederation:EntityPermissionClaim").Value!);

            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    var splitPermissions = claim.Split(',');
                    if (splitPermissions.Length == 2 &&
                        int.TryParse(splitPermissions[0].Trim(), out int entityId)
                        &&
                        EnumExtensions.TryParseEnumDescription(splitPermissions[1].Trim(), out ESSUserPermissionsEnum permission)
                        )
                    {
                        ESSUserPermissions.Add(new ESSUserPermission { EntityId = entityId, Permission = permission });
                    }
                }
            }

            return edOrgId != 0 ? ESSUserPermissions.Where(x => x.EntityId == edOrgId).ToList() : ESSUserPermissions;
        }


        private string GetClaim(string claimKey)
        {
            var user = GetCurrentUser();
            var claim = user?.FindFirst(c => c.Type.Equals(claimKey, StringComparison.InvariantCultureIgnoreCase));
            return claim?.Value ?? string.Empty;
        }


        private IEnumerable<string> GetClaims(string claimKey)
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return new List<string>();
            }

            var claims = user.FindAll(c => c.Type.Equals(claimKey, StringComparison.InvariantCultureIgnoreCase)).ToList();
            return claims.Select(c => c.Value).Distinct().ToList();
        }

        public string GetFullName()
        {
            var firstName = GetFirstName();
            var lastName = GetLastName();
            return string.Format("{0} {1}", firstName ?? string.Empty, lastName ?? string.Empty).TrimEnd();
        }
    }
}
