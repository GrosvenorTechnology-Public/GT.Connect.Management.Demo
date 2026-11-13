namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>
/// A device-config (configuration) asset scoped to a node (server <c>Domain.Dtos.AssetDto</c> in the DeviceConfiguration service).
/// Not to be confused with the application-package assets.
/// </summary>
public record DeviceConfigAsset
(
    Guid Id,
    Guid TenantId,
    int NodeId,
    string? DisplayName,
    string? OriginalFileName,
    string? CustomFilename,
    string? FileFormatType,
    long FileSize,
    Guid FileTypeId,
    string? AssetState,
    string? Data,
    bool IsConfigAsset,
    Guid CreatedById,
    DateTimeOffset CreatedOn,
    Guid LastModifiedById,
    DateTimeOffset LastModifiedOn
);

/// <summary>
/// Body for copying a source asset into a new configuration, or linking a source asset, on a node or device.
/// The server uses <c>TenantId</c> from this body. <c>TargetDeviceTypes</c> is only honoured by the v2 endpoints.
/// </summary>
public record CopyOrLinkDeviceConfigAssetCommand
(
    Guid TenantId,
    Guid StorageReference,
    int? NodeId,
    Guid? DeviceId,
    DeviceType[]? TargetDeviceTypes = null
);

/// <summary>Body for updating a device-config asset. <c>Id</c> and <c>TenantId</c> must match the route values.</summary>
public record UpdateDeviceConfigAssetCommand
(
    Guid Id,
    Guid TenantId,
    string? DisplayName,
    string? CustomFilename,
    string? Data
);
