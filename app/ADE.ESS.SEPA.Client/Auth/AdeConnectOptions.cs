using System.ComponentModel.DataAnnotations;

namespace ADE.ESS.SEPA.Client.Auth;

public sealed class AdeConnectOptions
{
    public const string SectionName = "WsFederation";

    public string? Wtrealm { get; init; }
    public string? MetadataAddress { get; init; }

    public string? AdeConnectUrl { get; init; }

    public string? SignoutUrl { get; init; }

    [Required] public string EmailAddressClaim { get; init; } = string.Empty;
    [Required] public string FirstNameClaim { get; init; } = string.Empty;
    [Required] public string LastNameClaim { get; init; } = string.Empty;
    [Required] public string DepartmentClaim { get; init; } = string.Empty;
    [Required] public string EntityPermissionClaim { get; init; } = string.Empty;
}
