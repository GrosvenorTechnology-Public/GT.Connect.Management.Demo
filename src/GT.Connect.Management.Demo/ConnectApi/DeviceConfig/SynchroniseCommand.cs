namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

public record SynchroniseCommand(Guid TenantId, Guid CommandId, int? NodeId, Guid? DeviceId, int TTL = 0);