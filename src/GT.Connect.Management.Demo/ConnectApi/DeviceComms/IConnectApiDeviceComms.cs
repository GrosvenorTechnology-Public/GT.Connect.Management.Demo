using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using GT.Connect.Management.Demo.ConnectApi.DeviceConfig;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.DeviceComms;

public interface IConnectApiDeviceComms
{
    /// <summary>Posts a raw RS-protocol XML request as a legacy (GT8/GT10) clock would. Anonymous; always 200 with an XML body.</summary>
    [Post("/api/device-comms/rs")]
    [Headers("Content-Type: application/xml")]
    Task<ApiResponse<string>> PostRsCommsMessage(string transaction);

    [Obsolete("The v1 endpoint is deprecated on the server, use GetCommandHistoryForDeviceV2 instead")]
    [Get("/api/tenants/{tenantId}/device-comms/commandHistory/device/{deviceId}")]
    Task<ApiResponse<CommandHistoryItem[]>> GetCommandHistoryForDevice(Guid tenantId, Guid deviceId);

    /// <summary>Gets the command history for a device, paged with Gridify (v2). Not ordered unless an OrderBy is given.</summary>
    [Get("/api/tenants/{tenantId}/device-comms/commandHistory/device/{deviceId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<CommandHistoryItem>>> GetCommandHistoryForDeviceV2(Guid tenantId, Guid deviceId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a single command history entry by its message id. Returns 404 if not found.</summary>
    [Get("/api/tenants/{tenantId}/device-comms/commandHistory/{messageId}")]
    Task<ApiResponse<CommandHistoryItem>> GetCommandHistoryMessage(Guid tenantId, Guid messageId);

    /// <summary>Gets a queued/sent device message (including payload) by its message (correlation) id. Returns 204 if not found.</summary>
    [Get("/api/tenants/{tenantId}/device-comms/Message/device/{deviceId}/{messageId}")]
    Task<ApiResponse<DeviceCommsMessage>> GetDeviceCommsMessage(Guid tenantId, Guid deviceId, Guid messageId);

    /// <summary>Gets the device message queue history for a device, paged with Gridify (v2).</summary>
    [Get("/api/tenants/{tenantId}/device-comms/MessageHistory/device/{deviceId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<MessageHistoryOverview>>> GetMessageHistoryForDevice(Guid tenantId, Guid deviceId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a single message history entry, including its raw and formatted payload. Returns 204 if not found.</summary>
    [Get("/api/tenants/{tenantId}/device-comms/MessageHistory/{id}")]
    Task<ApiResponse<MessageHistoryItem>> GetMessageHistoryItem(Guid tenantId, Guid id);
}

public record CommandHistoryItem(Guid Id, Guid TenantId, Guid CorrelationId, Guid DeviceId, string SerialNumber, string DeviceFamily,
    string MessageType, DateTimeOffset CreatedDate, string CreatedBy, DateTimeOffset ExpirationDate, DateTimeOffset? SentDate,
    DateTimeOffset? CompletedDate, string State, string? Payload);

public record DeviceCommsMessage(Guid MessageId, Guid TenantId, Guid DeviceId, string SerialNumber, DeviceFamily DeviceFamily,
    string MessageType, byte Priority, DateTimeOffset ExpirationDate, DateTimeOffset CreatedDate, bool IsEncrypted, string? Payload,
    string? ContextType, string? ContextValue, Guid RequestingUserId);

/// <summary>Message history list entry. <c>ExpirationDate</c> holds the completed/created date for messages no longer queued.</summary>
public record MessageHistoryOverview(Guid Id, Guid TenantId, Guid CorrelationId, Guid DeviceId, string SerialNumber, string DeviceFamily,
    string MessageType, byte Priority, DateTimeOffset ExpirationDate, DateTimeOffset CreatedDate, DateTimeOffset? CompletedDate,
    string MessageState, string FileName, Guid FileTypeId, string Description);

public record MessageHistoryItem(Guid Id, Guid TenantId, Guid CorrelationId, Guid DeviceId, string SerialNumber, string DeviceFamily,
    string MessageType, byte Priority, DateTimeOffset ExpirationDate, DateTimeOffset CreatedDate, DateTimeOffset? CompletedDate,
    string MessageState, string? ContextType, string? ContextValue, string FileName, Guid FileTypeId, string? Payload,
    string Description, string? FormattedPayload);
