namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>
/// Body for adding a configuration to a node or device. <c>TenantId</c> must match the route tenant.
/// <c>TargetDeviceTypes</c> is only honoured by the v2 endpoint (null = all device types).
/// </summary>
public record AddConfigurationCommand(Guid TenantId, int? nodeId, Guid? DeviceId, string Description, Guid FileTypeId, string Data,
    DeviceType[]? TargetDeviceTypes = null);

/// <summary>
/// Body for updating a configuration. <c>TenantId</c> is ignored by the server (taken from the route).
/// <c>TargetDeviceTypes</c> is only honoured by the v2 endpoint.
/// </summary>
public record UpdateConfigurationCommand(Guid TenantId, string Description, string Data, DeviceType[]? TargetDeviceTypes = null);
