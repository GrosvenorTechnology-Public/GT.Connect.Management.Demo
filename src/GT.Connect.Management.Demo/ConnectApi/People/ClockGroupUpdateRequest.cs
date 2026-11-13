namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>Body for updating a clock group (server <c>ClockGroupUpdateCommand</c>). <c>Id</c> must match the route id.</summary>
/// <param name="Tags">Clock group tags. NB: the server overwrites the stored tags with this value, so leaving it null clears them.</param>
/// <param name="SuspendTagging">When true the server does not trigger tag matching for this change.</param>
public record ClockGroupUpdateRequest(int NodeId, Guid TenantId, Guid CompanyId, Guid Id,
    string Name, string ExternalId, string? Tags = null, bool SuspendTagging = false);
