using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.Organisation;

/// <summary>
/// System user group and user group membership endpoints.
/// The "OnNode" variants are for callers whose permissions are defined on that node rather than their home node.
/// </summary>
public interface IConnectApiOrganisationUserGroups
{
    /// <summary>Lists user groups visible from the calling user's home node (v1, Sieve).</summary>
    [Get("/api/tenants/{tenantId}/org/userGroups")]
    Task<List<UserGroupResponse>> GetUserGroups(Guid tenantId, [Query] SieveQueryParams? query = null);

    /// <summary>Lists user groups visible from the calling user's home node (v2, Gridify paged).</summary>
    [Get("/api/tenants/{tenantId}/org/userGroups")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<UserGroupResponse>> GetUserGroupsV2(Guid tenantId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists the user groups available on <paramref name="nodeId"/> (v2, Gridify paged). 400 if the node is outside the caller's subtree.</summary>
    [Get("/api/tenants/{tenantId}/org/userGroups/node/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<UserGroupResponse>> GetUserGroupsOnNodeV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a user group. 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/org/userGroups/{userGroupId}")]
    Task<ApiResponse<UserGroupResponse>> GetUserGroup(Guid tenantId, Guid userGroupId);

    /// <summary>Gets a user group, checking permissions on <paramref name="nodeId"/>. 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/org/userGroups/node/{nodeId}/{userGroupId}")]
    Task<ApiResponse<UserGroupResponse>> GetUserGroupOnNode(Guid tenantId, int nodeId, Guid userGroupId);

    /// <summary>Creates a user group.</summary>
    [Post("/api/tenants/{tenantId}/org/userGroups")]
    Task<UserGroupResponse> CreateUserGroup(Guid tenantId, [Body] AddUserGroupCommand command);

    /// <summary>Updates a user group. The body Id must match <paramref name="userGroupId"/>.</summary>
    [Put("/api/tenants/{tenantId}/org/userGroups/{userGroupId}")]
    Task<UserGroupResponse> UpdateUserGroup(Guid tenantId, Guid userGroupId, [Body] UpdateUserGroupCommand command);

    /// <summary>Deletes a user group. 204 on success, 404 when not found.</summary>
    [Delete("/api/tenants/{tenantId}/org/userGroups/{userGroupId}")]
    Task<IApiResponse> DeleteUserGroup(Guid tenantId, Guid userGroupId);

    /// <summary>Lists user group memberships visible from the calling user's home node (v2, Gridify paged).</summary>
    [Get("/api/tenants/{tenantId}/org/userGroupMembers/list")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<UserToUserGroupResponse>> GetUserGroupMembersV2(Guid tenantId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists user group memberships (v2, Gridify paged), checking permissions on <paramref name="nodeId"/>. The server still lists from the caller's home node.</summary>
    [Get("/api/tenants/{tenantId}/org/userGroupMembers/node/{nodeId}/list")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<UserToUserGroupResponse>> GetUserGroupMembersOnNodeV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a single user group membership. 404 when the user is not a member.</summary>
    [Get("/api/tenants/{tenantId}/org/userGroupMembers")]
    Task<ApiResponse<UserToUserGroupResponse>> GetUserGroupMember(Guid tenantId, [Query] Guid userId, [Query] Guid userGroupId);

    /// <summary>Gets a single user group membership, scoped to <paramref name="nodeId"/>. 404 when the user is not a member.</summary>
    [Get("/api/tenants/{tenantId}/org/userGroupMembers/node/{nodeId}")]
    Task<ApiResponse<UserToUserGroupResponse>> GetUserGroupMemberOnNode(Guid tenantId, int nodeId, [Query] Guid userId, [Query] Guid userGroupId);

    /// <summary>Adds a user to a user group, checking permissions on <paramref name="nodeId"/>.</summary>
    [Post("/api/tenants/{tenantId}/org/userGroupMembers/node/{nodeId}")]
    Task<UserToUserGroupResponse> AddUserToUserGroupOnNode(Guid tenantId, int nodeId, [Body] AddUserToUserGroupRequest command);

    /// <summary>Removes a user from a user group. 204 on success, 404 when not a member.</summary>
    [Delete("/api/tenants/{tenantId}/org/userGroupMembers")]
    Task<IApiResponse> RemoveUserFromUserGroup(Guid tenantId, [Query] Guid userId, [Query] Guid userGroupId);

    /// <summary>Removes a user from a user group, checking permissions on <paramref name="nodeId"/>. 204 on success, 404 when not a member.</summary>
    [Delete("/api/tenants/{tenantId}/org/userGroupMembers/node/{nodeId}")]
    Task<IApiResponse> RemoveUserFromUserGroupOnNode(Guid tenantId, int nodeId, [Query] Guid userId, [Query] Guid userGroupId);
}

public record UpdateUserGroupCommand(Guid TenantId, Guid Id, string Name, int NodeId, bool IsSystem = false);
