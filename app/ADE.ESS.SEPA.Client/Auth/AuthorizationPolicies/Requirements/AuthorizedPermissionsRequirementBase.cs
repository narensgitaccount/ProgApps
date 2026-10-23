using Microsoft.AspNetCore.Authorization;

namespace ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies.Requirements
{
    public class AuthorizedPermissionsRequirementBase : IAuthorizationRequirement
    {
        public ESSUserPermissionsEnum _permissionEnum { get; }
        public List<ESSUserPermissionsEnum> _permissionsEnum { get; }

        public AuthorizedPermissionsRequirementBase(ESSUserPermissionsEnum permissionEnum)
        {
            _permissionEnum = permissionEnum;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizedPermissionsRequirement"/> class.
        /// </summary>
        /// <param name="permissionsEnum"></param>
        public AuthorizedPermissionsRequirementBase(List<ESSUserPermissionsEnum> permissionsEnum)
        {
            _permissionsEnum = permissionsEnum;
        }
    }
}
