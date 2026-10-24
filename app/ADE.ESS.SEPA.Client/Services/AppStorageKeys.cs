namespace ADE.ESS.SEPA.Client.Services;

public static class AppStorageKeys
{
    public const string State = "AppState";
    public const string EntityId = "DefaultedEntityId";
    public const string EntityName = "CurrentEntityName";
    public const string IsEssAdmin = "IsESSAdmin";

    public static readonly string[] All = [State, EntityId, EntityName, IsEssAdmin];
}
