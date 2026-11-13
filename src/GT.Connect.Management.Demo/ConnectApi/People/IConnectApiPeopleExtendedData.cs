using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>People-service extended data, extended data types and extension points.</summary>
public interface IConnectApiPeopleExtendedData
{
    // ── Extended data ─────────────────────────────────────────────────────────

    /// <summary>Gets a page of all extended data items on a company node (v2, Gridify). Returns 204 when empty.</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/extendeddata/data")]
    Task<ApiResponse<PagingResponse<ExtendedDataItem>>> GetAllExtendedDataItems(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets an extended data item by id. Returns 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/extendeddata/data/{extendedDataId}")]
    Task<ApiResponse<ExtendedDataItem>> GetExtendedDataItem(Guid tenantId, int nodeId, Guid extendedDataId);

    /// <summary>Gets all extended data items of a data type (by data type id). Returns 204 when empty.</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/extendeddata/data/datatype/{dataTypeId}")]
    Task<ApiResponse<List<ExtendedDataItem>>> GetExtendedDataItemsByDataTypeId(Guid tenantId, int nodeId, Guid dataTypeId);

    /// <summary>Gets an extended data item by data type name and external id. Returns 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/extendeddata/data/datatype/{dataTypeName}/externalid/{externalId}")]
    Task<ApiResponse<ExtendedDataItem>> GetExtendedDataItemByExternalId(Guid tenantId, int nodeId, string dataTypeName, string externalId);

    /// <summary>Machine-client only: gets the extended data of a type that is distributed to a device. Total count and type details are in the X-GT-TotalItemCount / X-GT-ExtensionPoint / X-GT-ExtendDataTypeName response headers. 404 when the type does not exist.</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/extendeddata/data/device/{deviceId}/datatype/{dataTypeName}")]
    Task<ApiResponse<List<ExtendedDataItem>>> GetExtendedDataForDevice(Guid tenantId, int nodeId, Guid deviceId, string dataTypeName, [Query] int? pageSize = null);

    /// <summary>Machine-client only: as <see cref="GetExtendedDataForDevice"/>, but only items modified after <paramref name="revision"/> (an ISO 8601 date-time-offset string).</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/extendeddata/data/device/{deviceId}/datatype/{dataTypeName}/revision/{revision}")]
    Task<ApiResponse<List<ExtendedDataItem>>> GetExtendedDataForDeviceSinceRevision(Guid tenantId, int nodeId, Guid deviceId, string dataTypeName, string revision, [Query] int? pageSize = null);

    /// <summary>Machine-client only: gets the external ids of the extended data of a type that is distributed to a device. 404 when the type does not exist.</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/extendeddata/data/device/{deviceId}/datatype/{dataTypeName}/ids")]
    Task<ApiResponse<List<string>>> GetExtendedDataIdsForDevice(Guid tenantId, int nodeId, Guid deviceId, string dataTypeName);

    /// <summary>Batch-updates extended data items of a data type, matched by external id. 201 when all succeed, 207 with failures otherwise, 204 for an empty batch.</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/extendeddata/data/datatype/{dataTypeName}")]
    Task<ApiResponse<ExtendedDataResponse>> UpdateExtendedDataItemsByExternalId(Guid tenantId, int nodeId, string dataTypeName, [Body] List<ExtendedDataItemUpdateByExternalIdRequest> data);

    /// <summary>Batch-deletes extended data items of a data type by external id. 200 when all succeed, 207 with failures otherwise.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/extendeddata/data/datatype/{dataTypeName}")]
    Task<ApiResponse<PeopleBatchDeleteResponse>> DeleteExtendedDataItemsByExternalId(Guid tenantId, int nodeId, string dataTypeName, [Body] List<string> externalIds);

    /// <summary>Batch-deletes extended data items by id. 200 when all succeed, 207 with failures otherwise.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/extendeddata/data")]
    Task<ApiResponse<PeopleBatchDeleteResponse>> DeleteExtendedDataItems(Guid tenantId, int nodeId, [Body] List<Guid> ids);

    /// <summary>Gets a page of the schedule extended data connected to a person (v2, Gridify). Returns 204 when empty.</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/schedules/person/{personId}")]
    Task<ApiResponse<PagingResponse<ExtendedDataItem>>> GetSchedulesForPersonV2(Guid tenantId, int nodeId, Guid personId, [Query] GridifyQuery? gridifyQuery = null);

    // ── Extended data types ───────────────────────────────────────────────────

    /// <summary>Gets a page of the tenant's extended data types (v2, Gridify). Returns 204 when empty.</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/extendeddata/datatype")]
    Task<ApiResponse<PagingResponse<ExtendedDataTypeResponse>>> GetExtendedDataTypesV2(Guid tenantId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets an extended data type by id. Returns 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/people/extendeddata/datatype/{id}")]
    Task<ApiResponse<ExtendedDataTypeResponse>> GetExtendedDataType(Guid tenantId, Guid id);

    /// <summary>Creates an extended data type (requires PeopleEdit on the root node).</summary>
    [Post("/api/tenants/{tenantId}/people/extendeddata/datatype")]
    Task<ExtendedDataTypeResponse> CreateExtendedDataType(Guid tenantId, [Body] ExtendedDataTypeRequest dataType);

    /// <summary>Renames an extended data type (only Name is applied).</summary>
    [Put("/api/tenants/{tenantId}/people/extendeddata/datatype/{id}")]
    Task<ExtendedDataTypeResponse> UpdateExtendedDataType(Guid tenantId, Guid id, [Body] ExtendedDataTypeRequest dataType);

    /// <summary>Deletes an extended data type. Returns true on success.</summary>
    [Delete("/api/tenants/{tenantId}/people/extendeddata/datatype/{id}")]
    Task<bool> DeleteExtendedDataType(Guid tenantId, Guid id);

    // ── Extension points ──────────────────────────────────────────────────────

    /// <summary>Gets all extension points. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/people/extendeddata/extensionpoint")]
    Task<ApiResponse<List<ExtensionPointResponse>>> GetExtensionPoints(Guid tenantId);

    /// <summary>Gets an extension point by id. Returns 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/people/extendeddata/extensionpoint/{extensionPointId}")]
    Task<ApiResponse<ExtensionPointResponse>> GetExtensionPoint(Guid tenantId, Guid extensionPointId);

    /// <summary>Gets the extended data types attached to an extension point. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/people/extendeddata/extensionpoint/{extensionPointId}/dataTypes")]
    Task<ApiResponse<List<ExtendedDataTypeResponse>>> GetExtendedDataTypesForExtensionPoint(Guid tenantId, Guid extensionPointId);
}

/// <summary>Body for updating an extended data item by external id (server <c>ExtendedDataUpdateByExternalIdRequest</c>). The server overwrites ConnectedEntityId.</summary>
public record ExtendedDataItemUpdateByExternalIdRequest(string ExternalId, string EntityData, Guid ConnectedEntityId = default);

/// <summary>Body for creating/renaming an extended data type (server <c>ExternalDataTypeRequest</c>).</summary>
public record ExtendedDataTypeRequest(string Name, Guid ExtensionPointId = default);

/// <summary>An extended data extension point (server <c>ExtensionPointDTO</c>).</summary>
public record ExtensionPointResponse(Guid Id, string Name);
