using System.Security.Claims;
using ADE.ESS.SEPA.Common.Extensions;

namespace ADE.ESS.SEPA.Client.Auth;

public static class FakeAuthDefaults
{
    public const int EntityId = 4235;
    public const string Email = "fake.user@azed.gov";
    public const string FirstName = "Fake";
    public const string LastName = "User";
    public const string Department = "Exceptional Student Services";

    public static IReadOnlyList<ESSUserPermissionsEnum> GetPermissions(FakeAuthOptions options) =>
        options.IsAdmin
            ? [ESSUserPermissionsEnum.ESSPostSchoolOutcomes, ESSUserPermissionsEnum.ESSAdmin]
            : [ESSUserPermissionsEnum.ESSPostSchoolOutcomes];

    public static ClaimsPrincipal CreatePrincipal(
        AdeConnectOptions options,
        FakeAuthOptions fakeOptions,
        string authenticationType)
    {
        List<Claim> claims =
        [
            new(options.EmailAddressClaim, Email),
            new(options.FirstNameClaim, FirstName),
            new(options.LastNameClaim, LastName),
            new(options.DepartmentClaim, Department),
            .. GetPermissions(fakeOptions).Select(p => new Claim(options.EntityPermissionClaim, $"{EntityId},{p.GetDescription()}"))
        ];

        return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType));
    }
}
