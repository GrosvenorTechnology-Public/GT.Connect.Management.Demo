using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using GT.Connect.Management.Demo.ConnectApi.DeviceConfig;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.ApplicationPackage;

/// <summary>
/// Application/firmware packages — /app-package/Packages.
/// The "OnNode" variants check permission on the given node instead of the caller's home node; otherwise they behave the same.
/// </summary>
public interface IConnectApiAppPackagePackages
{
    /// <summary>Gets all firmware available in the central GTC firmware store. Returns 204 when empty.</summary>
    [Get("/api/tenants/{tenantId}/app-package/Packages/firmwarestore")]
    Task<ApiResponse<List<AppPackageFirmware>>> GetAppPackageFirmwareStore(Guid tenantId);

    /// <summary>Gets all firmware available in the central firmware store (node-permission variant). Returns 204 when empty.</summary>
    [Get("/api/tenants/{tenantId}/app-package/Packages/node/{nodeId}/firmwarestore")]
    Task<ApiResponse<List<AppPackageFirmware>>> GetAppPackageFirmwareStoreOnNode(Guid tenantId, int nodeId);

    /// <summary>Adds firmware from the central firmware store to the tenant's packages (unknown/already installed items are skipped).</summary>
    [Post("/api/tenants/{tenantId}/app-package/Packages")]
    Task<IApiResponse> AddAppPackageFirmware(Guid tenantId, [Body] AddAppPackageFirmwareCommand command);

    /// <summary>Adds firmware from the central firmware store (node-permission variant).</summary>
    [Post("/api/tenants/{tenantId}/app-package/Packages/node/{nodeId}")]
    Task<IApiResponse> AddAppPackageFirmwareOnNode(Guid tenantId, int nodeId, [Body] AddAppPackageFirmwareCommand command);

    /// <summary>Sets the state of a firmware package (only firmware can be updated; 400 otherwise). Returns the updated package entity.</summary>
    [Put("/api/tenants/{tenantId}/app-package/Packages")]
    Task<AppPackageUpdateResponse> UpdateAppPackageState(Guid tenantId, [Body] AppPackageUpdateCommand command);

    /// <summary>Sets the state of a firmware package (node-permission variant).</summary>
    [Put("/api/tenants/{tenantId}/app-package/Packages/node/{nodeId}")]
    Task<AppPackageUpdateResponse> UpdateAppPackageStateOnNode(Guid tenantId, int nodeId, [Body] AppPackageUpdateCommand command);

