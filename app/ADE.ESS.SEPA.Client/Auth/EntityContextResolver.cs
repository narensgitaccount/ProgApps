using System.Globalization;
using ADE.ESS.SEPA.Application.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace ADE.ESS.SEPA.Client.Auth;

public sealed class EntityContextResolver(
    IAppStateService appState,
    IESSUserSecurityManager securityManager,
    ILogger<EntityContextResolver> logger) : IEntityContextResolver
{
    public async Task<EntityResolution> ResolveAsync(Uri currentUri)
    {
        if (TryGetQueryStringEntityId(currentUri, out var queryEntityId))
        {
            logger.LogInformation("EntityID {EntityId} resolved from query string", queryEntityId);
            return new EntityResolution(queryEntityId, EntityResolutionSource.QueryString);
        }

        var existing = await appState.GetEdOrgStateAsync();
        if (existing.EntityId > 0)
        {
            logger.LogInformation("EntityID {EntityId} resolved from stored state", existing.EntityId);
            return new EntityResolution(existing.EntityId, EntityResolutionSource.ExistingState);
        }

        var claimEntityId = ResolveFromClaims();
        if (claimEntityId > 0)
        {
            logger.LogInformation("EntityID {EntityId} resolved from permission claims", claimEntityId);
            return new EntityResolution(claimEntityId, EntityResolutionSource.Claims);
        }

        return EntityResolution.None;
    }

    private static bool TryGetQueryStringEntityId(Uri uri, out int entityId)
    {
        entityId = 0;
        var query = QueryHelpers.ParseQuery(uri.Query);
        return query.TryGetValue(AdeConstants.EntityIdQueryParameter, out var values)
            && int.TryParse(values.FirstOrDefault(), NumberStyles.Integer, CultureInfo.InvariantCulture, out entityId)
            && entityId > 0;
    }

    private int ResolveFromClaims()
    {
        var entityIds = securityManager.GetEntityPermissions()
            .Where(p => p.EntityId is > 0)
            .Select(p => p.EntityId!.Value)
            .ToList();

        if (entityIds.Count == 0)
            return 0;

        return entityIds.Contains(AdeConstants.AdeEntityId) ? AdeConstants.AdeEntityId : entityIds[0];
    }
}
