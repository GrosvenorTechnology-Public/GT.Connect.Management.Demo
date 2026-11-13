namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>
/// A configuration file assigned to a node or device (server <c>DeviceConfigurationDto</c> / <c>DeviceConfigurationV2Dto</c>).
/// <c>TargetDeviceTypes</c> is only returned by the v2 endpoints.
/// </summary>
public record DeviceConfiguration (Guid TenantId, Guid Id, Guid FileTypeId, Guid? DeviceId, int? NodeId, string Description,
    string ConfigurationType, string Data, Guid CreatedById, DateTimeOffset CreatedOn,
    Guid LastModifiedById, DateTimeOffset LastModifiedOn, DeviceType[]? TargetDeviceTypes = null);
