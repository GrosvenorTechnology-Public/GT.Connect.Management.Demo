using Refit;
using System.Text.Json;

namespace GT.Connect.Management.Demo.ConnectApi.AuditLog;

/// <summary>
/// AuditLog service endpoints.
/// </summary>
public interface IConnectApiAuditLog
{
    /// <summary>
    /// Streams audit log entries (newest first, max 500) as NDJSON; each line is an <see cref="AuditLogEntryResponse"/>.
    /// The body TenantId must match the route (400 otherwise). Entries are not filtered by <paramref name="nodeId"/>, which is only used for the permission check.
    /// </summary>
    [Post("/api/tenants/{tenantId}/audit/node/{nodeId}/auditLogs/stream")]
    Task<HttpResponseMessage> StreamAuditLogs(Guid tenantId, int nodeId, [Body] AuditLogRequest request, CancellationToken cancellationToken = default);
}

/// <summary>Audit log query. <paramref name="PageSize"/> outside 1..500 is treated as 500. <paramref name="QueryString"/> matches payload, entity name, type, username, correlation id or primary key.</summary>
public record AuditLogRequest(Guid TenantId, DateTime StartDateTime, DateTime EndDateTime, string? QueryString = null, int PageSize = 500);

/// <summary>One NDJSON line from <see cref="IConnectApiAuditLog.StreamAuditLogs"/>. The server writes PascalCase property names and the enum as a number.</summary>
public record AuditLogEntryResponse(Guid Id, string? ExternalId, Guid TenantId, string? PrimaryKey, string? ServiceName,
    Dictionary<string, JsonElement>? Payload, string? ActionType, string? Type, string? CorrelationId, string? Username,
    Guid CreatedById, DateTimeOffset CreatedOn, string? NodeName, int? NodeId, LogMessageType LogMessageType, string? EntityNameValue);

public enum LogMessageType
{
    TableChange,
    UserCommand
}
