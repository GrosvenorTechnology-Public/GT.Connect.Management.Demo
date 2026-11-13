using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.ApplicationPackage;

public interface IConnectApiApplicationPackage : IConnectApiAppPackageAssets, IConnectApiAppPackagePackages
{

    [Post("/api/tenants/{tenantId}/app-package/Storage/asset-url")]
    Task<GetAssetUploadUrlResponse> GetUploadAssetUrl(Guid tenantId, [Body] GetAssetUploadUrlCommand command);

    /// <summary>Gets a SAS upload URL for an application/firmware package (permission checked on the caller's home node).</summary>
    [Post("/api/tenants/{tenantId}/app-package/Storage/package-url")]
    Task<GetAssetUploadUrlResponse> GetUploadPackageUrl(Guid tenantId, [Body] GetPackageUploadUrlCommand command);

    /// <summary>Gets a SAS upload URL for a package, checking permission on the given node instead of the caller's home node.</summary>
    [Post("/api/tenants/{tenantId}/app-package/Storage/node/{nodeId}/package-url")]
    Task<GetAssetUploadUrlResponse> GetUploadPackageUrlOnNode(Guid tenantId, int nodeId, [Body] GetPackageUploadUrlCommand command);

    /// <summary>Gets a download URL for a package/asset (machine client only). Server returns 204 if the file is not found.</summary>
    [Post("/api/tenants/{tenantId}/app-package/Storage/download")]
    Task<GetStorageDownloadUrlResponse> GetStorageDownloadUrl(Guid tenantId, [Body] GetAssetDownloadUrlCommand command);

    /// <summary>Gets the state of an upload. Server returns 204 if the upload is not found.</summary>
    [Get("/api/tenants/{tenantId}/app-package/Storage/status/{uploadId}")]
    Task<GetUploadStatusResponse> GetUploadStatus(Guid tenantId, Guid uploadId);

    /// <summary>Gets the state of an upload, checking permission on the given node. Returns 204 if not found.</summary>
    [Get("/api/tenants/{tenantId}/app-package/Storage/node/{nodeId}/status/{uploadId}")]
    Task<ApiResponse<GetUploadStatusResponse>> GetUploadStatusOnNode(Guid tenantId, int nodeId, Guid uploadId);

    /// <summary>Lists uploads (packages and assets) for the tenant, paged with Gridify (v2).</summary>
    [Get("/api/tenants/{tenantId}/app-package/Storage/status")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<GetUploadStatusResponse>>> GetUploadStatuses(Guid tenantId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets all assets in the tenant (machine client only). Server returns 404 when there are none.</summary>
    [Get("/api/tenants/{tenantId}/app-package/assets")]
    Task<ApiResponse<List<GetAssetsResponse>>> GetAssets(Guid tenantId);

    [Obsolete("The v1 endpoint is deprecated on the server, use GetAssetsOnNodeV2 instead")]
    [Get("/api/tenants/{tenantId}/app-package/assets/node/{nodeId}")]
    Task<ApiResponse<List<GetAssetsResponse>>> GetAssetsOnNode(Guid tenantId, int nodeId);
}

/// <summary>Request for an asset upload URL. <c>NodeId</c> is required: the server checks the caller's permission on it and scopes the asset to it.</summary>
public record GetAssetUploadUrlCommand
(
    Guid TenantId,
    Guid FileTypeId,
    string FileName,
    string DisplayName,
    int NodeId
);

public record GetPackageUploadUrlCommand
(
    Guid TenantId,
    string FileName
);

/// <summary>Request for a download URL. <c>Ttl</c> is the URL lifetime in seconds; 0 = server default (1 hour).</summary>
public record GetAssetDownloadUrlCommand
(
    Guid TenantId,
    Guid FileId,
    FileAssignmentType FileAssignmentType,
    int Ttl = 0
);

public enum FileAssignmentType
{
    Asset = 0,
    Configuration = 1,
    Application = 2,
    Firmware = 3,
    SecondaryApplication = 4
}

public record GetAssetUploadUrlResponse
(
    Guid Id,
    string Url
);

public record GetStorageDownloadUrlResponse
(
    string Url,
    string FileExtension,
    string Version
);

public record GetUploadStatusResponse
(
    Guid TenantId,
    string FileName,
    string State,
    string? FaultSummary,
    Guid FileTypeId,
    Guid Id,
    Guid CreatedById,
    DateTimeOffset CreatedOn,
    Guid LastModifiedById,
    DateTimeOffset LastModifiedOn
);

public record GetAssetsResponse
(
    Guid TenantId,
    string DisplayName,
    string? CustomFilename,
    Guid FileTypeId,
    string AssetState,
    long FileSize,
    Guid Id,
    Guid CreatedById,
    DateTimeOffset CreatedOn,
    Guid LastModifiedById,
    DateTimeOffset LastModifiedOn,
    int NodeId = 0,
    bool IsConfigAsset = false
);
