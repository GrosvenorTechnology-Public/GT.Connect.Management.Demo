namespace GT.Connect.Management.Demo.ConnectApi.People;

public record ClockGroupUpdateRequest(int NodeId, Guid TenantId, Guid CompanyId, Guid Id,
    string Name, string ExternalId);