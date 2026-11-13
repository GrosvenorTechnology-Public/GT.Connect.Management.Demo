using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.Platform;

/// <summary>
/// Tenant feature flag endpoints. Reads are anonymous; add/update/delete require a machine client token.
/// </summary>
public interface IConnectApiPlatformFeatures
{
    /// <summary>Gets all feature flags for the tenant, with a hash of the set.</summary>
    [Get("/api/tenants/{tenantId}/features")]
    Task<TenantFeaturesResponse> GetTenantFeatures(Guid tenantId);

    /// <summary>Gets a single feature flag. 204 (or 404) when not found.</summary>
    [Get("/api/tenants/{tenantId}/features/{featureId}")]
    Task<ApiResponse<FeatureResponse>> GetTenantFeature(Guid tenantId, Guid featureId);

    /// <summary>Adds a feature flag (machine client only). The body TenantId must match the route.</summary>
    [Post("/api/tenants/{tenantId}/features")]
    Task<FeatureResponse> AddTenantFeature(Guid tenantId, [Body] AddFeatureRequest command);

    /// <summary>Updates a feature flag (machine client only). The body TenantId and Id must match the route.</summary>
    [Put("/api/tenants/{tenantId}/features/{featureId}")]
    Task<FeatureResponse> UpdateTenantFeature(Guid tenantId, Guid featureId, [Body] UpdateFeatureRequest command);

    /// <summary>Deletes a feature flag (machine client only). 204 on success.</summary>
    [Delete("/api/tenants/{tenantId}/features/{featureId}")]
    Task<IApiResponse> DeleteTenantFeature(Guid tenantId, Guid featureId);
}

public record TenantFeaturesResponse(Guid TenantId, List<TenantFeatureItem> Features, string Hash);

public record TenantFeatureItem(int? NodeId, string NameKey, string? Extra, string? State);

public record FeatureResponse(Guid TenantId, int? NodeId, string NameKey, string? Extra, string? State,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

public record AddFeatureRequest(Guid TenantId, int? NodeId, string NameKey, string? Extra = null, string? State = null);

public record UpdateFeatureRequest(Guid Id, Guid TenantId, int? NodeId, string NameKey, string? Extra = null, string? State = null);
