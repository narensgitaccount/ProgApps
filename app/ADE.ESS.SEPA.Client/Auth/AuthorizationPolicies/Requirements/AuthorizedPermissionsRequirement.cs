namespace ADE.ESS.SEPA.Client.Auth.AuthorizationPolicies.Requirements
{
    public class AuthorizedPermissionsRequirement : AuthorizedPermissionsRequirementBase
    {
        public AuthorizedPermissionsRequirement(ESSUserPermissionsEnum permissionEnum) : base(permissionEnum)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizedPermissionsRequirement"/> class.
        /// </summary>
        /// <param name="permissionsEnum"></param>
        public AuthorizedPermissionsRequirement(List<ESSUserPermissionsEnum> permissionsEnum) : base(permissionsEnum)
        {

        }
    }
}
