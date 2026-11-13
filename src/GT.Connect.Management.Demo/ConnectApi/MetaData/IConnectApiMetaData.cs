using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.MetaData;

/// <summary>
/// MetaData service endpoints: the per-node custom field labels (system settings "Fields").
/// </summary>
public interface IConnectApiMetaData
{
    /// <summary>Gets the custom field labels for a node. 204 when none are defined.</summary>
    [Get("/api/tenants/{tenantId}/meta/systemSettings/fields/{nodeId}")]
    Task<ApiResponse<CustomFieldsResponse>> GetMetaDataCustomFields(Guid tenantId, int nodeId);

    /// <summary>Gets the custom field labels for a node using the machine-client-only route. 204 when none are defined.</summary>
    [Get("/api/tenants/{tenantId}/meta/systemSettings/fields/node/{nodeId}")]
    Task<ApiResponse<CustomFieldsResponse>> GetMetaDataCustomFieldsAsMachineClient(Guid tenantId, int nodeId);

    /// <summary>Creates the custom field labels for the node given in the body.</summary>
    [Post("/api/tenants/{tenantId}/meta/systemSettings/fields")]
    Task<CustomFieldsResponse> AddMetaDataCustomFields(Guid tenantId, [Body] CustomFieldsRequest request);

    /// <summary>Updates a custom field labels record by its id.</summary>
    [Put("/api/tenants/{tenantId}/meta/systemSettings/fields/{customFieldsId}")]
    Task<CustomFieldsResponse> UpdateMetaDataCustomFields(Guid tenantId, Guid customFieldsId, [Body] CustomFieldsRequest request);

    /// <summary>Deletes a custom field labels record by its id. 200 (no body) on success.</summary>
    [Delete("/api/tenants/{tenantId}/meta/systemSettings/fields/{customFieldsId}")]
    Task<IApiResponse> DeleteMetaDataCustomFields(Guid tenantId, Guid customFieldsId);
}

public record CustomFieldsRequest(int NodeId,
    string? AdditionalField1 = null, string? AdditionalField2 = null, string? AdditionalField3 = null, string? AdditionalField4 = null,
    string? AdditionalField5 = null, string? AdditionalField6 = null, string? AdditionalField7 = null, string? AdditionalField8 = null,
    string? AdditionalField9 = null, string? AdditionalField10 = null,
    string? CompanyAdditionalField1 = null, string? CompanyAdditionalField2 = null, string? CompanyAdditionalField3 = null,
    string? CompanyAdditionalField4 = null, string? CompanyAdditionalField5 = null, string? CompanyAdditionalField6 = null,
    string? CompanyAdditionalField7 = null, string? CompanyAdditionalField8 = null, string? CompanyAdditionalField9 = null,
    string? CompanyAdditionalField10 = null);

public record CustomFieldsResponse(int NodeId, Guid TenantId,
    string? AdditionalField1, string? AdditionalField2, string? AdditionalField3, string? AdditionalField4, string? AdditionalField5,
    string? AdditionalField6, string? AdditionalField7, string? AdditionalField8, string? AdditionalField9, string? AdditionalField10,
    string? CompanyAdditionalField1, string? CompanyAdditionalField2, string? CompanyAdditionalField3, string? CompanyAdditionalField4,
    string? CompanyAdditionalField5, string? CompanyAdditionalField6, string? CompanyAdditionalField7, string? CompanyAdditionalField8,
    string? CompanyAdditionalField9, string? CompanyAdditionalField10,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);
