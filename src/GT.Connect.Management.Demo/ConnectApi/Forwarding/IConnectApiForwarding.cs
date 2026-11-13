using GT.Connect.Management.Demo.ConnectApi.Core;
using Gridify;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.Forwarding;

public interface IConnectApiForwarding
{
    // Forwarders

    /// <summary>Lists the forwarders on a node (optionally including child nodes), paged/filtered with Gridify (v2).</summary>
    [Get("/api/tenants/{tenantId}/forwarding/forwarders/node/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<ForwarderResponse>>> GetForwarders(Guid tenantId, int nodeId, [Query] bool getChildren = false, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a forwarder, including its filters (404 when not found).</summary>
    [Get("/api/tenants/{tenantId}/forwarding/forwarders/{forwarderId}")]
    Task<ApiResponse<ForwarderResponse>> GetForwarder(Guid tenantId, Guid forwarderId);

    /// <summary>Creates a forwarder on the node given in the request; returns 201 with the created forwarder.</summary>
    [Post("/api/tenants/{tenantId}/forwarding/forwarders")]
    Task<ApiResponse<ForwarderResponse>> AddForwarder(Guid tenantId, [Body] AddForwarderRequest request);

    /// <summary>Updates a forwarder.</summary>
    [Put("/api/tenants/{tenantId}/forwarding/forwarders/{forwarderId}")]
    Task<ApiResponse<ForwarderResponse>> UpdateForwarder(Guid tenantId, Guid forwarderId, [Body] UpdateForwarderRequest request);

    /// <summary>Deletes a forwarder (204).</summary>
    [Delete("/api/tenants/{tenantId}/forwarding/forwarders/{forwarderId}")]
    Task<IApiResponse> DeleteForwarder(Guid tenantId, Guid forwarderId);

    // Forwarder filters

    /// <summary>Lists the filters of a forwarder (204 when there are none).</summary>
    [Get("/api/tenants/{tenantId}/forwarding/forwarders/{forwarderId}/filters")]
    Task<ApiResponse<List<ForwarderFilterResponse>>> GetForwarderFilters(Guid tenantId, Guid forwarderId);

    /// <summary>Gets a single forwarder filter (404 when not found).</summary>
    [Get("/api/tenants/{tenantId}/forwarding/forwarders/{forwarderId}/filters/{filterId}")]
    Task<ApiResponse<ForwarderFilterResponse>> GetForwarderFilter(Guid tenantId, Guid forwarderId, Guid filterId);

    /// <summary>Adds a filter to a forwarder; returns 201 with the created filter.</summary>
    [Post("/api/tenants/{tenantId}/forwarding/forwarders/{forwarderId}/filters")]
    Task<ApiResponse<ForwarderFilterResponse>> AddForwarderFilter(Guid tenantId, Guid forwarderId, [Body] AddForwarderFilterRequest request);

    /// <summary>Updates a forwarder filter.</summary>
    [Put("/api/tenants/{tenantId}/forwarding/forwarders/{forwarderId}/filters/{filterId}")]
    Task<ApiResponse<ForwarderFilterResponse>> UpdateForwarderFilter(Guid tenantId, Guid forwarderId, Guid filterId, [Body] UpdateForwarderFilterRequest request);

    /// <summary>Deletes a forwarder filter (204).</summary>
    [Delete("/api/tenants/{tenantId}/forwarding/forwarders/{forwarderId}/filters/{filterId}")]
    Task<IApiResponse> DeleteForwarderFilter(Guid tenantId, Guid forwarderId, Guid filterId);

    // Forwarder properties

    /// <summary>Lists the properties of a forwarder (204 when there are none).</summary>
    [Get("/api/tenants/{tenantId}/forwarding/forwarders/{forwarderId}/properties")]
    Task<ApiResponse<List<ForwarderPropertyResponse>>> GetForwarderProperties(Guid tenantId, Guid forwarderId);

    /// <summary>Gets a single forwarder property (404 when not found).</summary>
    [Get("/api/tenants/{tenantId}/forwarding/forwarders/{forwarderId}/properties/{propertyId}")]
    Task<ApiResponse<ForwarderPropertyResponse>> GetForwarderProperty(Guid tenantId, Guid forwarderId, Guid propertyId);

    /// <summary>Adds a property (header, query string or body value) to a forwarder; returns 201 with the created property.</summary>
    [Post("/api/tenants/{tenantId}/forwarding/forwarders/{forwarderId}/properties")]
    Task<ApiResponse<ForwarderPropertyResponse>> AddForwarderProperty(Guid tenantId, Guid forwarderId, [Body] AddForwarderPropertyRequest request);

    /// <summary>Updates a forwarder property.</summary>
    [Put("/api/tenants/{tenantId}/forwarding/forwarders/{forwarderId}/properties/{propertyId}")]
    Task<ApiResponse<ForwarderPropertyResponse>> UpdateForwarderProperty(Guid tenantId, Guid forwarderId, Guid propertyId, [Body] UpdateForwarderPropertyRequest request);

    /// <summary>Deletes a forwarder property (200).</summary>
    [Delete("/api/tenants/{tenantId}/forwarding/forwarders/{forwarderId}/properties/{propertyId}")]
    Task<IApiResponse> DeleteForwarderProperty(Guid tenantId, Guid forwarderId, Guid propertyId);

    // Logs

    /// <summary>Streams the delivery logs of a forwarder as NDJSON of <see cref="ForwarderLogResponse"/> items (PascalCase property names). TenantId in the body must match the route.</summary>
    [Post("/api/tenants/{tenantId}/forwarding/forwarderlogs/stream")]
    Task<HttpResponseMessage> StreamForwarderLogs(Guid tenantId, [Body] ForwarderLogRequest request);

    // Metrics

    /// <summary>Gets the daily aggregate forwarding counts per forwarder for a node (204 when no data).</summary>
    [Get("/api/tenants/{tenantId}/forwarding/metrics/aggregate-forwarding/{nodeId}")]
    Task<ApiResponse<List<ForwarderAggregateResponse>>> GetDailyAggregateForwarding(Guid tenantId, int nodeId, [Query] bool forceCacheUpdate = false);

    /// <summary>Gets the daily aggregate forwarding failure counts per forwarder for a node (204 when no data).</summary>
    [Get("/api/tenants/{tenantId}/forwarding/metrics/aggregate-forwarding/{nodeId}/failurecounts")]
    Task<ApiResponse<List<ForwarderAggregateResponse>>> GetDailyAggregateForwardingFailureCounts(Guid tenantId, int nodeId, [Query] bool forceCacheUpdate = false);

    /// <summary>Gets the latest failure details of the failing forwarders for a node (204 when no data).</summary>
    [Get("/api/tenants/{tenantId}/forwarding/metrics/aggregate-forwarding/{nodeId}/failures")]
    Task<ApiResponse<List<ForwarderFailureResponse>>> GetFailedForwarderLogs(Guid tenantId, int nodeId);
}
