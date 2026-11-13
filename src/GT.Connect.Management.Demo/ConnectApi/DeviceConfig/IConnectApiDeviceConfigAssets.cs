using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>Device-config asset links (<c>device-config/AssetLink</c>) and configuration assets (<c>device-config/Assets</c>).</summary>
public interface IConnectApiDeviceConfigAssets
{
    // ---- Asset links ----

    /// <summary>Links an asset (by storage reference) to a node or device (v1). Returns 201.</summary>
    [Post("/api/tenants/{tenantId}/device-config/assetLink")]
    Task<ApiResponse<AddAssetToDeviceOrNodeResponse>> AddAssetToDeviceOrNode(Guid tenantId, [Body] AddAssetToDeviceOrNodeCommand command);

    /// <summary>Links an asset to a node or device, honouring <c>TargetDeviceTypes</c> (v2). Returns 201.</summary>
    [Post("/api/tenants/{tenantId}/device-config/assetLink")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<AssetLinkResponse>> AddAssetToDeviceOrNodeV2(Guid tenantId, [Body] AddAssetToDeviceOrNodeCommand command);

    /// <summary>Lists asset links on a node (v1). Returns 204 when empty.</summary>
    [Obsolete("The v1 endpoint is deprecated on the server, use GetAssetsLinkedOnNodeV2.")]
    [Get("/api/tenants/{tenantId}/device-config/assetLink/node/{nodeId}")]
    Task<ApiResponse<List<AssetLinkResponse>>> GetAssetsLinkedOnNode(Guid tenantId, int nodeId);

    /// <summary>Lists asset links on a node (v2, Gridify paged). <paramref name="getChildren"/> includes all child nodes.</summary>
    [Get("/api/tenants/{tenantId}/device-config/assetLink/node/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<AssetLinkResponse>>> GetAssetsLinkedOnNodeV2(Guid tenantId, int nodeId,
        [Query] GridifyQuery? gridifyQuery = null, [Query] bool? getChildren = null);

    /// <summary>Lists asset links assigned directly to a device (v2, Gridify paged).</summary>
    [Get("/api/tenants/{tenantId}/device-config/assetLink/device/{deviceId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<AssetLinkResponse>>> GetAssetsLinkedOnDeviceV2(Guid tenantId, Guid deviceId,
        [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets an asset link by id (v1). Returns 204 when not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/assetLink/{assetLinkId}")]
    Task<ApiResponse<AssetLinkResponse>> GetAssetLink(Guid tenantId, Guid assetLinkId);

    /// <summary>Gets an asset link by id including <c>TargetDeviceTypes</c> (v2). Returns 204 when not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/assetLink/{assetLinkId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<AssetLinkResponse>> GetAssetLinkV2(Guid tenantId, Guid assetLinkId);

    /// <summary>Updates an asset link's description and <c>TargetDeviceTypes</c> (v2 only).</summary>
    [Put("/api/tenants/{tenantId}/device-config/assetLink/{assetLinkId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<AssetLinkResponse>> UpdateAssetLink(Guid tenantId, Guid assetLinkId, [Body] UpdateAssetLinkCommand command);

    /// <summary>Removes an asset link.</summary>
    [Delete("/api/tenants/{tenantId}/device-config/assetLink/{assetLinkId}")]
    Task<IApiResponse> RemoveAssetLink(Guid tenantId, Guid assetLinkId);

    /// <summary>Lists asset links that reference a storage reference (machine client only). Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/assetLink/storage/{storageReference}")]
    Task<ApiResponse<List<AssetLinkResponse>>> GetAssetLinksByStorageReference(Guid tenantId, Guid storageReference);

    // ---- Configuration assets ----

    /// <summary>Lists device-config assets scoped to a node and its ancestors (v2 only, Gridify paged).</summary>
    [Get("/api/tenants/{tenantId}/device-config/assets/node/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<DeviceConfigAsset>>> GetDeviceConfigAssetsOnNode(Guid tenantId, int nodeId,
        [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a device-config asset by id (machine client only). Returns 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/assets/{assetId}")]
    Task<ApiResponse<DeviceConfigAsset>> GetDeviceConfigAsset(Guid tenantId, Guid assetId);

    /// <summary>Copies a source asset into a new configuration on a node or device (v1).</summary>
    [Post("/api/tenants/{tenantId}/device-config/assets/copy")]
    Task<ApiResponse<DeviceConfiguration>> CopyDeviceConfigAsset(Guid tenantId, [Body] CopyOrLinkDeviceConfigAssetCommand command);

    /// <summary>Copies a source asset into a new configuration, honouring <c>TargetDeviceTypes</c> (v2).</summary>
    [Post("/api/tenants/{tenantId}/device-config/assets/copy")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<DeviceConfiguration>> CopyDeviceConfigAssetV2(Guid tenantId, [Body] CopyOrLinkDeviceConfigAssetCommand command);

    /// <summary>Links a source asset to a node or device (v1).</summary>
    [Post("/api/tenants/{tenantId}/device-config/assets/link")]
    Task<ApiResponse<AssetLinkResponse>> LinkDeviceConfigAsset(Guid tenantId, [Body] CopyOrLinkDeviceConfigAssetCommand command);

    /// <summary>Links a source asset to a node or device, honouring <c>TargetDeviceTypes</c> (v2).</summary>
    [Post("/api/tenants/{tenantId}/device-config/assets/link")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<AssetLinkResponse>> LinkDeviceConfigAssetV2(Guid tenantId, [Body] CopyOrLinkDeviceConfigAssetCommand command);

    /// <summary>Updates a device-config asset's display name, custom filename and data. Body ids must match the route.</summary>
    [Put("/api/tenants/{tenantId}/device-config/assets/{assetId}")]
    Task<ApiResponse<DeviceConfigAsset>> UpdateDeviceConfigAsset(Guid tenantId, Guid assetId, [Body] UpdateDeviceConfigAssetCommand command);

    /// <summary>Deletes a device-config asset.</summary>
    [Delete("/api/tenants/{tenantId}/device-config/assets/{assetId}")]
    Task<IApiResponse> DeleteDeviceConfigAsset(Guid tenantId, Guid assetId);
}
