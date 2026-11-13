using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.Organisation;

/// <summary>
/// System user endpoints (users of the GtConnect portal/API, not people/employees) and Entra ID (Graph) guest lookups.
/// The "OnNode" variants are for callers whose permissions are defined on that node rather than their home node.
/// </summary>
public interface IConnectApiOrganisationUsers
{
    /// <summary>Lists system users in the tenant, excluding machine clients (v2, Gridify paged).</summary>
    [Get("/api/tenants/{tenantId}/org/users")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<UserResponse>> GetUsersV2(Guid tenantId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists system users (v2, Gridify paged) with <see cref="UserResponse.UserGroups"/> populated. The server does not filter by <paramref name="nodeId"/>.</summary>
    [Get("/api/tenants/{tenantId}/org/users/node/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<UserResponse>> GetUsersOnNodeV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets the system user for the calling token. 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/org/users/current")]
    Task<ApiResponse<UserResponse>> GetCurrentUser(Guid tenantId);

    /// <summary>Gets a system user by id. 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/org/users/{userId}")]
    Task<ApiResponse<UserResponse>> GetUser(Guid tenantId, Guid userId);

    /// <summary>Gets a system user by id, checking permissions on <paramref name="nodeId"/>. 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/org/users/node/{nodeId}/{userId}")]
    Task<ApiResponse<UserResponse>> GetUserOnNode(Guid tenantId, int nodeId, Guid userId);

    /// <summary>Gets a system user by email address. 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/org/users/email")]
    Task<ApiResponse<UserResponse>> GetUserByEmail(Guid tenantId, [Query] string email);

    /// <summary>Updates a system user. The body Id must match <paramref name="userId"/>.</summary>
    [Put("/api/tenants/{tenantId}/org/users/{userId}")]
    Task<UserResponse> UpdateUser(Guid tenantId, Guid userId, [Body] UpdateUserRequest command);

    /// <summary>Updates a system user, checking permissions on <paramref name="nodeId"/>. The body Id must match <paramref name="userId"/>.</summary>
    [Put("/api/tenants/{tenantId}/org/users/node/{nodeId}/{userId}")]
    Task<UserResponse> UpdateUserOnNode(Guid tenantId, int nodeId, Guid userId, [Body] UpdateUserRequest command);

    /// <summary>Changes a user's password. Only allowed for the user's own token (403 otherwise, e.g. for machine clients). 204 on success.</summary>
    [Put("/api/tenants/{tenantId}/org/users/{userId}/password")]
    Task<IApiResponse> ChangeUserPassword(Guid tenantId, Guid userId, [Body] ChangeUserPasswordRequest command);

    /// <summary>Removes a system user from the tenant. 204 on success.</summary>
    [Delete("/api/tenants/{tenantId}/org/users/{userId}")]
    Task<IApiResponse> DeleteUser(Guid tenantId, Guid userId);

    /// <summary>Removes a system user from the tenant, checking permissions on <paramref name="nodeId"/>. 204 on success.</summary>
    [Delete("/api/tenants/{tenantId}/org/users/node/{nodeId}/{userId}")]
    Task<IApiResponse> DeleteUserOnNode(Guid tenantId, int nodeId, Guid userId);

    /// <summary>Writes the GtConnect user id into the Auth0 user's app_metadata. Returns the server's success flag.</summary>
    [Post("/api/tenants/{tenantId}/org/users/{userId}/uuid")]
    Task<bool> AddUserUuidToAuth0Metadata(Guid tenantId, Guid userId, [Body] AddUserUuidToAuth0MetadataRequest command);

    /// <summary>Gets the system user id of the tenant's machine client (machine client only). Plain-text body.</summary>
    [Get("/api/tenants/{tenantId}/org/users/machine")]
    Task<string> GetMachineClientUserId(Guid tenantId);

    /// <summary>Lists every system user including machine clients (machine client only); refreshes the server's user cache. 204 when empty.</summary>
    [Get("/api/tenants/{tenantId}/org/users/inclusive")]
    Task<ApiResponse<List<UserResponse>>> GetUsersIncludingMachineClients(Guid tenantId);

    /// <summary>Gets a system user including machine clients (machine client only); refreshes the server's user cache. 204 when not found.</summary>
    [Get("/api/tenants/{tenantId}/org/users/singular/{userId}")]
    Task<ApiResponse<UserResponse>> GetUserIncludingMachineClients(Guid tenantId, Guid userId);

    /// <summary>Looks up an active Entra ID (Microsoft Graph) guest user by email.</summary>
    [Get("/api/tenants/{tenantId}/org/graph/activeGuest")]
    Task<GraphUserResponse> GetActiveGraphGuestUser(Guid tenantId, [Query] int nodeId, [Query] string email);

    /// <summary>Lists all active Entra ID (Microsoft Graph) guest users in the domain.</summary>
    [Get("/api/tenants/{tenantId}/org/graph/activeGuests")]
    Task<List<GraphUserResponse>> GetActiveGraphGuestUsers(Guid tenantId, [Query] int nodeId);
}

public record UpdateUserRequest(Guid TenantId, Guid Id, string Title, string Firstname, string Surname, string Email,
    int NodeId, bool IsActive, bool MfaEnabled);

public record ChangeUserPasswordRequest(Guid TenantId, Guid Id, string CurrentPassword, string Password);

public record AddUserUuidToAuth0MetadataRequest(Guid TenantId, string Email, Guid UserId);

public record GraphUserResponse(Guid TenantId, int NodeId, Guid Id, string? DisplayName, string? FirstName, string? Surname,
    string? Email, string? UserPrincipalName);
