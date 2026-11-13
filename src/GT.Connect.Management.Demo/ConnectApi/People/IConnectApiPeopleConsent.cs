using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>People endpoints for consent agreements, agreement texts, consent links and consent audit.</summary>
public interface IConnectApiPeopleConsent
{
    // ---- Consent agreements ----

    /// <summary>Lists the consent agreements on a company node (v1, unpaged). Returns 204 when there are none.</summary>
    [Obsolete("The v1 endpoint is deprecated on the server, use GetConsentAgreementsOnCompanyNodeV2.")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/consentAgreements")]
    Task<ApiResponse<List<ConsentResponse>>> GetConsentAgreementsOnCompanyNode(Guid tenantId, int nodeId);

    /// <summary>Lists the consent agreements on a company node (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/consentAgreements")]
    Task<ApiResponse<PagingResponse<ConsentResponse>>> GetConsentAgreementsOnCompanyNodeV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists the consent agreements of the company that are available to <paramref name="targetNodeId"/> (v2, Gridify paged). 404 if the caller cannot access the target node.</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/consentAgreements/{targetNodeId}")]
    Task<ApiResponse<PagingResponse<ConsentResponse>>> GetAvailableConsentAgreements(Guid tenantId, int nodeId, int targetNodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a consent agreement by id. Returns 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/consentAgreements/{consentAgreementId}")]
    Task<ApiResponse<ConsentResponse>> GetConsentAgreement(Guid tenantId, int nodeId, Guid consentAgreementId);

    /// <summary>Creates a consent agreement on a company node (201).</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/consentAgreements")]
    Task<ApiResponse<ConsentResponse>> AddConsentAgreementToCompany(Guid tenantId, int nodeId,
        [Body] CreateConsentCommand command);

    /// <summary>Updates a consent agreement (the id is taken from the body).</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/consentAgreements")]
    Task<ApiResponse<ConsentResponse>> UpdateConsentAgreement(Guid tenantId, int nodeId, [Body] UpdateConsentCommand command);

    /// <summary>Deletes a consent agreement.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/consentAgreements/{consentAgreementId}")]
    Task<bool> DeleteConsentAgreement(Guid tenantId, int nodeId, Guid consentAgreementId);

    // ---- Consent agreement texts ----

    /// <summary>Lists the consent agreement texts on a company node (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/consentAgreementText")]
    Task<ApiResponse<PagingResponse<ConsentAgreementText>>> GetConsentAgreementTexts(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a consent agreement text by id (v2). Returns 404 when not found.</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/consentAgreementText/{consentAgreementTextId}")]
    Task<ApiResponse<ConsentAgreementText>> GetConsentAgreementText(Guid tenantId, int nodeId, Guid consentAgreementTextId);

    /// <summary>Adds a text (language variant) to a consent agreement (201).</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/consentAgreementText")]
    Task<ApiResponse<ConsentAgreementText>> AddConsentTextToAgreement(Guid tenantId, int nodeId,
        [Body] CreateConsentAgreementTextCommand command);

    /// <summary>Updates a consent agreement text (the id is taken from the body).</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/consentAgreementText")]
    Task<ApiResponse<ConsentAgreementText>> UpdateConsentAgreementText(Guid tenantId, int nodeId, [Body] UpdateConsentAgreementTextCommand command);

    /// <summary>Deletes a consent agreement text.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/consentAgreementText/{consentAgreementTextId}")]
    Task<bool> DeleteConsentAgreementText(Guid tenantId, int nodeId, Guid consentAgreementTextId);

    // ---- Consent links (activation of an agreement on a node) ----

    /// <summary>Lists the consent links of a company node (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/consentLinks")]
    Task<ApiResponse<PagingResponse<ActivateConsenttResponse>>> GetConsentLinks(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists the company's consent links that apply to <paramref name="targetNodeId"/> (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/consentLinks/{targetNodeId}")]
    Task<ApiResponse<PagingResponse<ActivateConsenttResponse>>> GetConsentLinksOnNode(Guid tenantId, int nodeId, int targetNodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Activates a consent agreement on <c>command.NodeId</c> by creating a consent link; <paramref name="nodeId"/> is the company node. 409 if one already exists for the credential type.</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/consentLinks")]
    Task<ActivateConsenttResponse> ActivateConsentOnCompany(Guid tenantId, int nodeId,
        [Body] ActivateConsentCommand command);

    /// <summary>Removes the consent link <c>command.ConsentLinkId</c> (DELETE consentLinks/{id}).</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/consentLinks/{command.ConsentLinkId}")]
    Task<bool> DeactivateConsentOnCompany(Guid tenantId, int nodeId, DeactivateConsentCommand command);

    // ---- Consent audit ----

    /// <summary>Lists the consent audit entries for a person (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/consentAudit/person/{personId}/consents")]
    Task<ApiResponse<PagingResponse<ConsentAuditResponse>>> GetPersonConsentAudit(Guid tenantId, int nodeId, Guid personId, [Query] GridifyQuery? gridifyQuery = null);
}
