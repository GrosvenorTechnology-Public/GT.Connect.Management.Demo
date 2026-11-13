using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using GT.Connect.Management.Demo.ConnectApi.DeviceConfig;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>People endpoints for clock groups, their members (people) and their devices.</summary>
public interface IConnectApiPeopleClockGroups
{
    // ---- Clock groups ----

    /// <summary>Lists the clock groups on a company node (v1, unpaged). Returns 204 when there are none.</summary>
    [Obsolete("The v1 endpoint is deprecated on the server, use GetClockGroupsV2.")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/clockGroups")]
    Task<ApiResponse<List<ClockGroupResponse>>> GetClockGroups(Guid tenantId, int nodeId);

    /// <summary>Lists the clock groups on a company node (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/clockGroups")]
    Task<ApiResponse<PagingResponse<ClockGroupResponse>>> GetClockGroupsV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists the clock groups on a company node with their device and people counts (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/clockGroups/withcounts")]
    Task<ApiResponse<PagingResponse<ClockGroupWithCountsResponse>>> GetClockGroupsWithCounts(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a clock group by id. Returns 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/clockGroups/{clockGroupId}")]
    Task<ApiResponse<ClockGroupResponse>> GetClockGroupById(Guid tenantId, int nodeId, Guid clockGroupId);

    /// <summary>Gets a clock group by external id. Returns 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/clockGroups/externalId/{externalId}")]
    Task<ApiResponse<ClockGroupResponse>> GetClockGroupByExternalId(Guid tenantId, int nodeId, string externalId);

    /// <summary>Creates a clock group (201). Body TenantId and NodeId must match the route.</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/clockGroups")]
    Task<ClockGroupResponse> AddClockGroup(Guid tenantId, int nodeId, [Body] ClockGroupRequest person);

    /// <summary>Updates a clock group. Body Id must match <paramref name="id"/>; body Tags replace the stored tags.</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/clockGroups/{id}")]
    Task<ClockGroupResponse> UpdateClockGroup(Guid tenantId, int nodeId, Guid id, [Body] ClockGroupUpdateRequest clockGroup);

    /// <summary>Deletes a clock group.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/clockGroups/{id}")]
    Task<bool> DeleteClockGroup(Guid tenantId, int nodeId, Guid id);

    /// <summary>Force-deletes a clock group, removing all its device and person mappings first (machine client only).</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/clockGroups/{clockGroupId}/force")]
    Task<bool> ForceDeleteClockGroup(Guid tenantId, int nodeId, Guid clockGroupId);

    /// <summary>Lists devices on the node that are not in the clock group (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/clockGroups/{clockGroupId}/notDevices")]
    Task<ApiResponse<PagingResponse<Device>>> GetDevicesNotInClockGroup(Guid tenantId, int nodeId, Guid clockGroupId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists people on the node that are not in the clock group (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/clockGroups/{clockGroupId}/notMembers")]
    Task<ApiResponse<PagingResponse<PersonResponse>>> GetPeopleNotInClockGroup(Guid tenantId, int nodeId, Guid clockGroupId, [Query] GridifyQuery? gridifyQuery = null);

    // ---- Clock groups: machine-to-machine (/internal/) routes, used by the Integration Edge ----

    /// <summary>Lists the clock groups on a company node via the machine-client route (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/internal/clockGroups")]
    Task<ApiResponse<PagingResponse<ClockGroupResponse>>> GetClockGroupsInternal(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Creates a clock group via the machine-client route (201).</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/internal/clockGroups")]
    Task<ClockGroupResponse> AddClockGroupInternal(Guid tenantId, int nodeId, [Body] ClockGroupRequest clockGroup);

    /// <summary>Updates a clock group via the machine-client route. Body Id must match <paramref name="id"/>.</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/internal/clockGroups/{id}")]
    Task<ClockGroupResponse> UpdateClockGroupInternal(Guid tenantId, int nodeId, Guid id, [Body] ClockGroupUpdateRequest clockGroup);

    /// <summary>Deletes a clock group via the machine-client route.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/internal/clockGroups/{clockGroupId}")]
    Task<bool> DeleteClockGroupInternal(Guid tenantId, int nodeId, Guid clockGroupId);

    // ---- Clock group members (people) ----

    /// <summary>Lists the people in a clock group (v1, unpaged). Returns 204 when empty.</summary>
    [Obsolete("The v1 endpoint is deprecated on the server, use GetPeopleInGroupV2.")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/clockGroup/{clockGroupId}/members")]
    Task<ApiResponse<PersonResponse[]>> GetPeopleInGroup(Guid tenantId, int nodeId, Guid clockGroupId);

    /// <summary>Lists the people in a clock group (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/clockGroup/{clockGroupId}/members")]
    Task<ApiResponse<PagingResponse<PersonResponse>>> GetPeopleInGroupV2(Guid tenantId, int nodeId, Guid clockGroupId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Adds a person to a clock group (201, returns the link).</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/clockGroup/{clockGroupId}/members/{personId}")]
    Task<ApiResponse<PersonToClockGroupResponse>> AddPersonToClockGroup(Guid tenantId, int nodeId, Guid clockGroupId, Guid personId);

    /// <summary>Adds several people to a clock group, returning one link per person.</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/clockGroup/{clockGroupId}/members")]
    Task<List<PersonToClockGroupResponse>> AddPeopleToClockGroup(Guid tenantId, int nodeId, Guid clockGroupId, [Body] List<Guid> personIds);

    /// <summary>Removes a person from a clock group (the server returns a bool).</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/clockGroup/{clockGroupId}/members/{personId}")]
    Task RemovePersonFromClockGroup(Guid tenantId, int nodeId, Guid clockGroupId, Guid personId);

    // ---- Clock group devices ----

    /// <summary>Adds a device to a clock group.</summary>
    /// <remarks>The server actually returns the device, not a clock group; only id/name fields of the result are meaningful.</remarks>
    [Obsolete("The server returns the device, not a clock group. Use LinkDeviceToClockGroup.")]
    [Post("/api/tenants/{tenantId}/people/{nodeId}/clockGroup/{clockGroupId}/devices/{deviceId}")]
    Task<ApiResponse<ClockGroupResponse>> AddDeviceToClockGroup(Guid tenantId, int nodeId, Guid clockGroupId, Guid deviceId);

    /// <summary>Adds a device to a clock group (201, returns the device).</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/clockGroup/{clockGroupId}/devices/{deviceId}")]
    Task<ApiResponse<Device>> LinkDeviceToClockGroup(Guid tenantId, int nodeId, Guid clockGroupId, Guid deviceId);

    /// <summary>Adds several devices to a clock group, returning the devices.</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/clockGroup/{clockGroupId}/devices")]
    Task<List<Device>> AddDevicesToClockGroup(Guid tenantId, int nodeId, Guid clockGroupId, [Body] List<Guid> deviceIds);

    /// <summary>Removes a device from a clock group.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/clockGroup/{clockGroupId}/devices/{deviceId}")]
    Task<bool> RemoveDeviceFromClockGroup(Guid tenantId, int nodeId, Guid clockGroupId, Guid deviceId);

    /// <summary>Lists the devices in a clock group (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/clockGroup/{clockGroupId}/devices")]
    Task<ApiResponse<PagingResponse<Device>>> GetDevicesInClockGroup(Guid tenantId, int nodeId, Guid clockGroupId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists the clock groups a device belongs to (v2, Gridify paged).</summary>
    /// <remarks>The server route is <c>clockGroup/{clockGroupId}/Devices/{deviceId}</c> but ignores the clock group id, so an empty GUID is sent.</remarks>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/clockGroup/00000000-0000-0000-0000-000000000000/devices/{deviceId}")]
    Task<ApiResponse<PagingResponse<ClockGroupResponse>>> GetClockGroupsForDevice(Guid tenantId, int nodeId, Guid deviceId, [Query] GridifyQuery? gridifyQuery = null);
}
