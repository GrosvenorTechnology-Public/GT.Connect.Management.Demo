using System.Text.Json.Serialization;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>A People-service company (server <c>CompanyDto</c>).</summary>
public record CompanyResponse(int NodeId, Guid TenantId, Guid Id, string Name,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn,
    int? ParentNodeId = null, string? Action = null, DateTime CreationDate = default, bool EnableAnonymousCredentials = false)
{
    /// <summary>Old (never bound) name for <see cref="LastModifiedById"/>.</summary>
    [Obsolete("Use LastModifiedById.")]
    [JsonIgnore]
    public Guid LastModifiedBy => LastModifiedById;
}
