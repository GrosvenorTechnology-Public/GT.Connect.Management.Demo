using System.Text.Json.Serialization;
using GT.Connect.Management.Demo.ConnectApi.Core;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>Body for linking (activating) a consent agreement on a node (server <c>ConsentLinkAddCommand</c>).</summary>
/// <param name="NodeId">The node the consent is activated on (the route node is the company node).</param>
public record ActivateConsentCommand(int NodeId, Guid ConsentAgreementId, Guid TenantId = default);

/// <summary>Consent link (server <c>ConsentLinkDto</c>).</summary>
public record ActivateConsenttResponse(Guid TenantId, int NodeId, Guid Id, Guid ConsentAgreementId,
    string Description, Guid CredentialTypeId, int DurationFor, int StartWarningBeforeExpire,
    List<ConsentSummaryDetail> ConsentDetails,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn,
    Guid FileTypeId = default)
{
    /// <summary>Old (misspelt, never bound) name for <see cref="ConsentDetails"/>.</summary>
    [Obsolete("Use ConsentDetails.")]
    [JsonIgnore]
    public List<ConsentSummaryDetail> DonsentDetails => ConsentDetails;

    /// <summary>Old (never bound) name for <see cref="LastModifiedById"/>.</summary>
    [Obsolete("Use LastModifiedById.")]
    [JsonIgnore]
    public Guid LastModifiedBy => LastModifiedById;
}

/// <summary>Identifies a consent link to remove.</summary>
public record DeactivateConsentCommand(int NodeId, Guid ConsentLinkId);

/// <summary>Consent text/language pair on a consent link (server <c>ConsentDetailsDto</c>).</summary>
public record ConsentSummaryDetail(string ConsentText, string Language);

/// <summary>Body for creating a consent agreement (server <c>ConsentAgreementAddCommand</c>). NodeId must match the route.</summary>
public record CreateConsentCommand(Guid TenantId, int NodeId, Guid CompanyId, string Description,
    Guid CredentialTypeId, int DurationFor, int StartWarningBeforeExpire, uint Version);

/// <summary>Body for updating a consent agreement (server <c>ConsentAgreementUpdateRequest</c>). NodeId must match the route.</summary>
public record UpdateConsentCommand(Guid Id, int NodeId, Guid CompanyId, Guid CredentialTypeId, string Description,
    int DurationFor, int StartWarningBeforeExpire, int Version);

/// <summary>Body for creating a consent agreement text (server <c>ConsentAgreementTextAddCommand</c>).</summary>
public record CreateConsentAgreementTextCommand(Guid TenantId, int NodeId, Guid ConsentAgreementId,
    string Description, string Language, string ConsentText);

/// <summary>Body for updating a consent agreement text (server <c>ConsentAgreementTextUpdateRequest</c>).</summary>
public record UpdateConsentAgreementTextCommand(Guid Id, Guid ConsentAgreementId, Guid TenantId,
    string Description, string Language, string ConsentText);

/// <summary>Consent agreement (server <c>ConsentAgreementDto</c>). The server does not return NodeId (always 0).</summary>
public record ConsentResponse(Guid TenantId, int NodeId, Guid CompanyId, Guid Id, string Description,
    Guid? CredentialTypeId, int DurationFor, int StartWarningBeforeExpire, uint Version,
    List<ConsentAgreementText> ConsentAgreementsText,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
{
    /// <summary>Old (never bound) name for <see cref="LastModifiedById"/>.</summary>
    [Obsolete("Use LastModifiedById.")]
    [JsonIgnore]
    public Guid LastModifiedBy => LastModifiedById;
}

/// <summary>Consent agreement text (server <c>ConsentAgreementTextDto</c>).</summary>
public record ConsentAgreementText(Guid TenantId, Guid Id, Guid ConsentAgreementId, string Description,
    string Language, string ConsentText,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
{
    /// <summary>Old (never bound) name for <see cref="LastModifiedById"/>.</summary>
    [Obsolete("Use LastModifiedById.")]
    [JsonIgnore]
    public Guid LastModifiedBy => LastModifiedById;
}

/// <summary>Consent audit entry for a person (server <c>ConsentAuditDto</c>).</summary>
public record ConsentAuditResponse(Guid Id, Guid TenantId, Guid PersonId, Guid? CredentialId, Guid CredentialTypeId,
    string? ConsentText, DateTimeOffset EventTime, DateTimeOffset? ConsentExpiresOn, string[]? FingerTemplateDigits,
    string? State, Guid? DeviceId, int Version, Guid? SupervisorId, string? SupervisorExternalId,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);
