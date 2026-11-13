using GT.Connect.Management.Demo.ConnectApi.Core;
using GT.Connect.Management.Demo.ConnectApi.DeviceConfig;
using GT.Connect.Management.Demo.ConnectApi.IntegrationEdge;

namespace GT.Connect.Management.Demo.ConnectApi.Forwarding;

public enum ForwarderType
{
    HttpForwarder = 0,
    ServiceBusForwarder = 1
}

public enum ForwarderStatus
{
    Unknown = 0,
    Healthy = 1,
    Warning = 2,
    Error = 3,
    Failed = 4,
    Disabled = 5
}

public enum ForwarderFilterByField
{
    EventName = 0,
    SourceType = 1
}

public enum ForwarderFilterByType
{
    Include = 0,
    Exclude = 1
}

// Forwarders

public record ForwarderResponse(Guid TenantId, int NodeId, string Name, DeviceFamily DeviceFamily,
    ForwarderType ForwarderType, ForwarderStatus Status, bool Enabled, string Destination,
    AuthType AuthenticationType, List<ForwarderFilterResponse>? Filters,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

public record AddForwarderRequest(int NodeId, string Name, DeviceFamily DeviceFamily, ForwarderType ForwarderType,
    bool Enabled, string Destination, AuthType AuthenticationType);

public record UpdateForwarderRequest(string Name, DeviceFamily DeviceFamily, ForwarderType ForwarderType,
    bool Enabled, string Destination, AuthType AuthenticationType);

// Filters

public record ForwarderFilterResponse(Guid ForwarderId, ForwarderFilterByField FilterByField, ForwarderFilterByType FilterByType,
    string FilterByValue,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

public record AddForwarderFilterRequest(ForwarderFilterByField FilterByField, ForwarderFilterByType FilterByType, string FilterByValue);

public record UpdateForwarderFilterRequest(ForwarderFilterByField FilterByField, ForwarderFilterByType FilterByType, string FilterByValue);

// Properties

public record ForwarderPropertyResponse(Guid ForwarderId, string Name, string Value, bool IsSecret, bool IsHeader, bool IsQueryString,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

public record AddForwarderPropertyRequest(string Name, string Value, bool IsSecret, bool IsHeader, bool IsQueryString);

/// <param name="Value">Null leaves the existing value unchanged (useful for secrets).</param>
public record UpdateForwarderPropertyRequest(string Name, string? Value, bool IsSecret, bool IsHeader, bool IsQueryString);

// Logs

public record ForwarderLogRequest(Guid TenantId, Guid ForwarderId,
    DateTime StartDateTime, DateTime EndDateTime, string? QueryString, int PageSize);

public record ForwarderLogResponse(Guid Id, Guid TenantId, Guid ForwarderId, int? CompanyNodeId, Guid DeviceId,
    string RequestData, int ResponseCode, string FailureDetail, DateTime TriggeredAt, DateTime RemoteRespondedAt);

// Metrics

public record ForwarderAggregateResponse(Guid TenantId, Guid ForwarderId, string DailyCountList, long TotalCount);

public record ForwarderFailureResponse(Guid ForwarderId, int NodeId, string NodeName, Guid TenantId, string ForwarderName,
    int ResponseCode, DateTimeOffset LastModifiedOn, string FailureDetail, ForwarderType ForwarderType);
