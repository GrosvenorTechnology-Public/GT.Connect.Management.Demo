using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.Organisation;

/// <summary>
/// Organisation service endpoints (organisation, nodes, users, roles, permissions, security targets and user groups).
/// The core endpoints live here; the other areas are in the inherited sub-interfaces.
/// </summary>
public interface IConnectApiOrganisation :
    IConnectApiOrganisationNodes,
    IConnectApiOrganisationUsers,
    IConnectApiOrganisationRoles,
    IConnectApiOrganisationSecurity,
    IConnectApiOrganisationUserGroups
{
    [Get("/api/tenants/{tenantId}/org/organisations")]
    Task<Organisation> GetOrganisation(Guid tenantId);

    [Obsolete("This endpoint is deprecated, please use GetNodeV2 instead.")]
    [Get("/api/tenants/{tenantId}/org/nodes/{nodeId}")]
    Task<List<NodeResponse>> GetNode(Guid tenantId, int nodeId);

    [Obsolete("This endpoint is deprecated, please use GetNodeV2 instead.")]
    [Get("/api/tenants/{tenantId}/org/nodes/{nodeId}?walkdown=false&onlyAdjacent=false")]
    Task<NodeResponse> GetNodeOnly(Guid tenantId, int nodeId);

    /// <summary>Gets a single node (v2). The Gridify query is not used by the server.</summary>
    [Get("/api/tenants/{tenantId}/org/nodes/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<NodeResponse> GetNodeV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets the node and its descendants down to <paramref name="nodeDepth"/> levels (0/omitted = the node only, 1 = immediate children).</summary>
    [Get("/api/tenants/{tenantId}/org/nodes/{nodeId}/children")]
    [Headers("X-Api-Version: 2.0")]
    Task<List<NodeResponse>> GetChildNodesV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null, [Query] int? nodeDepth = null);

    /// <summary>Gets the node and all its visible ancestors (v2). 404 when nothing is visible.</summary>
    [Get("/api/tenants/{tenantId}/org/nodes/{nodeId}/parents")]
    [Headers("X-Api-Version: 2.0")]
    Task<List<NodeResponse>> GetParentNodesV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists all nodes visible to the current user, starting at their home node (v1, Sieve).</summary>
    [Get("/api/tenants/{tenantId}/org/nodes")]
    Task<List<NodeResponse>> GetNodes(Guid tenantId, [Query] SieveQueryParams? query = null);

    /// <summary>Gets the node and all of its descendants (v1 walkDown).</summary>
    [Obsolete("This endpoint is deprecated, please use GetNodesV2/GetChildNodesV2/GetParentNodesV2 instead.")]
    [Get("/api/tenants/{tenantId}/org/nodes/{nodeId}?walkdown=true&onlyAdjacent=false")]
    Task<List<NodeResponse>> GetNodeAndAllChildren(Guid tenantId, int nodeId);

    /// <summary>Gets only the immediate children of the node (v1 onlyAdjacent).</summary>
    [Obsolete("This endpoint is deprecated, please use GetChildNodesV2 with nodeDepth: 1 instead (note v2 also returns the requested node).")]
    [Get("/api/tenants/{tenantId}/org/nodes/{nodeId}?walkdown=false&onlyAdjacent=true")]
    Task<List<NodeResponse>> GetNodeAdjacentNodes(Guid tenantId, int nodeId);

    [Post("/api/tenants/{tenantId}/org/nodes")]
    Task<NodeResponse> AddNode(Guid tenantId, [Body] AddNodeCommand command);

    /// <summary>Updates a node. Name, retention periods and set additional fields are applied; see <see cref="UpdateNodeCommand"/>.</summary>
    [Put("/api/tenants/{tenantId}/org/nodes/{nodeId}")]
    Task<NodeResponse> UpdateNode(Guid tenantId, int nodeId, [Body] UpdateNodeCommand command);

    /// <summary>Creates a system user. <paramref name="nodeId"/> is not used by the server (it is sent as an ignored query value); the node comes from the body.</summary>
    [Post("/api/tenants/{tenantId}/org/users")]
    Task<UserResponse> CreateUser(Guid tenantId, int nodeId, [Body] CreateUserRequest command);

    /// <summary>Attaches an existing identity to the tenant as a system user. <paramref name="nodeId"/> is not used by the server; the node comes from the body.</summary>
    [Post("/api/tenants/{tenantId}/org/users/attach")]
    Task<UserResponse> AttachUser(Guid tenantId, int nodeId, [Body] AttachUserRequest command);

    /// <summary>Invites a system user. <paramref name="nodeId"/> is not used by the server; the node comes from the body.</summary>
    [Post("/api/tenants/{tenantId}/org/users/invite")]
    Task<UserResponse> InviteUser(Guid tenantId, int nodeId, [Body] InviteUserRequest command);

    [Obsolete("This route does not exist on the server. Accepting an invite is the non-tenant anonymous endpoint POST /api/org/users/acceptinvite, use IConnectApiAuth.AcceptUserInvite instead.")]
    [Post("/api/tenants/{tenantId}/org/users/acceptinvite")]
    Task<UserResponse> AcceptInvite(Guid tenantId, int nodeId, [Body] AcceptInviteRequest command);

    [Obsolete("This endpoint is deprecated on the server, please use GetUserGroupsOnNodeV2 instead.")]
    [Get("/api/tenants/{tenantId}/org/userGroups/node/{nodeId}")]
    Task<List<UserGroupResponse>> GetUserGroupsOnNode(Guid tenantId, int nodeId);

    [Post("/api/tenants/{tenantId}/org/userGroupMembers")]
    Task<UserToUserGroupResponse> AddUserToUserGroup(Guid tenantId, [Body] AddUserToUserGroupRequest command);

    [Obsolete("This endpoint is deprecated on the server, please use GetUsersV2 instead.")]
    [Get("/api/tenants/{tenantId}/org/users")]
    Task<List<UserResponse>> GetUsers(Guid tenantId);

    [Obsolete("This endpoint is deprecated on the server, please use GetRolesV2 instead.")]
    [Get("/api/tenants/{tenantId}/org/roles")]
    Task<List<RoleResponse>> GetRoles(Guid tenantId);

    /// <summary>Creates a security target (assigns a role to a user or user group on a node). The body TenantId must match the route.</summary>
    [Post("/api/tenants/{tenantId}/org/security")]
    Task<SecurityTargetResponse> AddSecurityTarget(Guid tenantId, [Body] AddSecurityTargetRequest command);

    /// <summary>Lists the tenant ids of all organisations (root/super tenant only).</summary>
    [Get("/api/tenants/{tenantId}/org/organisations/GetTenantIds")]
    Task<List<Guid>> GetOrganisationTenantIds(Guid tenantId);

    /// <summary>Lists all organisations (root/super tenant only).</summary>
    [Get("/api/tenants/{tenantId}/org/organisations/list")]
    Task<List<Organisation>> GetAllOrganisations(Guid tenantId);
}


public enum NodeType
{
    Partner,
    ChannelPartner,
    Company,
    Structural,
    Unknown
}

public record CreateUserRequest(Guid TenantId, int NodeId, string Title, string Firstname, string Surname, string Email, string Password,
    bool IsEnterpriseUser = false, Guid? UserId = null);
public record AttachUserRequest(Guid TenantId, int NodeId, string Title, string Firstname, string Surname, string Email);
public record InviteUserRequest(Guid TenantId, int NodeId, string Title, string Firstname, string Surname, string Email, string Password);
public record AcceptInviteRequest(string Email);

public record UserResponse(Guid TenantId, int NodeId, string Title, string Firstname, string Surname, string Email,
    bool IsActive, string NodeType, string NodeName, bool MfaEnabled,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn,
    List<UserGroupSummary>? UserGroups = null)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

/// <summary>User group membership embedded in <see cref="UserResponse.UserGroups"/> (only populated by GetUsersOnNodeV2).</summary>
public record UserGroupSummary(Guid Id, Guid TenantId, int NodeId, string Name, bool IsSystem);

public record UserGroupResponse(Guid TenantId, int NodeId, Guid Id, string Name, bool IsSystem, string? NodeType, string? NodeName,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

public record AddUserToUserGroupRequest(Guid TenantId, Guid UserId, Guid UserGroupId);

public record UserToUserGroupResponse(Guid TenantId, Guid SystemUserId, Guid UserGroupId,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseObject(CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

public record RoleResponse(Guid TenantId, Guid Id, string Name, bool IsSystem,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject<Guid>(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

public record AddSecurityTargetRequest(Guid TenantId, int NodeId, Guid SystemRoleId, Guid? UserGroupId, Guid? SystemUserId);

[Obsolete("The server returns a single SecurityTargetResponse from POST /org/security; this type is no longer used.")]
public record AddSecurityTargetResponse(Guid TenantId, int NodeId, Guid SystemRoleId, Guid? UserGroupId, Guid? SystemUserId);

public record Organisation(Guid TenantId, Guid Id, string Name, int PartnerNodeId,
    int? DeviceTimeToOfflineInSeconds,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject<Guid>(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);
