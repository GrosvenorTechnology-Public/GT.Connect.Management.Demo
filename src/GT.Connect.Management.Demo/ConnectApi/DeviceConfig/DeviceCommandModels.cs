namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>Body for <c>Command/Reload</c>: asks a device to reload. <c>CommandId</c> is a caller-generated tracking id.</summary>
public record ReloadDeviceCommand(Guid TenantId, Guid CommandId, Guid DeviceId);

/// <summary>Body for <c>Command/requestFile/{deviceId}</c>. <c>TenantId</c>/<c>DeviceId</c> must match the route values.</summary>
public record RequestFileFromDeviceCommand(Guid TenantId, Guid DeviceId, string FileToUpload, int Ttl = 0);

/// <summary>Body for <c>Command/actionRequest/{deviceId}</c>. <c>TenantId</c>/<c>DeviceId</c> must match the route values.</summary>
public record DeviceActionRequestCommand(Guid TenantId, Guid DeviceId, string ActionName, int Ttl = 0,
    Dictionary<string, string>? Parameters = null);

/// <summary>Body for purging specific queued messages for a device. <c>MessageIds</c> must not be empty.</summary>
public record PurgeQueuedMessagesCommand(Guid CommandId, Guid DeviceId, List<Guid> MessageIds);

/// <summary>Body carrying just a caller-generated command tracking id.</summary>
public record DeviceCommandIdRequest(Guid CommandId);
