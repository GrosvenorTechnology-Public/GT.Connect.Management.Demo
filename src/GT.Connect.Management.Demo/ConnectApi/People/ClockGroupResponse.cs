namespace GT.Connect.Management.Demo.ConnectApi.People;

public record ClockGroupResponse(int NodeId, Guid TenantId, Guid CompanyId, Guid Id,
    string Name, string ExternalId,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn);
