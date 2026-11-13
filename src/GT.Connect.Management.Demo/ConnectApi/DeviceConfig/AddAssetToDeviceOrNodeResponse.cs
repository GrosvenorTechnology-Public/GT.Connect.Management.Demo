namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>
/// Asset link returned when an asset is attached to a node or device (server <c>AssetLinkDto</c>).
/// Prefer <see cref="AssetLinkResponse"/> for new code. <c>FileAssignmentType</c> is never sent by the server.
/// </summary>
public record AddAssetToDeviceOrNodeResponse
(
    Guid TenantId,
    Guid StorageReference,
    Guid? DeviceId,
    int? NodeId,
    string? Description,
    string? FileAssignmentType,
    Guid CreatedById,
    DateTimeOffset CreatedOn,
    Guid LastModifiedById,
    DateTimeOffset LastModifiedOn,
    Guid Id = default,
    bool IsLinkedToConfigAsset = false,
    DeviceType[]? TargetDeviceTypes = null
);
