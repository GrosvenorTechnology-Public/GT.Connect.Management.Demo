using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.Organisation;

/// <summary>
/// Organisation node endpoints: v2 node lists, node deletion, business domains and machine-client node snapshots.
/// </summary>
public interface IConnectApiOrganisationNodes
{
    /// <summary>Lists all nodes visible to the current user from their home node (v2, Gridify). Returns a plain array; unordered unless orderBy is set.</summary>
    [Get("/api/tenants/{tenantId}/org/nodes")]
    [Headers("X-Api-Version: 2.0")]
    Task<List<NodeResponse>> GetNodesV2(Guid tenantId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Walks down from each of the given parent nodes to <paramref name="nodeDepth"/> levels (v2, Gridify paged). 400 if a node does not exist.</summary>
    [Post("/api/tenants/{tenantId}/org/nodes/list")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<NodeResponse>> GetChildNodesFromListV2(Guid tenantId, [Body] List<int> parentNodeIds, [Query] int? nodeDepth = null, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Deletes a node. 204 on success, 404 when not found, 405 when the node cannot be deleted.</summary>
    [Delete("/api/tenants/{tenantId}/org/nodes/{nodeId}")]
    Task<IApiResponse> DeleteNode(Guid tenantId, int nodeId);

    /// <summary>Lists the business domains on a node (v1, Sieve).</summary>
    [Get("/api/tenants/{tenantId}/org/nodes/{nodeId}/domain")]
    Task<List<BusinessDomainResponse>> GetNodeBusinessDomains(Guid tenantId, int nodeId, [Query] SieveQueryParams? query = null);

    /// <summary>Lists the business domains on a node (v2, Gridify filtering; plain array, not paged).</summary>
    [Get("/api/tenants/{tenantId}/org/nodes/{nodeId}/domain")]
    [Headers("X-Api-Version: 2.0")]
    Task<List<BusinessDomainResponse>> GetNodeBusinessDomainsV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Adds a business domain to a node. The node is taken from the body.</summary>
    [Post("/api/tenants/{tenantId}/org/nodes/{nodeId}/domain")]
    Task<BusinessDomainResponse> AddNodeBusinessDomain(Guid tenantId, int nodeId, [Body] AddBusinessDomainCommand command);

    /// <summary>Removes a business domain from a node. 204 on success, 404 when not found.</summary>
    [Delete("/api/tenants/{tenantId}/org/nodes/{nodeId}/domain/{businessDomainId}")]
    Task<IApiResponse> DeleteNodeBusinessDomain(Guid tenantId, int nodeId, Guid businessDomainId);

    /// <summary>Gets a minimal snapshot of the whole node tree (machine client only) and rebuilds the server's node-tree cache. 404 when empty.</summary>
    [Get("/api/tenants/{tenantId}/org/nodes/snapshot")]
    Task<ApiResponse<List<NodeSnapshotResponse>>> GetNodeTreeSnapshot(Guid tenantId);

    /// <summary>Gets the name of a deleted node from the server's node-tree cache (machine client only). Plain-text body; 404 when unknown.</summary>
    [Get("/api/tenants/{tenantId}/org/nodes/deletednodename/{nodeId}")]
    Task<ApiResponse<string>> GetDeletedNodeName(Guid tenantId, int nodeId);
}

public record AddBusinessDomainCommand(Guid TenantId, int NodeId, string Name);

public record BusinessDomainResponse(Guid TenantId, int NodeId, string Name,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

public record NodeSnapshotResponse(int Id, int? ParentId, Guid TenantId, string NodeType, string Name,
    int PersonTombstonePeriod, int TransactionRetentionPeriod);
