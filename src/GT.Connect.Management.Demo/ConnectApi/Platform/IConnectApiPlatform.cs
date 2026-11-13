using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.Platform;

/// <summary>
/// Platform endpoints that are not tenant-scoped. Registered separately from IConnectApi because the
/// machine token handler requires a tenant id in the URL.
/// </summary>
public interface IConnectApiDiscovery
{
    [Get("/api/platform/{tenant}")]
    Task<Tenant> GetTenant(string tenant);

    /// <summary>Creates a new tenant (platform record, organisation and first user). Only Id/Name/DisplayName are populated in the response.</summary>
    [Post("/api/platform")]
    Task<ApiResponse<Tenant>> CreateTenant([Body] AddTenantCommand command);
}

/// <summary>
/// Tenant-scoped platform endpoints. Feature flag endpoints are in the inherited <see cref="IConnectApiPlatformFeatures"/>.
/// </summary>
public interface IConnectApiPlatform : IConnectApiPlatformFeatures
{
    [Get("/api/tenants/{tenantId}/platform")]
    Task<Tenant> GetTenant(Guid tenantId);
}
