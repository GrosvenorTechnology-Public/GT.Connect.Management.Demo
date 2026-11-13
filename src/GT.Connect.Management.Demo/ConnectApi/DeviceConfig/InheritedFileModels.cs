namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>
/// A file link or configuration that applies to a node/device, including ones inherited from ancestor nodes
/// (server <c>InheritedFileDto</c>). <c>FileAssignmentType</c> is sent as a number.
/// </summary>
public record InheritedFile
(
    Guid Id,
    Guid TenantId,
    int? NodeId,
    Guid? DeviceId,
    Guid FileTypeId,
    DeviceFamily DeviceFamily,
    ApplicationPackage.FileAssignmentType FileAssignmentType,
    string? DisplayName,
    bool IsLinkedToConfigAsset,
    Guid CreatedById,
    DateTimeOffset CreatedOn,
    Guid LastModifiedById,
    DateTimeOffset LastModifiedOn
);

/// <summary>Number of active (enabled and online) devices on a node that a file/config type applies to (server <c>NodeDeviceSummaryDto</c>).</summary>
public record NodeDeviceSummary(Guid TenantId, string? NodePath, int DeviceCount);
