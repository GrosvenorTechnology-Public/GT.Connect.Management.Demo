using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.ApplicationPackage;

/// <summary>Application-package assets (logos, images and other static files) — /app-package/Assets.</summary>
public interface IConnectApiAppPackageAssets
{
    /// <summary>Gets assets scoped to the node and its ancestors up to the partner node, paged with Gridify (v2).</summary>
    [Get("/api/tenants/{tenantId}/app-package/Assets/node/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<GetAssetsResponse>>> GetAssetsOnNodeV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a single application-package asset. Returns 404 if not found.</summary>
    [Get("/api/tenants/{tenantId}/app-package/Assets/{assetId}")]
    Task<ApiResponse<GetAssetsResponse>> GetAppPackageAsset(Guid tenantId, Guid assetId);

    /// <summary>Updates an asset's display name, custom filename and node; returns the updated asset.</summary>
    [Put("/api/tenants/{tenantId}/app-package/Assets/{assetId}")]
    Task<GetAssetsResponse> UpdateAppPackageAsset(Guid tenantId, Guid assetId, [Body] AppPackageAssetUpdateRequest request);

    /// <summary>Permanently deletes an application-package asset.</summary>
    [Delete("/api/tenants/{tenantId}/app-package/Assets/{assetId}")]
    Task<IApiResponse> DeleteAppPackageAsset(Guid tenantId, Guid assetId);
}

public record AppPackageAssetUpdateRequest
(
    string DisplayName,
    string? CustomFilename,
    int NodeId
);
