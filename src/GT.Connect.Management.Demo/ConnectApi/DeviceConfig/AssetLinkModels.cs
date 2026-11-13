namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>
/// An asset linked to a node or device (server <c>AssetLinkDto</c> / <c>AssetLinkV2Dto</c>).
/// <c>TargetDeviceTypes</c> is only returned by the v2 endpoints.
/// </summary>
public record AssetLinkResponse
(
    Guid Id,
    Guid TenantId,
    Guid StorageReference,
    Guid? DeviceId,
    int? NodeId,
    string? Description,
    bool IsLinkedToConfigAsset,
    Guid CreatedById,
    DateTimeOffset CreatedOn,
    Guid LastModifiedById,
    DateTimeOffset LastModifiedOn,
    DeviceType[]? TargetDeviceTypes = null
);

/// <summary>Body for updating an asset link (v2). Null <c>TargetDeviceTypes</c> = all device types.</summary>
public record UpdateAssetLinkCommand
(
    string? Description,
    DeviceType[]? TargetDeviceTypes = null
);
