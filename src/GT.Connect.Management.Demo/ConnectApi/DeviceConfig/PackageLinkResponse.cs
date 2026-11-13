namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>
/// A package (application/firmware) linked to a node or device (server <c>PackageLinkDto</c> / <c>PackageLinkV2Dto</c>).
/// <c>TargetDeviceTypes</c> is only returned by the v2 endpoints.
/// </summary>
public record PackageLinkResponse
(
    Guid TenantId,
    Guid StorageReference,
    Guid FileTypeId,
    Guid? DeviceId,
    int? NodeId,
    string Description,
    string FileAssignmentType,
    Guid Id,
    Guid CreatedById,
    DateTimeOffset CreatedOn,
    Guid LastModifiedById,
    DateTimeOffset LastModifiedOn,
    string? PackageState = null,
    DeviceType[]? TargetDeviceTypes = null
);
