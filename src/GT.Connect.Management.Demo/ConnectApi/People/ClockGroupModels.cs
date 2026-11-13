using GT.Connect.Management.Demo.ConnectApi.Core;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>Clock group with member counts (server <c>ClockGroupExtendedDto</c>), returned by the v2 <c>withcounts</c> endpoint.</summary>
public record ClockGroupWithCountsResponse(int NodeId, Guid TenantId, Guid CompanyId, Guid Id,
    string Name, string? ExternalId, string? Tags, int DeviceCount, int PeopleCount,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

/// <summary>Person-to-clock-group link returned when adding people to a clock group (server <c>PersonToClockGroupDto</c>).</summary>
public record PersonToClockGroupResponse(Guid PersonId, Guid ClockGroupId);
