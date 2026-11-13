using System.Text.Json.Serialization;
using GT.Connect.Management.Demo.ConnectApi.Core;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>Clock group as returned by the People service (server <c>ClockGroupDto</c>).</summary>
public record ClockGroupResponse(int NodeId, Guid TenantId, Guid CompanyId, Guid Id,
    string Name, string? ExternalId,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn,
    string? Tags = null)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn)
{
    /// <summary>Old (never bound) name for <see cref="BaseModifiyableObject{T}.LastModifiedById"/>.</summary>
    [Obsolete("Use LastModifiedById.")]
    [JsonIgnore]
    public Guid LastModifiedBy => LastModifiedById;
}
