using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>Machine-client only People batch endpoints used by Integration Edge imports (<c>people/{nodeId}/integrationedge/...</c>).</summary>
public interface IConnectApiPeopleIntegrationEdge
{
    // ── People ────────────────────────────────────────────────────────────────

    /// <summary>Gets a page of people on a company node INCLUDING tombstoned people, with clock group / job code external ids (v2, Gridify).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/integrationedge/People")]
    Task<ApiResponse<PagingResponse<IntegrationEdgePersonResponse>>> GetIntegrationEdgePeople(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Batch-creates people. 201 when all succeed, 207 with failures otherwise, 204 for an empty batch.</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/integrationedge/People")]
    Task<ApiResponse<PeopleBatchResponse>> AddIntegrationEdgePeople(Guid tenantId, int nodeId, [Body] List<IntegrationEdgePersonAddRequest> people,
        [Query, AliasAs("preserveclockgroups")] bool? preserveClockGroups = null);

    /// <summary>Batch-updates people (matched by Id). 200 when all succeed, 207 with failures otherwise, 204 for an empty batch.</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/integrationedge/People")]
    Task<ApiResponse<PeopleBatchResponse>> UpdateIntegrationEdgePeople(Guid tenantId, int nodeId, [Body] List<IntegrationEdgePersonUpdateRequest> people,
        [Query, AliasAs("preserveclockgroups")] bool? preserveClockGroups = null);

    /// <summary>Batch-tombstones people by id. 200 when all succeed, 207 with failures otherwise, 204 for an empty batch.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/integrationedge/People")]
    Task<ApiResponse<PeopleBatchDeleteResponse>> DeleteIntegrationEdgePeople(Guid tenantId, int nodeId, [Body] List<Guid> personIds);

    // ── Extended data ─────────────────────────────────────────────────────────

    /// <summary>Gets a page of all extended data on a company node (v2, Gridify). Returns 204 when empty.</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/integrationedge/extendeddata")]
    Task<ApiResponse<PagingResponse<ExtendedDataItem>>> GetIntegrationEdgeExtendedData(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Batch-creates extended data items. 201 when all succeed, 207 with failures otherwise, 204 for an empty batch.</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/integrationedge/extendeddata")]
    Task<ApiResponse<ExtendedDataResponse>> AddIntegrationEdgeExtendedData(Guid tenantId, int nodeId, [Body] List<ExtendedDataItemCreateRequest> data);

    /// <summary>Batch-updates extended data items by id (the server overwrites ConnectedEntityId). 201 when all succeed, 207 with failures otherwise.</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/integrationedge/extendeddata")]
    Task<ApiResponse<ExtendedDataResponse>> UpdateIntegrationEdgeExtendedData(Guid tenantId, int nodeId, [Body] List<ExtendedDataItemUpdateRequest> data);

    /// <summary>Batch-updates extended data items of a data type by external id. 201 when all succeed, 207 with failures otherwise.</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/integrationedge/extendeddata/datatype/{dataTypeName}")]
    Task<ApiResponse<ExtendedDataResponse>> UpdateIntegrationEdgeExtendedDataByExternalId(Guid tenantId, int nodeId, string dataTypeName, [Body] List<ExtendedDataItemUpdateByExternalIdRequest> data);

    /// <summary>Batch-deletes extended data items of a data type by external id. 200 when all succeed, 207 with failures otherwise.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/integrationedge/extendeddata/datatype/{dataTypeName}")]
    Task<ApiResponse<PeopleBatchDeleteResponse>> DeleteIntegrationEdgeExtendedDataByExternalId(Guid tenantId, int nodeId, string dataTypeName, [Body] List<string> externalIds);

    /// <summary>Batch-deletes extended data items by id. 200 when all succeed, 207 with failures otherwise.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/integrationedge/extendeddata")]
    Task<ApiResponse<PeopleBatchDeleteResponse>> DeleteIntegrationEdgeExtendedData(Guid tenantId, int nodeId, [Body] List<Guid> ids);
}

/// <summary>A person as returned by the Integration Edge people endpoints (server <c>ExtendedData.PersonDto</c>).</summary>
public record IntegrationEdgePersonResponse(Guid Id, string? ExternalId, Guid TenantId, int NodeId,
    string? BadgeCode, string? KeyPad, string? PinCode, string? VerifyBy, string? FirstName, string? Surname,
    List<string>? ClockGroups, List<string>? JobCodes, string? Language, string? Roles, string? DisplayItems,
    string? ExternalRevisionId, string? Token, string? BioFilterTags, string? ExtendedData, bool IsDeleted,
    string? Email, string? Telephone,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

/// <summary>A person that failed in a batch (server <c>FailedPersonDto</c>).</summary>
public record PeopleBatchFailure(string? ExternalId, string? Name, string? ErrorMessage);

/// <summary>Result of a people batch create/update (server <c>PeopleBatchDto</c>).</summary>
public record PeopleBatchResponse(List<IntegrationEdgePersonResponse> People, List<PeopleBatchFailure> Failures);

/// <summary>A person in a batch create (server <c>ExtendedData.People.PersonAddCommand</c>). ClockGroups/JobCodes are ids.</summary>
public record IntegrationEdgePersonAddRequest
{
    public string? ExternalId { get; init; }
    public string? Firstname { get; init; }
    public string? Surname { get; init; }
    public string? Name { get; init; }
    public string? BadgeCode { get; init; }
    public string? KeyPad { get; init; }
    public string? PinCode { get; init; }
    public string? VerifyBy { get; init; }
    public string? DisplayItems { get; init; }
    public List<Guid>? ClockGroups { get; init; }
    public List<Guid>? JobCodes { get; init; }
    public string? Language { get; init; }
    public string? Roles { get; init; }
    public string? ExternalRevisionId { get; init; }
    public string? Token { get; init; }
    public string? BioFilterTags { get; init; }
    public string? ExtendedData { get; init; }
    public string? Email { get; init; }
    public string? Telephone { get; init; }
}

/// <summary>A person in a batch update (server <c>ExtendedData.People.PersonUpdateCommand</c>). ClockGroups/JobCodes are ids.</summary>
public record IntegrationEdgePersonUpdateRequest
{
    public required Guid Id { get; init; }
    public string? FirstName { get; init; }
    public string? Surname { get; init; }
    public string? Name { get; init; }
    public string? BadgeCode { get; init; }
    public string? KeyPad { get; init; }
    public string? PinCode { get; init; }
    public string? VerifyBy { get; init; }
    public string? DisplayItems { get; init; }
    public List<Guid>? ClockGroups { get; init; }
    public List<Guid>? JobCodes { get; init; }
    public string? Language { get; init; }
    public string? Roles { get; init; }
    public string? ExternalRevisionId { get; init; }
    public string? Token { get; init; }
    public string? BioFilterTags { get; init; }
    public string? ExtendedData { get; init; }
    public string? Email { get; init; }
    public string? Telephone { get; init; }
}
