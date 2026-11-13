namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>
/// Body for attaching a package (application/firmware) to a node or device. <c>TenantId</c> must match the route tenant.
/// <c>TargetDeviceTypes</c> is only honoured by the v2 endpoint (null = all device types).
/// </summary>
public record AddPackageToDeviceOrNodeCommand
(
    Guid TenantId,
    Guid StorageReference,
    int? NodeId,
    Guid? DeviceId,
    string Description,
    Guid FileTypeId,
    DeviceType[]? TargetDeviceTypes = null
);

/// <summary>Body for updating a package link (v2). Null <c>StorageReference</c>/<c>FileTypeId</c> leave those unchanged.</summary>
public record UpdatePackageLinkCommand
(
    string? Description,
    Guid? StorageReference = null,
    Guid? FileTypeId = null,
    DeviceType[]? TargetDeviceTypes = null
);
