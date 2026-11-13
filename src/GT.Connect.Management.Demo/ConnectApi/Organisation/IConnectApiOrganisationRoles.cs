using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.Organisation;

/// <summary>
/// System role, role privilege and permission endpoints.
/// The "OnNode" variants are for callers whose permissions are defined on that node rather than their home node.
/// </summary>
public interface IConnectApiOrganisationRoles
{
    /// <summary>Lists system roles (v2, Gridify paged).</summary>
    [Get("/api/tenants/{tenantId}/org/roles")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<RoleResponse>> GetRolesV2(Guid tenantId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists system roles (v2, Gridify paged). The server does not filter by <paramref name="nodeId"/>.</summary>
    [Get("/api/tenants/{tenantId}/org/roles/node/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<RoleResponse>> GetRolesOnNodeV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a system role. 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/org/roles/{roleId}")]
    Task<ApiResponse<RoleResponse>> GetRole(Guid tenantId, Guid roleId);

    /// <summary>Gets a system role, checking permissions on <paramref name="nodeId"/>. 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/org/roles/node/{nodeId}/{roleId}")]
    Task<ApiResponse<RoleResponse>> GetRoleOnNode(Guid tenantId, int nodeId, Guid roleId);

    /// <summary>Creates a system role.</summary>
    [Post("/api/tenants/{tenantId}/org/roles")]
    Task<RoleResponse> CreateRole(Guid tenantId, [Body] AddRoleRequest command);

    /// <summary>Creates a system role, checking permissions on <paramref name="nodeId"/>.</summary>
    [Post("/api/tenants/{tenantId}/org/roles/node/{nodeId}")]
    Task<RoleResponse> CreateRoleOnNode(Guid tenantId, int nodeId, [Body] AddRoleRequest command);

    /// <summary>Deletes a system role. 204 on success, 400 when it could not be removed.</summary>
    [Delete("/api/tenants/{tenantId}/org/roles/{roleId}")]
    Task<IApiResponse> DeleteRole(Guid tenantId, Guid roleId);

    /// <summary>Deletes a system role, checking permissions on <paramref name="nodeId"/>. 204 on success, 400 when it could not be removed.</summary>
    [Delete("/api/tenants/{tenantId}/org/roles/node/{nodeId}/{roleId}")]
    Task<IApiResponse> DeleteRoleOnNode(Guid tenantId, int nodeId, Guid roleId);

    /// <summary>Adds a permission (privilege) to a system role.</summary>
    [Post("/api/tenants/{tenantId}/org/roles/{roleId}/privilege")]
    Task<PrivilegeResponse> AssignRolePrivilege(Guid tenantId, Guid roleId, [Body] AssignPermissionRequest command);

    /// <summary>Adds a permission (privilege) to a system role, checking permissions on <paramref name="nodeId"/>.</summary>
    [Post("/api/tenants/{tenantId}/org/roles/node/{nodeId}/{roleId}/privilege")]
    Task<PrivilegeResponse> AssignRolePrivilegeOnNode(Guid tenantId, int nodeId, Guid roleId, [Body] AssignPermissionRequest command);

    /// <summary>Removes a permission (privilege) from a system role. 204 on success, 404 when not found.</summary>
    [Delete("/api/tenants/{tenantId}/org/roles/{roleId}/privilege/{permissionId}")]
    Task<IApiResponse> RemoveRolePrivilege(Guid tenantId, Guid roleId, Guid permissionId);

    /// <summary>Removes a permission (privilege) from a system role, checking permissions on <paramref name="nodeId"/>. 204 on success, 404 when not found.</summary>
    [Delete("/api/tenants/{tenantId}/org/roles/node/{nodeId}/{roleId}/privilege/{permissionId}")]
    Task<IApiResponse> RemoveRolePrivilegeOnNode(Guid tenantId, int nodeId, Guid roleId, Guid permissionId);

    /// <summary>Lists the permissions held by the calling user (introspection). 204 when there are none.</summary>
    [Get("/api/tenants/{tenantId}/org/permissions/introspection")]
    Task<ApiResponse<List<PermissionResponse>>> GetCurrentUserPermissions(Guid tenantId);

    /// <summary>Lists all permissions (v2, Gridify paged).</summary>
    [Get("/api/tenants/{tenantId}/org/permissions")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<PermissionResponse>> GetPermissionsV2(Guid tenantId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists all permissions (v2, Gridify paged). The server does not filter by <paramref name="nodeId"/>.</summary>
    [Get("/api/tenants/{tenantId}/org/permissions/node/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<PermissionResponse>> GetPermissionsOnNodeV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a permission. 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/org/permissions/{permissionId}")]
    Task<ApiResponse<PermissionResponse>> GetPermission(Guid tenantId, Guid permissionId);

    /// <summary>Lists the permissions granted by a role (v2). 404 when the role is missing; 200 with an EMPTY body (Content null) when it has none.</summary>
    [Get("/api/tenants/{tenantId}/org/permissions/role/{roleId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<List<PermissionResponse>>> GetRolePermissionsV2(Guid tenantId, Guid roleId);

    /// <summary>Lists the permissions granted by a role, checking permissions on <paramref name="nodeId"/> (v2). Same empty-body behaviour as GetRolePermissionsV2.</summary>
    [Get("/api/tenants/{tenantId}/org/permissions/node/{nodeId}/role/{roleId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<List<PermissionResponse>>> GetRolePermissionsOnNodeV2(Guid tenantId, int nodeId, Guid roleId);

    /// <summary>Creates a permission.</summary>
    [Post("/api/tenants/{tenantId}/org/permissions")]
    Task<PermissionResponse> CreatePermission(Guid tenantId, [Body] AddPermissionRequest command);

    /// <summary>Deletes a permission. 204 on success, 404 when not found.</summary>
    [Delete("/api/tenants/{tenantId}/org/permissions/{permissionId}")]
    Task<IApiResponse> DeletePermission(Guid tenantId, Guid permissionId);
}

public record AddRoleRequest(Guid TenantId, string Name, bool IsSystem = false);

public record AssignPermissionRequest(Guid PermissionId);

public record PrivilegeResponse(Guid TenantId, Guid SystemRoleId, Guid PermissionId,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseObject(CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

public record PermissionResponse(Guid Id, string Name);

public record AddPermissionRequest(Guid Id, string Name);
