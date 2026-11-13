using System.Text.Json.Serialization;
using GT.Connect.Management.Demo.ConnectApi.Core;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>A person as returned by the People service (server <c>PersonDto</c>).</summary>
public record PersonResponse (Guid CompanyId, Guid TenantId, int NodeId, Guid Id,
    string Firstname, string Surname, string ExternalId, string BadgeCode, string KeyPad, string PinCode,
    string Language, string Roles, string VerifyBy, bool IsDeleted, string ExternalRevisionId, string? Token,
    string? DisplayItems, string? BioFilterTags, string? ExtendedData,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn,
    string? Email = null, string? Telephone = null, bool BlockBiometrics = false) 
    : BaseModifiyableObject (Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn)
{
    /// <summary>Old (never bound) name for <see cref="BaseModifiyableObject{T}.LastModifiedById"/>.</summary>
    [Obsolete("Use LastModifiedById.")]
    [JsonIgnore]
    public Guid LastModifiedBy => LastModifiedById;
}
