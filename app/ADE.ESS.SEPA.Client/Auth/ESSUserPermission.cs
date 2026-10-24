namespace ADE.ESS.SEPA.Client.Auth;

public sealed record ESSUserPermission
{
    public int? EntityId { get; init; }

    public ESSUserPermissionsEnum Permission { get; init; }
}
