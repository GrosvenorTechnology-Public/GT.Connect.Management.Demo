using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>Device-config package links (<c>device-config/Package</c>): applications/firmware attached to nodes or devices.</summary>
public interface IConnectApiDeviceConfigPackages
{
    /// <summary>Lists package links assigned directly to a device (v1). Returns null (204) when empty.</summary>
    [Obsolete("The v1 endpoint is deprecated on the server, use GetPackagesOnDeviceV2.")]
    [Get("/api/tenants/{tenantId}/device-config/package/device/{deviceId}")]
    Task<List<PackageLinkResponse>> GetPackagesOnDevice(Guid tenantId, Guid deviceId);

    /// <summary>Lists package links assigned directly to a device (v2, Gridify paged).</summary>
    [Get("/api/tenants/{tenantId}/device-config/package/device/{deviceId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<PackageLinkResponse>>> GetPackagesOnDeviceV2(Guid tenantId, Guid deviceId,
        [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists package links on a node (v1). Returns 204 when empty.</summary>
    [Obsolete("The v1 endpoint is deprecated on the server, use GetPackagesOnNodeV2.")]
    [Get("/api/tenants/{tenantId}/device-config/package/node/{nodeId}")]
    Task<ApiResponse<List<PackageLinkResponse>>> GetPackagesOnNode(Guid tenantId, int nodeId);

    /// <summary>Lists package links on a node (v2, Gridify paged). <paramref name="getChildren"/> includes all child nodes.</summary>
    [Get("/api/tenants/{tenantId}/device-config/package/node/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<PackageLinkResponse>>> GetPackagesOnNodeV2(Guid tenantId, int nodeId,
        [Query] GridifyQuery? gridifyQuery = null, [Query] bool? getChildren = null);

    /// <summary>Gets a package link by id (v1). Returns 204 when not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/package/{packageLinkId}")]
    Task<ApiResponse<PackageLinkResponse>> GetPackageLink(Guid tenantId, Guid packageLinkId);

    /// <summary>Gets a package link by id including <c>TargetDeviceTypes</c> (v2). Returns 204 when not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/package/{packageLinkId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PackageLinkResponse>> GetPackageLinkV2(Guid tenantId, Guid packageLinkId);

    /// <summary>Attaches a package to a node or device (v1). Returns 201.</summary>
    [Post("/api/tenants/{tenantId}/device-config/package")]
    Task<PackageLinkResponse> AddPackageToDeviceOrNode(Guid tenantId, [Body] AddPackageToDeviceOrNodeCommand command);

    /// <summary>Attaches a package to a node or device, honouring <c>TargetDeviceTypes</c> (v2). Returns 201.</summary>
    [Post("/api/tenants/{tenantId}/device-config/package")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PackageLinkResponse>> AddPackageToDeviceOrNodeV2(Guid tenantId, [Body] AddPackageToDeviceOrNodeCommand command);

    /// <summary>Updates a package link's description, storage reference, file type and <c>TargetDeviceTypes</c> (v2 only).</summary>
    [Put("/api/tenants/{tenantId}/device-config/package/{packageLinkId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PackageLinkResponse>> UpdatePackageLink(Guid tenantId, Guid packageLinkId, [Body] UpdatePackageLinkCommand command);

    /// <summary>Removes a package link. The server returns no body, so the result is always null.</summary>
    [Obsolete("The server returns an empty 200, so the List result is always null. Use RemovePackageLink.")]
    [Delete("/api/tenants/{tenantId}/device-config/package/{packageId}")]
    Task<List<PackageLinkResponse>> DeletePackageLink(Guid tenantId, Guid packageId);

    /// <summary>Removes a package link.</summary>
    [Delete("/api/tenants/{tenantId}/device-config/package/{packageLinkId}")]
    Task<IApiResponse> RemovePackageLink(Guid tenantId, Guid packageLinkId);
}
