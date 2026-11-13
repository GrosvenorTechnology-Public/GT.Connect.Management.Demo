using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

public interface IConnectApiDeviceConfig : IConnectApiDeviceConfigFiles, IConnectApiDeviceConfigDevices, IConnectApiDeviceConfigScheduling
{
    [Obsolete("This endpoint is deprecated, please use GetDevicesV2 instead")]
    [Get("/api/tenants/{tenantId}/device-config/devices/node/{nodeId}")]
    Task<ApiResponse<List<Device>>> GetDevices(Guid tenantId, int nodeId, [Query] DeviceQueryParams? qparams = null, [Query] bool liveData = true);

    [Get("/api/tenants/{tenantId}/device-config/devices/node/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<Device>>> GetDevicesV2(Guid tenantId, int nodeId, [Query] bool getChildren = false, [Query] bool liveData = false, [Query] GridifyQuery? gridifyModel = null);

    /// <summary>
    /// Gets devices under the given nodes (JSON array body). Nodes the caller lacks device.view on are skipped; 403 when none are permitted.
    /// The server does not support getChildren on this endpoint - the parameter is ignored.
    /// </summary>
    [Post("/api/tenants/{tenantId}/device-config/devices/nodes")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<Device>>> GetDevicesUnderNodesV2(Guid tenantId, [Body] List<int> parentNodeIds, [Query] bool getChildren = false, [Query] bool liveData = false, [Query] GridifyQuery? gridifyQuery = null);

    [Get("/api/tenants/{tenantId}/device-config/devices/{deviceId}")]
    Task<Device> GetDevice(Guid tenantId, Guid deviceId);

    [Get("/api/tenants/{tenantId}/device-config/devices/device/{serialNumber}")]
    Task<Device> GetDeviceBySerial(Guid tenantId, string serialNumber);

    [Post("/api/tenants/{tenantId}/device-config/devices")]
    Task<Device> AddDevice(Guid tenantId, [Body] AddDeviceCommand command);

    [Put("/api/tenants/{tenantId}/device-config/devices/{deviceId}")]
    Task<Device> UpdateDevice(Guid tenantId, Guid deviceId, [Body] UpdateDeviceCommand command);

    [Delete("/api/tenants/{tenantId}/device-config/devices/{deviceId}")]
    Task RemoveDevice(Guid tenantId, Guid deviceId);

    [Put("/api/tenants/{tenantId}/device-config/devices/{deviceId}/assign")]
    Task<ApiResponse<Device>> AssignDevice(Guid tenantId, Guid deviceId, [Body] AllocateDeviceCommand command);

    [Put("/api/tenants/{tenantId}/device-config/devices/{deviceId}/unassign")]
    Task<Device> UnassignDevice(Guid tenantId, Guid deviceId, [Body] UnassignDeviceCommand command);

    [Put("/api/tenants/{tenantId}/device-config/devices/{deviceId}/pool")]
    Task<ApiResponse<Device>> AllocateUnassignedDevice(Guid tenantId, Guid deviceId, [Body] AllocateDeviceCommand command);

    [Get("/api/tenants/{tenantId}/device-config/tablets/serialnumber/family/{family}")]
    Task<ApiResponse<string>> GetTabletSerialNumber(Guid tenantId, DeviceFamily family);

    [Get("/api/tenants/{tenantId}/device-config/tablets/invitecode")]
    Task<ApiResponse<string>> GetTabletInviteCode(Guid tenantId, DeviceFamily family);

    [Post("/api/tenants/{tenantId}/device-config/tablets")]
    Task<IApiResponse> AddAndInviteTablet(Guid tenantId, [Body] AddAndInviteTabletCommand request);

    [Post("/api/tenants/{tenantId}/device-config/tablets/reinvite/{deviceId}")]
    Task<IApiResponse> ResendTabletInvite(Guid tenantId, Guid deviceId);

    [Delete("/api/tenants/{tenantId}/device-config/tablets/{deviceId}")]
    Task<IApiResponse> DeleteTablet(Guid tenantId, Guid deviceId);
}

public record AddAndInviteTabletCommand(
    Guid TenantId,
    int NodeId,
    string DisplayName,
    string SerialNumber,
    string InviteCode,
    DeviceFamily DeviceFamily,
    int TtlHours,
    ContactType ContactType,
    string ContactDetails,
    bool SendInvite = true);

[Obsolete("Only exposed getChildren; use DeviceQueryParams, which also supports Sieve filters, sorts and paging.")]
public class SeiveQueryParams : DeviceQueryParams
{
}

public enum ContactType
{
    Undefined,
    SMS,
    Email,
    WhatsApp
}

public enum DeviceFamily
{
    Unknown = 0,
    ADV = 1,
    GT = 2,
    IT = 3,
    Internal = 4,
    APPLE = 5,
    ANDROID = 6
}

public enum DeviceType
{
    ADV10 = 0,
    ADV40 = 1,
    GT8 = 2,
    GT10 = 3,
    IT11 = 4,
    IT31 = 5,
    IT32 = 6,
    IT51 = 7,
    IT71 = 8,
    IT33 = 9,
    GT10mk2 = 10,
    SMT575 = 11,
    APPLE = 12,
    ANDROID = 13,
    IT72 = 14,
    IT82 = 15,
    IT12 = 16
}

