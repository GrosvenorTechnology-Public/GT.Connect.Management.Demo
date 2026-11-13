using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>Scheduled tasks and historic device logs (both v2-only in the DeviceConfiguration service).</summary>
public interface IConnectApiDeviceConfigScheduling
{
    // ---- Scheduled tasks ----

    /// <summary>Gets scheduled tasks on a node, newest first; getChildren also includes tasks on child nodes and their devices.</summary>
    [Get("/api/tenants/{tenantId}/device-config/scheduledtask/node/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<ScheduledTask>> GetScheduledTasksOnNode(Guid tenantId, int nodeId, [Query] bool getChildren = false, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets scheduled tasks on a device, newest first.</summary>
    [Get("/api/tenants/{tenantId}/device-config/scheduledtask/device/{deviceId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<ScheduledTask>> GetScheduledTasksOnDevice(Guid tenantId, Guid deviceId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a scheduled task. 204 when not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/scheduledtask/{taskId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<ScheduledTask>> GetScheduledTask(Guid tenantId, Guid taskId);

    /// <summary>Schedules a task against a device or node (201 Created).</summary>
    [Post("/api/tenants/{tenantId}/device-config/scheduledtask")]
    [Headers("X-Api-Version: 2.0")]
    Task<ScheduledTask> AddScheduledTask(Guid tenantId, [Body] AddScheduledTaskCommand command);

    /// <summary>
    /// Updates a scheduled task's time and payload and resets it to Scheduled. Id/TenantId must match the route, and the body must
    /// carry the task's NodeId or DeviceId for the permission check.
    /// </summary>
    [Put("/api/tenants/{tenantId}/device-config/scheduledtask/{taskId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ScheduledTask> UpdateScheduledTask(Guid tenantId, Guid taskId, [Body] UpdateScheduledTaskCommand command);

    /// <summary>Deletes a scheduled task.</summary>
    [Delete("/api/tenants/{tenantId}/device-config/scheduledtask/{taskId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<IApiResponse> DeleteScheduledTask(Guid tenantId, Guid taskId);

    /// <summary>Invokes a scheduled task now (machine clients only; normally called by the scheduler).</summary>
    [Post("/api/tenants/{tenantId}/device-config/scheduledtask/invoke")]
    [Headers("X-Api-Version: 2.0")]
    Task<IApiResponse> InvokeScheduledTask(Guid tenantId, [Body] InvokeScheduledTaskCommand command);

    // ---- Historic device logs ----

    /// <summary>Gets historic log lines for a device, newest first (max 10000 rows). 503 when the log store is not configured.</summary>
    [Post("/api/tenants/{tenantId}/device-config/devicelogs/device/{deviceId}/messages")]
    [Headers("X-Api-Version: 2.0")]
    Task<List<DeviceLogMessage>> GetDeviceLogMessages(Guid tenantId, Guid deviceId, [Body] DeviceLogQueryRequest request);

    /// <summary>Gets device log line counts per time bin and severity (bin defaults to 5 minutes).</summary>
    [Post("/api/tenants/{tenantId}/device-config/devicelogs/device/{deviceId}/summary")]
    [Headers("X-Api-Version: 2.0")]
    Task<List<DeviceLogSummaryRow>> GetDeviceLogSummary(Guid tenantId, Guid deviceId, [Body] DeviceLogQueryRequest request);

    /// <summary>Gets the earliest and latest log timestamps available for a device within the filter.</summary>
    [Post("/api/tenants/{tenantId}/device-config/devicelogs/device/{deviceId}/timespan")]
    [Headers("X-Api-Version: 2.0")]
    Task<DeviceLogTimespan> GetDeviceLogTimespan(Guid tenantId, Guid deviceId, [Body] DeviceLogQueryRequest request);

    /// <summary>Gets the distinct log sources (processes) that have written logs for a device, alphabetically.</summary>
    [Post("/api/tenants/{tenantId}/device-config/devicelogs/device/{deviceId}/sources")]
    [Headers("X-Api-Version: 2.0")]
    Task<List<string>> GetDeviceLogSources(Guid tenantId, Guid deviceId, [Body] DeviceLogQueryRequest request);
}
