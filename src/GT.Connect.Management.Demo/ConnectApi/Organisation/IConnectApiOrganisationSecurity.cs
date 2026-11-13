using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.Organisation;

/// <summary>
/// Security target endpoints (role assignments of users/user groups on nodes) and permission checks.
/// The "OnNode" variants are for callers whose permissions are defined on that node rather than their home node.
/// </summary>
public interface IConnectApiOrganisationSecurity
{
    /// <summary>Lists security targets with their permissions, excluding the machine client's (v2, Gridify paged).</summary>
    [Get("/api/tenants/{tenantId}/org/security/list")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<SecurityTargetPermissionsResponse>> GetSecurityTargetsV2(Guid tenantId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists security targets defined on <paramref name="nodeId"/> with their permissions (v2, Gridify paged).</summary>
    [Get("/api/tenants/{tenantId}/org/security/node/{nodeId}/list")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<SecurityTargetPermissionsResponse>> GetSecurityTargetsOnNodeV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a security target with its permissions. 404 when not found, 400 when it has no role.</summary>
    [Get("/api/tenants/{tenantId}/org/security/{securityTargetId}")]
    Task<ApiResponse<SecurityTargetPermissionsResponse>> GetSecurityTarget(Guid tenantId, Guid securityTargetId);

    /// <summary>Creates a security target, checking permissions on <paramref name="nodeId"/>.</summary>
    [Post("/api/tenants/{tenantId}/org/security/node/{nodeId}")]
    Task<SecurityTargetResponse> AddSecurityTargetOnNode(Guid tenantId, int nodeId, [Body] AddSecurityTargetRequest command);

    /// <summary>Deletes a security target. 204 on success, 400 on failure.</summary>
    [Delete("/api/tenants/{tenantId}/org/security/{securityTargetId}")]
    Task<IApiResponse> DeleteSecurityTarget(Guid tenantId, Guid securityTargetId);

    /// <summary>Deletes a security target, checking permissions on <paramref name="nodeId"/>. 204 on success, 400 on failure.</summary>
    [Delete("/api/tenants/{tenantId}/org/security/node/{nodeId}/{securityTargetId}")]
    Task<IApiResponse> DeleteSecurityTargetOnNode(Guid tenantId, int nodeId, Guid securityTargetId);

    /// <summary>Checks whether a user holds <paramref name="permissionName"/> on a node (anonymous endpoint).</summary>
    [Get("/api/tenants/{tenantId}/org/security/check")]
    Task<bool> CheckSecurityPermission(Guid tenantId, [Query] int nodeId, [Query] Guid userId, [Query] string permissionName);

    /// <summary>Checks whether a user holds <paramref name="permissionName"/> on ALL of the given nodes (false for an empty list; anonymous endpoint).</summary>
    [Post("/api/tenants/{tenantId}/org/security/check/list")]
    Task<bool> CheckSecurityPermissionForNodes(Guid tenantId, [Query] Guid userId, [Query] string permissionName, [Body] List<int> nodeIds);

    /// <summary>Returns the subset of the given nodes on which a user holds <paramref name="permissionName"/> (anonymous endpoint).</summary>
    [Post("/api/tenants/{tenantId}/org/security/check/list/filter")]
    Task<List<int>> FilterNodesBySecurityPermission(Guid tenantId, [Query] Guid userId, [Query] string permissionName, [Body] List<int> nodeIds);
}

public record SecurityTargetResponse(Guid TenantId, int NodeId, bool IsSystem, Guid? SystemRoleId, Guid? UserGroupId, Guid? SystemUserId,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

public record SecurityTargetPermissionsResponse(Guid TenantId, int NodeId, bool IsSystem, Guid? SystemRoleId, Guid? UserGroupId, Guid? SystemUserId,
    List<PermissionResponse> Permissions,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);
