namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>Body for creating a clock group (server <c>ClockGroupAddCommand</c>). <c>NodeId</c> must match the route node.</summary>
/// <param name="SuspendTagging">When true the server does not trigger tag matching for this change.</param>
public record ClockGroupRequest(Guid TenantId, int NodeId, Guid CompanyId,
    string ExternalId, string Name, string Tags, bool SuspendTagging = false);
