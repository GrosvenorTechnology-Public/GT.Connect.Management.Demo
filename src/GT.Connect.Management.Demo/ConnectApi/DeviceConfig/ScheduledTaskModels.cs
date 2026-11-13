using GT.Connect.Management.Demo.ConnectApi.Core;

namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

public enum ScheduledState
{
    Scheduled,
    Processing,
    Failed,
    Completed
}

/// <summary>A task scheduled against a device or a node.</summary>
public record ScheduledTask(
    Guid Id,
    Guid TenantId,
    Guid? DeviceId,
    int? NodeId,
    DateTimeOffset ScheduledAt,
    ScheduledState Status,
    string? Payload,
    Guid CreatedById,
    DateTimeOffset CreatedOn,
    Guid LastModifiedById,
    DateTimeOffset LastModifiedOn
    ) : BaseModifiyableObject<Guid>(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

/// <summary>Body for scheduling a task against a device (<see cref="DeviceId"/>) or a node (<see cref="NodeId"/>).</summary>
public record AddScheduledTaskCommand(Guid TenantId, Guid? DeviceId, int? NodeId, DateTimeOffset ScheduledAt, string Payload);

/// <summary>
/// Body for editing a scheduled task. <see cref="NodeId"/> or <see cref="DeviceId"/> is not stored but MUST be supplied
/// (the task's node or device): the server's permission check reads it from the body and rejects the request without it.
/// </summary>
public record UpdateScheduledTaskCommand(Guid Id, Guid TenantId, DateTimeOffset ScheduledAt, string Payload, int? NodeId = null, Guid? DeviceId = null);

/// <summary>Body for invoking a scheduled task immediately (machine clients only).</summary>
public record InvokeScheduledTaskCommand(Guid ScheduledTaskId, Guid TenantId, Guid UserId, string Payload, int? NodeId = null, Guid? DeviceId = null);