    /// <summary>Lists all packages in the tenant, paged with Gridify (v2).</summary>
    [Get("/api/tenants/{tenantId}/app-package/Packages")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<AppPackage>>> GetAppPackages(Guid tenantId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists packages (Sieve filtering, node-permission variant). Restricted firmware is hidden from non-admins. Returns 204 when empty.</summary>
    [Get("/api/tenants/{tenantId}/app-package/Packages/node/{nodeId}")]
    Task<ApiResponse<List<AppPackage>>> GetAppPackagesOnNode(Guid tenantId, int nodeId, [Query] SieveQueryParams? query = null);

    /// <summary>Returns true if the given firmware version exists in the central firmware store for the device type.</summary>
    [Get("/api/tenants/{tenantId}/app-package/Packages/firmware/{deviceType}/version/{version}")]
    Task<bool> IsAppPackageFirmwareAvailable(Guid tenantId, DeviceType deviceType, string version);

    /// <summary>
    /// Lists packages supported by a device type, paged with Gridify (v2).
    /// NB: when there are no matches the server returns a bare <c>[]</c> instead of a paging object, which fails to
    /// deserialise as a paging response — treat a deserialisation error on a 200 response as an empty result.
    /// </summary>
    [Get("/api/tenants/{tenantId}/app-package/Packages/{deviceType}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<AppPackage>>> GetAppPackagesByDeviceType(Guid tenantId, DeviceType deviceType, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists packages supported by a device type (Sieve, node-permission variant). Returns 204 when empty.</summary>
    [Get("/api/tenants/{tenantId}/app-package/Packages/node/{nodeId}/{deviceType}")]
    Task<ApiResponse<List<AppPackage>>> GetAppPackagesByDeviceTypeOnNode(Guid tenantId, int nodeId, DeviceType deviceType, [Query] SieveQueryParams? query = null);

    /// <summary>Gets a single package. Returns 404 if not found.</summary>
    [Get("/api/tenants/{tenantId}/app-package/Packages/{packageId}")]
    Task<ApiResponse<AppPackage>> GetAppPackage(Guid tenantId, Guid packageId);

    /// <summary>Gets a single package (node-permission variant). Returns 404 if not found.</summary>
    [Get("/api/tenants/{tenantId}/app-package/Packages/node/{nodeId}/{packageId}")]
    Task<ApiResponse<AppPackage>> GetAppPackageOnNode(Guid tenantId, int nodeId, Guid packageId);

    /// <summary>Deletes a package.</summary>
    [Delete("/api/tenants/{tenantId}/app-package/Packages/{packageId}")]
    Task<IApiResponse> DeleteAppPackage(Guid tenantId, Guid packageId);

    /// <summary>Deletes a package (node-permission variant).</summary>
    [Delete("/api/tenants/{tenantId}/app-package/Packages/node/{nodeId}/{packageId}")]
    Task<IApiResponse> DeleteAppPackageOnNode(Guid tenantId, int nodeId, Guid packageId);
}

/// <summary>Package as returned by the package list/get endpoints. <c>PackageType</c>/<c>PackageState</c> are the enum names as strings.</summary>
public record AppPackage
(
    Guid Id,
    Guid TenantId,
    Guid PackageGroupId,
    string Version,
    string? VersionCode,
    string? Publisher,
    string DisplayName,
    Guid FileTypeId,
    string PackageType,
    string? PackageName,
    string PackageState,
    string? Metadata,
    byte[]? Icon,
    long FileSize,
    List<string>? SupportedDevices,
    int NodeId,
    string? MinFirmwareVersion,
    string? MaxFirmwareVersion,
    Guid CreatedById,
    DateTimeOffset CreatedOn,
    Guid LastModifiedById,
    DateTimeOffset LastModifiedOn
);

/// <summary>Firmware entry from the central GTC firmware store.</summary>
public record AppPackageFirmware
(
    string Version,
    string? VersionCode,
    string? Publisher,
    string? DisplayName,
    Guid FileTypeId,
    string? PackageState,
    byte[]? Icon,
    string[]? SupportedDevices,
    string? Url,
    string? Device,
    bool Exists,
    string? MinFirmwareVersion,
    string? MaxFirmwareVersion
);

public record AddAppPackageFirmwareCommand(List<AppPackageFirmwareRef> Firmware);

public record AppPackageFirmwareRef(Guid FileTypeId, string Version);

public record AppPackageUpdateCommand(Guid Id, AppPackageState State);

/// <summary>
/// The raw package entity returned by the update endpoints. Unlike <see cref="AppPackage"/>, <c>SupportedDevices</c> is the stored
/// JSON string and the enums are serialised as numbers.
/// </summary>
public record AppPackageUpdateResponse
(
    Guid Id,
    Guid TenantId,
    Guid PackageGroupId,
    string Version,
    string? VersionCode,
    string? Publisher,
    string DisplayName,
    Guid FileTypeId,
    AppPackageType PackageType,
    string? PackageName,
    AppPackageState PackageState,
    string? Metadata,
    byte[]? Icon,
    long FileSize,
    string? SupportedDevices,
    string? MinFirmwareVersion,
    string? MaxFirmwareVersion,
    Guid CreatedById,
    DateTimeOffset CreatedOn,
    Guid LastModifiedById,
    DateTimeOffset LastModifiedOn
);

public enum AppPackageState
{
    Pending,
    Active,
    Deprecated,
    Retired,
    Restricted
}

public enum AppPackageType
{
    application,
    firmware
}
