using GT.Connect.Management.Demo.ConnectApi.Core;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>Body for <c>PUT people/{nodeId}/upsert</c> (server <c>UpsertPersonRequest</c>). The person is matched on <paramref name="PersonId"/>.</summary>
public record PersonUpsertRequest(Guid CompanyId, string PersonId,
    int? Revision = null, string? Firstname = null, string? Surname = null, string? KeypadId = null,
    string? BadgeCode = null, string? Pin = null, string? TemplateId = null, string? Language = null,
    string? Roles = null, string? Token = null, string? DisplayItems = null, string? ExternalId = null);

/// <summary>A single JSON Patch (RFC 6902) operation, e.g. <c>new("replace", "/firstname", "Bob")</c>.</summary>
/// <param name="Op">add, remove, replace, move, copy or test.</param>
public record PersonPatchOperation(string Op, string Path, object? Value = null, string? From = null);

/// <summary>Anti-passback state of a person in a zone (server <c>PersonApbDto</c>).</summary>
public record PersonApbResponse(Guid Id, Guid TenantId, Guid PersonId, int ZoneIdentifier, string? PunchDirection,
    DateTimeOffset LastClockingAt,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

/// <summary>Body for the machine-client <c>GET people/apb</c> endpoint (server <c>GetPersonAntiPassbackCommand</c>).</summary>
public record PersonAntiPassbackRequest(Guid TenantId, Guid PersonId, int ZoneIdentifier);

/// <summary>Body for the machine-client <c>GET people/questionnaire</c> endpoint (server <c>GetQuestionsandAnswersCommand</c>).</summary>
public record PersonQuestionnaireRequest(Guid TenantId, Guid DeviceId, int NodeId, Guid PersonId, string PunchDirection);

/// <summary>A biometric template / photo payload (server <c>BiometricTemplatesDto</c>).</summary>
public record PersonBiometricTemplate(string Value, string? Reason = null);

/// <summary>Body for the machine-client <c>PATCH people</c> device-update endpoint (server <c>PersonUpdateFromClockRequest</c>).</summary>
/// <remarks>Name/credential fields are applied as sent, so populate them with the person's current values.</remarks>
public record PersonUpdateFromDeviceRequest(Guid Id, Guid DeviceId, DateTimeOffset Timestamp,
    string? Firstname = null, string? Surname = null, string? Language = null, string? Roles = null,
    string? VerifyBy = null, string? KeyPad = null, string? PinCode = null, string? BadgeCode = null,
    PersonBiometricTemplate? Templates = null, PersonBiometricTemplate? FingerTemplates = null,
    PersonBiometricTemplate? FaceTemplates = null, PersonBiometricTemplate? Photo = null,
    string? ConsentData = null, bool AnonymousEnrollment = false);

/// <summary>Body for the machine-client create-company endpoint (server <c>CompanyAddCommand</c>).</summary>
public record CreateCompanyRequest(Guid TenantId, int NodeId, string Name, DateTime EventTime, bool EnableAnonymousCredentials = false);

/// <summary>A failed item in a People batch operation (server <c>FailedDataIdDto</c> / <c>FailedDataExternalIdDto</c>).</summary>
public record PeopleBatchItemFailure(Guid? Id, string? ExternalId, string? ErrorCode);

/// <summary>Result of a People batch delete (server <c>DeleteBatchResponseDto</c> / <c>DeleteByExternalIdBatchResponseDto</c>).</summary>
public record PeopleBatchDeleteResponse(int Deleted, List<PeopleBatchItemFailure> Failures);

/// <summary>Body for creating a supervisor override (server <c>SupervisorOverrideAddRequest</c>).</summary>
public record SupervisorOverrideAddRequest(Guid CompanyId,
    Guid SupervisorEmpId, string SupervisorExternalId, string SupervisorDisplayName,
    Guid EmployeeEmpId, string EmployeeExternalId, string EmployeeDisplayName,
    Guid DeviceId, string DeviceName, string DeviceSerialNumber,
    DateTime ExpiresAt, DateTime TransactionTime, int TimeToLiveMinutes);

/// <summary>Body for updating a supervisor override (server <c>SupervisorOverrideUpdateRequest</c>).</summary>
public record SupervisorOverrideUpdateRequest(Guid SupervisorOverrideId, Guid CompanyId,
    Guid SupervisorEmpId, string SupervisorExternalId, string SupervisorDisplayName,
    Guid EmployeeEmpId, string EmployeeExternalId, string EmployeeDisplayName,
    Guid DeviceId, string DeviceName, string DeviceSerialNumber, int TimeToLiveMinutes);
