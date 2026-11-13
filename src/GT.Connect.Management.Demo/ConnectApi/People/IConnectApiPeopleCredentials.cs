using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>People-service credentials, credential types and credential providers.</summary>
public interface IConnectApiPeopleCredentials
{
    // ── Credentials ───────────────────────────────────────────────────────────

    /// <summary>Gets a page of a person's credentials (v2, Gridify; requires PeopleAdmin).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/Credential/node/{nodeId}/person/{personId}")]
    Task<ApiResponse<PagingResponse<Credential>>> GetCredentialsForPerson(Guid tenantId, int nodeId, Guid personId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a page of a person's credentials of one credential type (v2, Gridify; requires PeopleAdmin).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/Credential/node/{nodeId}/person/{personId}/type/{credentialTypeId}")]
    Task<ApiResponse<PagingResponse<Credential>>> GetCredentialsForPersonByType(Guid tenantId, int nodeId, Guid personId, Guid credentialTypeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Issues a (mobile) credential to a person via a credential provider. Returns 201 with the credential.</summary>
    [Post("/api/tenants/{tenantId}/people/Credential/issue")]
    Task<Credential> IssueCredential(Guid tenantId, [Body] CredentialIssueRequest request);

    /// <summary>Revokes a credential. Returns 200 with a confirmation message.</summary>
    [Post("/api/tenants/{tenantId}/people/Credential/{credentialId}/revoke")]
    Task<ApiResponse<CredentialRevokeResponse>> RevokeCredential(Guid tenantId, Guid credentialId, [Body] CredentialRevokeRequest request);

    /// <summary>Machine-client only: deletes credentials that expired before <paramref name="expiryCutoff"/> (housekeeping endpoint).</summary>
    [Delete("/api/tenants/{tenantId}/people/Credential")]
    Task<IApiResponse> DeleteExpiredCredentials(Guid tenantId, [Query(Format = "o")] DateTimeOffset expiryCutoff);

    // ── Credential types ──────────────────────────────────────────────────────

    /// <summary>Gets a page of credential types (v2, Gridify). Returns 204 when empty.</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/CredentialTypes")]
    Task<ApiResponse<PagingResponse<CredentialType>>> GetCredentialTypes(Guid tenantId, [Query] GridifyQuery? gridifyQuery = null);

    // ── Credential providers ──────────────────────────────────────────────────

    /// <summary>Gets the credential providers on a company node (Sieve filters/sorts supported; requires PeopleAdmin).</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/credentialprovider")]
    Task<ApiResponse<List<CredentialProviderResponse>>> GetCredentialProviders(Guid tenantId, int nodeId, [Query] SieveQueryParams? query = null);

    /// <summary>Gets a credential provider by id. Returns 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/credentialprovider/{id}")]
    Task<ApiResponse<CredentialProviderResponse>> GetCredentialProvider(Guid tenantId, int nodeId, Guid id);

    /// <summary>Creates a credential provider on a company node. Returns 201.</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/credentialprovider")]
    Task<CredentialProviderResponse> CreateCredentialProvider(Guid tenantId, int nodeId, [Body] CredentialProviderRequest provider);

    /// <summary>Updates a credential provider. Returns 404 when not found.</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/credentialprovider/{id}")]
    Task<ApiResponse<CredentialProviderResponse>> UpdateCredentialProvider(Guid tenantId, int nodeId, Guid id, [Body] CredentialProviderRequest provider);

    /// <summary>Deletes a credential provider. Returns 204, or 404 when not found.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/credentialprovider/{id}")]
    Task<IApiResponse> DeleteCredentialProvider(Guid tenantId, int nodeId, Guid id);
}

/// <summary>Body for issuing a credential (server <c>CredentialIssueCommand</c>). NodeId is used for the permission check.</summary>
public record CredentialIssueRequest(Guid PersonId, int NodeId, string CredentialData, Guid CredentialProviderId,
    Guid DeviceId, DateTimeOffset ExpiresOn, string? BioFilterTags = null);

/// <summary>Body for revoking a credential (server <c>CredentialRevokeCommand</c>). NodeId is used for the permission check.</summary>
public record CredentialRevokeRequest(Guid PersonId, int NodeId, string? Reason = null);

/// <summary>Response of a credential revoke.</summary>
public record CredentialRevokeResponse(string Message, Guid CredentialId);

/// <summary>A credential provider (server <c>CredentialProviderDto</c>).</summary>
public record CredentialProviderResponse(Guid Id, Guid TenantId, Guid CompanyId, int NodeId, string Name, string Endpoint, string? ApiKey);

/// <summary>Body for creating (server <c>CredentialProviderAddCommand</c>) or updating (server <c>CredentialProviderUpdateCommand</c>) a credential provider. CompanyId is only used on create.</summary>
public record CredentialProviderRequest(string Name, string Endpoint, string? ApiKey = null, Guid CompanyId = default);
