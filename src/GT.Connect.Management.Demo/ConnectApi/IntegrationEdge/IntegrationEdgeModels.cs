using System.Text.Json.Serialization;
using GT.Connect.Management.Demo.ConnectApi.Core;

namespace GT.Connect.Management.Demo.ConnectApi.IntegrationEdge;

// Server-Actions

public enum AuthType
{
    None,
    OAuth,
    Basic
}

public enum PayloadType
{
    XML = 0,
    Json = 1,
}

public record ServerActionResponse(Guid TenantId, int NodeId, string Name, string Endpoint,
    bool IsEnabled, PayloadType PayloadType, List<ServerActionPropertyResponse> Properties, AuthType AuthenticationType,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedBy, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedBy, LastModifiedOn);

public record ServerActionPropertyResponse(string Name, string Value, bool IsSecret, bool IsHeader,
    bool IsQueryString, Guid ServerActionId,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedBy, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedBy, LastModifiedOn);

// NB: the server's create command names this property 'AuthType' (update uses 'AuthenticationType').
public record AddServerActionRequest(Guid TenantId, int NodeId, string Name, string Endpoint,
    bool IsEnabled, AuthType AuthType, PayloadType PayloadType);

public record UpdateServerActionRequest(Guid TenantId, Guid Id, string Name, string Endpoint,
    bool IsEnabled, PayloadType PayloadType, AuthType AuthenticationType);

public record AddServerActionPropertyRequest(string Name, string Value, bool IsSecret, bool IsHeader, bool IsQueryString);

public record UpdateServerActionPropertyRequest(string Name, string? Value, bool IsSecret, bool IsHeader, bool IsQueryString);

public record ServerActionDeviceRequest(string Action, string RequestedOn, string SourceDeviceSerial,
    string SourceDeviceId, string TenantId, Guid ServerActionId, Dictionary<string, string> Payload,
    Dictionary<string, string>? PayloadContext = null);

public record ServerActionStatus(string State, string? Reason);

public record ServerActionRequestResponse(DateTime RespondedOn, ServerActionStatus Status, string Result);

public record ServerActionLogRequest(Guid TenantId, Guid ServerActionId,
    DateTime StartDateTime, DateTime EndDateTime, string QueryString, int PageSize);

public record ServerActionLogResponse(Guid Id, Guid ServerActionId, Guid TenantId, Guid CompanyId,
    Guid DeviceId, string RequestData, int ResponseCode, string FailureDetail,
    DateTimeOffset TriggeredAt, DateTimeOffset RemoteResponsedAt);

// Components

// FailureDisableThreshold is only returned for import components.
public record ComponentNameResponse(Guid TenantId, Guid Id, string Name, int? FailureDisableThreshold = null);

public record AddImportComponentRequest(Guid TenantId, int NodeId, string Name, string BaseUri,
    string Files, string PersonOverrides, int? FailureDisableThreshold);

public record UpdateImportComponentRequest(Guid Id, string Name, Guid TenantId, string BaseUri,
    string Files, string PersonOverrides, int? FailureDisableThreshold);

public record AddImportComponentPropertyRequest(Guid TenantId, Guid ImportComponentId,
    string Name, string FieldType, string Value, bool IsSecret);

public record UpdateImportComponentPropertyRequest(Guid TenantId, Guid ImportComponentId, Guid Id,
    string Name, string FieldType, string Value, bool IsSecret);

public record AddImportComponentJobPropertyRequest(Guid TenantId, Guid ImportComponentId,
    string Name, string FieldType, bool IsOptional, string DefaultValue, bool IsSecret,
    string CustomValidator = "", string CustomValidatorMessage = "");

public record UpdateImportComponentJobPropertyRequest(Guid TenantId, Guid ImportComponentId, Guid Id,
    string Name, string FieldType, bool IsOptional, string DefaultValue, bool IsSecret,
    string CustomValidator = "", string CustomValidatorMessage = "");

public record AddExportComponentRequest(Guid TenantId, int NodeId, string Name, string Uri);

public record UpdateExportComponentRequest(Guid Id, string Name, Guid TenantId, string Uri);

public record AddExportComponentPropertyRequest(Guid TenantId, int NodeId, Guid ExportComponentId,
    string Name, string FieldType, string Value, bool IsSecret);

public record UpdateExportComponentPropertyRequest(Guid TenantId, Guid ExportComponentId, Guid Id,
    string Name, string FieldType, string Value, bool IsSecret);

public record AddExportComponentJobPropertyRequest(Guid TenantId, Guid ExportComponentId,
    string Name, string FieldType, bool IsOptional, string DefaultValue, bool IsSecret,
    string CustomValidator = "", string CustomValidatorMessage = "");

