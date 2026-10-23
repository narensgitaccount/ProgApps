namespace ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies.Requirements
{
    public class AuthorizedPermissionsUIRequirement : AuthorizedPermissionsRequirementBase
    {
        public AuthorizedPermissionsUIRequirement(ESSUserPermissionsEnum permissionEnum) : base(permissionEnum)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizedPermissionsRequirement"/> class.
        /// </summary>
        /// <param name="permissionsEnum"></param>
        public AuthorizedPermissionsUIRequirement(List<ESSUserPermissionsEnum> permissionsEnum) : base(permissionsEnum)
        {

        }
    }
}
