namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

public class StructuredCommand
{
    public Guid MessageId { get; } = Guid.NewGuid();
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid? PreviousMessageId { get; init; }
    public DateTimeOffset TimeStamp { get; init; } = DateTimeOffset.UtcNow;
    public TimeSpan Ttl { get; init; } = TimeSpan.FromMinutes(5);
    public required string Entity { get; init; }
    public required string CommandName { get; init; }
    public Dictionary<string, string> Parameters { get; init; } = new Dictionary<string, string>();
    public string RequestingEntity { get; init; } = "GtConnect:1";
    public string PermissionedEntity { get; init; } = "";
}

