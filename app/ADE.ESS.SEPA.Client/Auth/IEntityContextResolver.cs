namespace ADE.ESS.SEPA.Client.Auth;

public enum EntityResolutionSource
{
    None,
    QueryString,
    ExistingState,
    Claims
}

public readonly record struct EntityResolution(int EntityId, EntityResolutionSource Source)
{
    public static EntityResolution None { get; } = new(0, EntityResolutionSource.None);

    public bool IsResolved => EntityId > 0;
}

public interface IEntityContextResolver
{
    Task<EntityResolution> ResolveAsync(Uri currentUri);
}
