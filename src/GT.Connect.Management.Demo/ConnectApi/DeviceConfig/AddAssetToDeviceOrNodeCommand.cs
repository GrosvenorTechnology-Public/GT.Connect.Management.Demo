namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>
/// Body for linking an asset (by storage reference) to a node or device. <c>TenantId</c> is ignored by the server.
/// <c>TargetDeviceTypes</c> is only honoured by the v2 endpoint (null = all device types).
/// </summary>
public record AddAssetToDeviceOrNodeCommand
(
    Guid TenantId,
    Guid StorageReference,
    int? nodeId,
    Guid? DeviceId,
    DeviceType[]? TargetDeviceTypes = null
);
