using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>People-service transaction logs, verification images and metrics.</summary>
public interface IConnectApiPeopleTransactions
{
    // ── Transactions ──────────────────────────────────────────────────────────

    /// <summary>Gets a page of the transactions of a company node (v2, Gridify).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/Transactions/node/{nodeId}")]
    Task<ApiResponse<PagingResponse<TransactionResponse>>> GetCompanyTransactions(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a page of the transactions from a device (v2, Gridify).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/Transactions/node/{nodeId}/device/{deviceId}")]
    Task<ApiResponse<PagingResponse<TransactionResponse>>> GetDeviceTransactions(Guid tenantId, int nodeId, Guid deviceId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a page of the transactions from a device, by serial number (v2, Gridify).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/Transactions/node/{nodeId}/device/{serialNumber}")]
    Task<ApiResponse<PagingResponse<TransactionResponse>>> GetDeviceTransactionsBySerialNumber(Guid tenantId, int nodeId, string serialNumber, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a page of a person's transactions with a free-text search term (v2, Gridify). See also GetPersonTransactions.</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/Transactions/node/{nodeId}/person/{personId}")]
    Task<ApiResponse<PagingResponse<PersonTransactionItemResponse>>> SearchPersonTransactions(Guid tenantId, int nodeId, Guid personId, [Query] GridifyQuery? gridifyQuery = null, [Query] string? search = null);

    /// <summary>Downloads the verification image (PNG) captured for a transaction session. Returns 204 when there is none.</summary>
    [Get("/api/tenants/{tenantId}/people/verificationImage/node/{nodeId}/sessionid/{sessionId}")]
    Task<HttpResponseMessage> GetVerificationImage(Guid tenantId, int nodeId, Guid sessionId);

    // ── Metrics ───────────────────────────────────────────────────────────────

    /// <summary>Gets the number of people on a company node.</summary>
    [Get("/api/tenants/{tenantId}/people/Metrics/peopleCount/{nodeId}")]
    Task<int> GetPeopleCount(Guid tenantId, int nodeId);

    /// <summary>Gets the number of people that are sent to a device (via its clock groups).</summary>
    [Get("/api/tenants/{tenantId}/people/Metrics/{nodeId}/peopleOnDevice/{deviceId}")]
    Task<int> GetPeopleCountOnDevice(Guid tenantId, int nodeId, Guid deviceId);

    /// <summary>Gets the number of credentials on a company node.</summary>
    [Get("/api/tenants/{tenantId}/people/Metrics/credentialTotal/{nodeId}")]
    Task<int> GetCredentialTotal(Guid tenantId, int nodeId);

    /// <summary>Gets the daily transaction aggregate of a company node (cached for 2 minutes). Returns 204 when there is no data.</summary>
    [Get("/api/tenants/{tenantId}/people/Metrics/aggregate-tx/{nodeId}")]
    Task<ApiResponse<TransactionAggregateResponse>> GetCompanyTransactionAggregate(Guid tenantId, int nodeId, [Query] bool forceCacheUpdate = false);

    /// <summary>Gets the daily transaction aggregate of a node including its child nodes (cached for 2 minutes). Returns 204 when there is no data.</summary>
    [Get("/api/tenants/{tenantId}/people/Metrics/aggregate-tx/{nodeId}/includechildren")]
    Task<ApiResponse<TransactionAggregateResponse>> GetTransactionAggregateIncludingChildren(Guid tenantId, int nodeId, [Query] bool forceCacheUpdate = false);

    /// <summary>Gets the daily transaction aggregate of a device over 7, 14 or 30 days (other values fall back to 30). Returns 204 when there is no data.</summary>
    [Get("/api/tenants/{tenantId}/people/Metrics/aggregate-tx/{nodeId}/device/{deviceId}")]
    Task<ApiResponse<TransactionAggregateResponse>> GetDeviceTransactionAggregate(Guid tenantId, int nodeId, Guid deviceId, [Query] int days = 30, [Query] bool forceCacheUpdate = false);
}

/// <summary>Delivery outcome of a transaction (server <c>Outcome</c>; serialised as a number).</summary>
public enum TransactionOutcome
{
    Unknown = 0,
    Success = 1,
    RetryableError = 2,
    NonRetryableError = 3,
    Discard = 4
}

/// <summary>A transaction log entry (server <c>TransactionDto</c>). <c>Message</c> is the transaction XML converted to JSON.</summary>
public record TransactionResponse(Guid Id, Guid TenantId, Guid CompanyId, Guid DeviceId, DateTimeOffset EventTimeLocal,
    int EventTimeOffset, string? RawMessage, JsonObject? Message, DateTimeOffset MessageReceivedAt,
    [property: JsonPropertyName("corelationId")] Guid CorrelationId, int CompanyNodeId, bool IsOnlineTransaction,
    TransactionOutcome? Outcome, DateTimeOffset? MessageDeliveredAt, Guid? PersonId, string? FirstName, string? Surname, Guid? SessionId);

/// <summary>Daily transaction aggregate (server <c>TransactionLogsMonthlyAggregateDto</c>). <c>DailyCountList</c> is a JSON-encoded list of day buckets.</summary>
public record TransactionAggregateResponse(Guid TenantId, string? DailyCountList, long TotalSuccess, long TotalRetry, long TotalFailed, long TotalDiscard);