public record UpdateExportComponentJobPropertyRequest(Guid TenantId, Guid ExportComponentId, Guid Id,
    string Name, string FieldType, bool IsOptional, string DefaultValue, bool IsSecret,
    string CustomValidator = "", string CustomValidatorMessage = "");

// Jobs

public record UpdateImportJobRequest(Guid TenantId, int NodeId, Guid ImportComponentId, Guid Id,
    string Name, string Hostname, string Username, string Password, bool IsEnabled,
    int UpdateFrequencyInterval, string FileExclusions, int? FailureDisableThreshold);

public record AddImportJobPropertyRequest(Guid TenantId, int NodeId, Guid ImportJobId, string Name, string Value);

public record UpdateImportJobPropertyRequest(Guid TenantId, int NodeId, Guid ImportJobId, Guid Id, string Name, string Value);

public record UpdateExportJobRequest(Guid TenantId, int NodeId, Guid ExportComponentId, Guid Id,
    string Name, string HostName, string UserName, string Password);

public record AddExportJobPropertyRequest(Guid TenantId, int NodeId, Guid ExportJobId, string Name, string Value);

public record UpdateExportJobPropertyRequest(Guid TenantId, int NodeId, Guid ExportJobId, Guid Id, string Name, string Value);

// Import process, progress and reporting

public record ImportProcessResponse(Guid Id, Guid ImportJobId, int NodeId, Guid TenantId,
    DateTime TimeToLive, string ParentFolderName, string FolderPath, string CurrentState, bool IsEnabled,
    Guid CreatedById = default, DateTimeOffset CreatedOn = default, Guid LastModifiedById = default, DateTimeOffset LastModifiedOn = default);

// NB: the server formats TimeToLive with DateTime.ToString() (culture dependent), hence a string here.
public record ImportProcessDetailResponse(Guid Id, Guid ImportJobId, int NodeId, Guid TenantId,
    string ParentFolderName, string FolderPath, string CurrentState, string TimeToLive,
    List<ImportProcessMessageResponse>? ImportProcessMessages,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

public record ImportProcessMessageResponse(Guid ImportProcessId, DateTime Timestamp, MessageType MessageType, string Message,
    string? Operation, string? Entity, long? Value, TimeSpan? Duration);

public record StartImportProcessRequest(string Hostname);

public record ImportStartResponse(Uri StorageUri, DateTime RequestedOn, Guid TenantId, int NodeId);

public record ImportProgressOutcomeRequest(bool Success, IEnumerable<string> Messages);

public record ProcessNotificationRequest(Guid TenantId, Guid UserId, Guid ImportProcessId, bool IsSuceess, List<string> Messages);

public enum MessageType
{
    Info = 0,
    Warning = 1,
    Error = 2,
    Unauthorised = 3
}

public record ImportProcessMessageRequest(DateTime Timestamp, MessageType MessageType, string Message,
    string? Operation, string? Entity, long? Value, TimeSpan? Duration);

// Transaction logging

public record AddExportJobTransactionStatus(Guid MessageId, Guid CorrelationId, Guid CompanyId,
    string DeviceId, string SerialNumber, string PersonId, DateTime EventTimeLocal, double EventTimeOffset,
    string Message, DateTimeOffset MessageReceivedAt, bool IsOnlineTransaction,
    Outcome OperationState, ErrorClass OperationErrorClass,
    [property: JsonConverter(typeof(JsonStringEnumConverter))] SourceType? SourceType = null,
    string? SourceName = null, Guid SourceId = default);

public record TransactionDeliveryLogRequest(Guid TenantId, int NodeId,
    DateTime StartDateTime, DateTime EndDateTime, string QueryString, int PageSize);

public record TransactionDeliveryLogResponse(Guid Id, Guid MessageId, Guid CorrelationId, Guid TenantId,
    int CompanyNodeId, string SourceType, string SourceName, Guid SourceId, DateTime CreatedOn,
    string OperationState, string OperationErrorClass, string Message);

// Metrics

public record ServerActionAggregateResponse(Guid TenantId, string ServerActionName, Guid ServerActionId,
    string DailyCountList, long TotalCount);

public record ServerActionFailureResponse(Guid TenantId, string ServerActionName, Guid ServerActionId,
    int ResponseCode, DateTime TriggeredAt, string FailureDetail);

public record TransactionStatusesResponse(DateTimeOffset EventDate, int SuccessCount, int FailureCount, int RetryCount);

// Assistant

public record AssistantChatRequest(string Message);
