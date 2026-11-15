namespace ADE.ESS.SEPA.Client.Auth;

public sealed class FakeAuthOptions
{
    public const string SectionName = "FakeAuth";

    public bool IsAdmin { get; init; } = true;
}
