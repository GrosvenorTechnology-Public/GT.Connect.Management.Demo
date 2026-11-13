using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GT.Connect.Management.Demo.ConnectApi.People;

public interface IConnectApiPeople : IConnectApiPeopleClockGroups, IConnectApiPeopleConsent, IConnectApiPeopleJobs,
    IConnectApiPeopleExtendedData, IConnectApiPeopleCredentials, IConnectApiPeopleSupervisorOverrides,
    IConnectApiPeopleTransactions, IConnectApiPeopleIntegrationEdge
{
    [Obsolete("Duplicate of GetPersons and the v1 endpoint is deprecated on the server, use GetPeopleV2 instead")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}")]
    Task<ApiResponse<List<PersonResponse>>> GetPeople(Guid tenantId, int nodeId);

    [Obsolete("Duplicate of GetCompanyByNodeId, use that instead")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/company")]
    Task<CompanyResponse> GetCompany(Guid tenantId, int nodeId);

    [Get("/api/tenants/{tenantId}/people/company/{companyId}")]
    Task<CompanyResponse> GetCompanyById(Guid tenantId, Guid companyId);

    [Post("/api/tenants/{tenantId}/people/{nodeId}")]
    Task<PersonResponse> AddPerson(Guid tenantId, int nodeId, [Body] PersonRequest person);

    [Get("/api/tenants/{tenantId}/people/{nodeId}/person/externalId/{externalId}")]
    Task<ApiResponse<PersonResponse>> GetPersonByExternalId(Guid tenantId, int nodeId, string externalId);

    [Get("/api/tenants/{tenantId}/people/{nodeId}/person/token/{token}")]
    Task<ApiResponse<PersonResponse>> GetPersonByToken(Guid tenantId, int nodeId, string token);

    [Obsolete("The v1 endpoint is deprecated on the server, use GetPersonV2 instead")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/person/{personId}")]
    Task<PersonResponse> GetPerson(Guid tenantId, int nodeId, Guid personId);

    [Get("/api/tenants/{tenantId}/people/{nodeId}/extendeddata/data/datatype/{dataTypeName}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<ExtendedDataQuery>> GetExtendedDataItems(Guid tenantId, int nodeId, string dataTypeName, [Query] GridifyQuery? gridifyModel);

    [Put("/api/tenants/{tenantId}/people/{nodeId}/{personId}")]
    Task<ApiResponse<PersonResponse>> UpdatePerson(Guid tenantId, int nodeId, Guid personId, [Body] PersonRequest person);

    [Obsolete("The server ignores the body and returns a bool, so this overload cannot deserialise the response. Use DeletePerson(tenantId, nodeId, personId) instead")]
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/{personId}")]
    Task<ApiResponse<PersonResponse>> DeletePerson(Guid tenantId, int nodeId, Guid personId, [Body] PersonDeleteRequest person);

    [Obsolete("The v1 endpoint is deprecated on the server, use GetSchedulesForPersonV2 instead")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/schedules/person/{personId}")]
    Task<ApiResponse<List<ExtendedDataItem>>> GetSchedulesForPerson(Guid tenantId, int nodeId, Guid personId);

    [Get("/api/tenants/{tenantId}/people/{nodeId}/supervisorOverride/person/{personId}")]
    Task<ApiResponse<List<SupervisorOverride>>> GetSupervisorOverridesForPerson(Guid tenantId, int nodeId, Guid personId);

    [Get("/api/tenants/{tenantId}/people/{nodeId}/supervisorOverride/person/externalId/{externalId}")]
    Task<ApiResponse<List<SupervisorOverride>>> GetSupervisorOverridesForPersonByExternalId(Guid tenantId, int nodeId, string externalId);

    //Note returns 204 if no active override
    [Get("/api/tenants/{tenantId}/people/{nodeId}/supervisorOverride/person/{personId}/active")]
    Task<ApiResponse<SupervisorOverride>> GetActiveSupervisorOverrideForPerson(Guid tenantId, int nodeId, Guid personId);

    //Note returns 204 if no active override
    [Get("/api/tenants/{tenantId}/people/{nodeId}/supervisorOverride/person/{personId}/use")]
    Task<ApiResponse<SupervisorOverride>> UseSupervisorOverrideForPerson(Guid tenantId, int nodeId, Guid personId);

    [Get("/api/tenants/{tenantId}/people/{nodeId}/company")]
    Task<CompanyResponse> GetCompanyByNodeId(Guid tenantId, int nodeId);

    [Obsolete("The v1 endpoint is deprecated on the server, use GetPeopleV2 instead")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}")]
    Task<ApiResponse<List<PersonResponse>>> GetPersons(Guid tenantId, int nodeId, [Query] PeopleSeiveQueryParams? qparams = null);

    [Delete("/api/tenants/{tenantId}/people/{nodeId}/{personId}")]
    Task<bool> DeletePerson(Guid tenantId, int nodeId, Guid personId);

    [Post("/api/tenants/{tenantId}/people/{nodeId}/command/ReplayTransactions")]
    Task<ApiResponse<string>> ReplayTransactions(Guid tenantId, int nodeId,
        [Body] ReplayTransactionsCommand command);

    [Obsolete("The v1 endpoint is deprecated on the server, use GetExtendedDataTypesV2 instead")]
    [Get("/api/tenants/{tenantId}/people/extendeddata/datatype")]
    Task<List<ExtendedDataTypeResponse>> GetExtendedDataTypes(Guid tenantId);

    [Post("/api/tenants/{tenantId}/people/{nodeId}/extendeddata/data")]
    Task<ExtendedDataResponse> CreateExtendedDataItem(Guid tenantId, int nodeId, [Body] List<ExtendedDataItemCreateRequest> data);

    /// <summary>Batch-updates extended data items by id. Note the server overwrites ConnectedEntityId, so always supply it.</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/extendeddata/data")]
    Task<ExtendedDataResponse> UpdateExtendedDataItem(Guid tenantId, int nodeId, [Body] List<ExtendedDataItemUpdateRequest> data);

    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/person/{personId}/clockGroups")]
    Task<ApiResponse<PagingResponse<ClockGroup>>> GetPersonClockgroups(Guid tenantId, int nodeId, Guid personId);

    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/transactions/node/{nodeId}/person/{personId}")]
    Task<ApiResponse<PagingResponse<PersonTransactionItemResponse>>> GetPersonTransactions(Guid tenantId, int nodeId, Guid personId, [Query] GridifyQuery? query);

    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/person/{personId}/credentials")]
    Task<ApiResponse<PagingResponse<Credential>>> GetPersonCredentials(Guid tenantId, int nodeId, Guid personId, [Query] GridifyQuery? query);

    // For migration use only
    [Put("/api/tenants/{tenantId}/migration/people/{personId}/templates")]
    Task<PersonResponse> AddMigratedLinuxFingerTemplate(Guid tenantId, Guid personId, [Body] MigratedTemplate migratedTemplate);

    // For migration use only
    [Put("/api/tenants/{tenantId}/migration/people/{personId}/faceTemplates")]
    Task<PersonResponse> AddMigratedAndroidFaceTemplate(Guid tenantId, Guid personId, [Body] MigratedTemplate migratedTemplate);

    // For migration use only
    [Put("/api/tenants/{tenantId}/migration/people/{personId}/fingerTemplates")]
    Task<PersonResponse> AddMigratedAndroidFingerTemplate(Guid tenantId, Guid personId, [Body] MigratedTemplate migratedTemplate);

    // For migration use only
    [Obsolete("The server has no migration photo endpoint (this always fails). Use UpdatePersonFromDevice with the Photo property instead")]
    [Put("/api/tenants/{tenantId}/migration/people/{personId}/photo")]
    Task<PersonResponse> AddMigratedPhoto(Guid tenantId, Guid personId, [Body] MigratedTemplate migratedTemplate);

    // ── People (v2 / additional endpoints) ─────────────────────────────────────

    /// <summary>Gets a page of people on a company node (v2, Gridify).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}")]
    Task<ApiResponse<PagingResponse<PersonResponse>>> GetPeopleV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a person by id (v2). Returns 404 when not found.</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/person/{personId}")]
    Task<ApiResponse<PersonResponse>> GetPersonV2(Guid tenantId, int nodeId, Guid personId);

    /// <summary>Gets a page of people on the node who have not yet given consent (v2, Gridify).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/missingConsent")]
    Task<ApiResponse<PagingResponse<PersonResponse>>> GetPeopleMissingConsent(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Creates or updates a person identified by <see cref="PersonUpsertRequest.PersonId"/> (clock-style upsert).</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/upsert")]
    Task<PersonResponse> UpsertPerson(Guid tenantId, int nodeId, [Body] PersonUpsertRequest person);

    /// <summary>Applies a JSON Patch (RFC 6902) document to a person, e.g. <c>new("replace", "/firstName", "Bob")</c>.</summary>
    [Patch("/api/tenants/{tenantId}/people/{nodeId}/{personId}")]
    Task<PersonResponse> PatchPerson(Guid tenantId, int nodeId, Guid personId, [Body] List<PersonPatchOperation> patch);

    /// <summary>Gets a person's anti-passback records (v2, Gridify). Returns 403 when anti-passback data is not visible.</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/person/{personId}/apb")]
    Task<ApiResponse<PagingResponse<PersonApbResponse>>> GetPersonApb(Guid tenantId, int nodeId, Guid personId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Machine-client only: gets the anti-passback details for a person/zone as JSON-converted XML (404 when none). Sends a body on GET.</summary>
    [Get("/api/tenants/{tenantId}/people/apb")]
    Task<ApiResponse<JsonElement>> GetPersonAntiPassbackDetails(Guid tenantId, [Body] PersonAntiPassbackRequest request);

    /// <summary>Machine-client only: gets the punch questionnaire (questions and answers) for a person as JSON-converted XML (404 when none). Sends a body on GET.</summary>
    [Get("/api/tenants/{tenantId}/people/questionnaire")]
    Task<ApiResponse<JsonElement>> GetPersonQuestionnaire(Guid tenantId, [Body] PersonQuestionnaireRequest request);

    /// <summary>Machine-client only: permanently deletes people tombstoned before <paramref name="expiryCutoff"/> (housekeeping endpoint).</summary>
    [Delete("/api/tenants/{tenantId}/people")]
    Task<IApiResponse> DeleteExpiredTombstonedPeople(Guid tenantId, [Query(Format = "o")] DateTimeOffset expiryCutoff);

    /// <summary>Machine-client only: updates a person (details, biometric templates, photo, consent) as if reported by a device (the ClockConnect path). The person must be in a clock group linked to the device.</summary>
    [Patch("/api/tenants/{tenantId}/people")]
    Task<PersonResponse> UpdatePersonFromDevice(Guid tenantId, [Body] PersonUpdateFromDeviceRequest request);

    /// <summary>Deletes a credential belonging to a person. Returns true on success.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/person/{personId}/credentials/{credentialId}")]
    Task<bool> DeletePersonCredential(Guid tenantId, int nodeId, Guid personId, Guid credentialId);

    // ── Companies ──────────────────────────────────────────────────────────────

    /// <summary>Machine-client only: gets a page of all People-service companies in the tenant (v2, Gridify).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/company")]
    Task<ApiResponse<PagingResponse<CompanyResponse>>> GetCompanies(Guid tenantId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Machine-client only: gets the People-service company for a node (404 when none).</summary>
    [Get("/api/tenants/{tenantId}/people/company/{nodeId}")]
    Task<ApiResponse<CompanyResponse>> GetCompanyByNodeIdInternal(Guid tenantId, int nodeId);

    /// <summary>Gets the highest numeric person ExternalRevisionId on the company node. Returns 204 when none, 400 when the company does not exist.</summary>
    [Get("/api/tenants/{tenantId}/people/company/{nodeId}/maxrevision")]
    Task<ApiResponse<int?>> GetCompanyMaxExternalRevisionId(Guid tenantId, int nodeId);

    /// <summary>Machine-client only: creates the People-service company record for a node.</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/company")]
    Task<CompanyResponse> CreateCompany(Guid tenantId, int nodeId, [Body] CreateCompanyRequest company);
}

public record MigratedTemplate(Guid DeviceId, string Value);

public record CredentialType(
        Guid Id,
        string Description,
        Guid FileTypeId
    );

public record Credential(
        Guid Id,
        Guid TenantId,
        int NodeId,
        Guid PersonId,
        string CredentialSensorType,
        string CredentialSensorDetails,
        int CredentialLength,
        int TemplateCount,
        CredentialType? CredentialType,
        Guid? CredentialProviderId,
        Guid CredentialTypeId,
        DateTimeOffset IssuedOn,
        DateTimeOffset ExpiresOn,
        string? BioFilterTags,
        Guid DeviceId,
        Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn,
        string? CredentialData = null)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn)
{
    /// <summary>Old (never bound) name for <see cref="BaseModifiyableObject{T}.LastModifiedById"/>.</summary>
    [Obsolete("Use LastModifiedById.")]
    [JsonIgnore]
    public Guid LastModifiedBy => LastModifiedById;
}

public enum SupervisorOverrideStatus
{
    Active,
    Used,
    Expired,
    Cancelled
}

public record SupervisorOverride(Guid Id, SupervisorOverrideStatus Status, Guid SupervisorEmpId, string SupervisorExternalId,
    string SupervisorDisplayName, Guid EmployeeEmpId, string EmployeeExternalId, string EmployeeDisplayName, Guid DeviceId,
    string DeviceName, string DeviceSerialNumber, DateTime ExpiresAt, DateTime TimeUsed, DateTime TimeExpired, int TimeToLiveMinutes,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn)
{
    /// <summary>Old (never bound) name for <see cref="BaseModifiyableObject{T}.LastModifiedById"/>.</summary>
    [Obsolete("Use LastModifiedById.")]
    [JsonIgnore]
    public Guid LastModifiedBy => LastModifiedById;
}

public record PersonDeleteRequest(Guid id, Guid companyId);

public class PeopleSeiveQueryParams
{
    [AliasAs("Filters")]
    public string? Filters { get; set; }
}

public class Page<TEntity> where TEntity : BaseModifiyableObject
{
    public List<TEntity> Payload { get; set; } = new();

    public int? PreviousPage { get; set; }

    public int? CurrentPage { get; set; }

    public int? NextPage { get; set; }

    public int? PageSize { get; set; }

    public int? TotalPages { get; set; }

    public int TotalItems { get; set; }
}

/// <summary>Result of a batch create/update of extended data (server <c>InsertBatchResponseDto</c> / <c>UpdateBatchResponseDto</c> / <c>UpdateByExternalIdBatchResponseDto</c>).</summary>
public record ExtendedDataResponse(List<ExtendedDataItem> ExtendedData, List<PeopleBatchItemFailure>? Failures = null);

public record ExtendedDataItem(
    string EntityData,
    Guid ExtendedDataTypeId,
    string ExternalId,
    Guid ConnectedEntityId,
    Guid CompanyId,
    int NodeId,
    Guid TenantId,
    Guid Id,
    Guid CreatedById,
    DateTimeOffset CreatedOn,
    Guid LastModifiedById,
    DateTimeOffset LastModifiedOn
);

public record ExtendedDataTypeResponse(
    string Name,
    Guid ExtensionPointId,
    Guid TenantId,
    bool IsSystem,
    Guid Id,
    Guid CreatedById,
    DateTimeOffset CreatedOn,
    Guid LastModifiedById,
    DateTimeOffset LastModifiedOn
);

public record ExtendedDataItemCreateRequest(Guid DataTypeId, Guid ConnectedEntityId, string ExternalId, string EntityData);

/// <param name="ConnectedEntityId">The server always overwrites the connected entity with this value, so supply the existing value to keep it.</param>
public record ExtendedDataItemUpdateRequest(Guid Id, string EntityData, Guid ConnectedEntityId = default);

public record ReplayTransactionsCommand(Guid CommandId, Guid TenantId, int NodeId, List<Guid> TransactionIds);

public record ClockGroup(string Name, Guid CompanyId, int NodeId, Guid TenantId, string ExternalId, string Tags,
    Guid Id = default, Guid CreatedById = default, DateTimeOffset CreatedOn = default,
    Guid LastModifiedById = default, DateTimeOffset LastModifiedOn = default);

public class ExtendedDataQuery
{
    /// <summary>
    /// Total count of items.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// List of extended data items.
    /// </summary>
    public List<ExtendedDataItem> Data { get; set; } = [];
}

public record PersonTransactionItemResponse(Guid Id, Guid TenantId, Guid CompanyId, Guid DeviceId, DateTimeOffset EventTimeLocal,
    int EventTimeOffset, string RawMessage, JsonObject? Message, DateTimeOffset MessageReceivedAt,
    [property: JsonPropertyName("corelationId")] Guid CorrelationId, int CompanyNodeId,
    bool IsOnlineTransaction, int? Outcome, DateTimeOffset? MessageDeliveredAt, Guid PersonId, string FirstName, string Surname, Guid? SessionId);
