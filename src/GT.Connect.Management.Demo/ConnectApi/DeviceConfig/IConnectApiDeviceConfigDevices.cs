using System.Text.Json;
using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>
/// Device endpoints of the DeviceConfiguration service beyond the basic CRUD in <see cref="IConnectApiDeviceConfig"/>:
/// patching, decommissioning, bulk assignment, IoT reported data, registry look-ups and metrics.
/// </summary>
public interface IConnectApiDeviceConfigDevices
{
    // ---- Devices ----

    /// <summary>Applies a JSON Patch to a device and returns the updated device. The server answers 304 when the patch cannot be applied.</summary>
    [Patch("/api/tenants/{tenantId}/device-config/devices/{deviceId}")]
    Task<ApiResponse<Device>> PatchDevice(Guid tenantId, Guid deviceId, [Body] IEnumerable<DevicePatchOperation> operations);

    /// <summary>Decommissions devices: unassigns them and moves them back to the partner. Permissions are checked per device.</summary>
    [Put("/api/tenants/{tenantId}/device-config/devices/decommission")]
    Task<IApiResponse> DecommissionDevices(Guid tenantId, [Body] DecommissionDevicesCommand command);

    /// <summary>Gets the IoT Hub connection state of a device.</summary>
    [Get("/api/tenants/{tenantId}/device-config/devices/{deviceId}/firmware-connection-state")]
    Task<DeviceConnectionStatus> GetDeviceConnectionState(Guid tenantId, Guid deviceId);

    /// <summary>Gets the applications installed on a device, keyed by package, as reported via IoT. 204 when nothing is reported.</summary>
    [Get("/api/tenants/{tenantId}/device-config/devices/{deviceId}/installed-app-versions")]
    Task<ApiResponse<Dictionary<string, DeviceAppVersion>>> GetDeviceInstalledAppVersions(Guid tenantId, Guid deviceId);

    /// <summary>Gets the firmware version reported by a device via IoT. 204 when nothing is reported.</summary>
    [Get("/api/tenants/{tenantId}/device-config/devices/{deviceId}/firmware-version")]
    Task<ApiResponse<DeviceFirmwareVersion>> GetDeviceFirmwareVersion(Guid tenantId, Guid deviceId);

    /// <summary>Gets the deviceInfo (hardware) block reported by a device via IoT; structure varies by device type. 204 when nothing is reported.</summary>
    [Get("/api/tenants/{tenantId}/device-config/devices/{deviceId}/hardware-information")]
    Task<ApiResponse<JsonElement>> GetDeviceHardwareInformation(Guid tenantId, Guid deviceId);

    /// <summary>Gets the configuration currently reported by a device via IoT (key/value settings). 204 when nothing is reported.</summary>
    [Get("/api/tenants/{tenantId}/device-config/devices/{deviceId}/reported-configuration")]
    Task<ApiResponse<JsonElement>> GetDeviceReportedConfiguration(Guid tenantId, Guid deviceId);

    /// <summary>Gets a one-hour SAS URL a device file can be uploaded to (machine clients only). 404 when the device does not exist.</summary>
    [Post("/api/tenants/{tenantId}/device-config/devices/{deviceId}/requestfileuploadurl")]
    Task<ApiResponse<DeviceFileUploadUrlResponse>> RequestDeviceFileUploadUrl(Guid tenantId, Guid deviceId, [Body] DeviceFileUploadUrlRequest request);

    /// <summary>Gets the ids of all devices across all tenants. Root (super) tenant only.</summary>
    [Get("/api/tenants/{tenantId}/device-config/devices/keys")]
    Task<List<Guid>> GetDeviceKeys(Guid tenantId);

    /// <summary>Gets a page of devices across all tenants, ordered by id, without live data (skip/take, take clamped to 1..1000). Root (super) tenant only.</summary>
    [Get("/api/tenants/{tenantId}/device-config/devices/all")]
    Task<List<Device>> GetDevicesForCachePriming(Guid tenantId, [Query] int skip = 0, [Query] int take = 500);

    /// <summary>Gets a device by id searching across all tenants, with live IoT data. Root (super) tenant only. 204 when not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/devices/device/{deviceId}")]
    Task<ApiResponse<Device>> GetDeviceByIdAllTenants(Guid tenantId, Guid deviceId);

    /// <summary>Gets a device by serial number searching across all tenants, with live IoT data. Root (super) tenant only. 204 when not found or ambiguous.</summary>
    [Get("/api/tenants/{tenantId}/device-config/devices/{serialNumber}")]
    Task<ApiResponse<Device>> GetDeviceBySerialNumberAllTenants(Guid tenantId, string serialNumber);

    // ---- Device assignment ----

    /// <summary>
    /// Moves several unassigned devices into an unassigned device pool. The route needs a device id but the server
    /// ignores it; pass any of <see cref="AllocateMultipleToUnassignedPoolCommand.DeviceIds"/>.
    /// </summary>
    [Put("/api/tenants/{tenantId}/device-config/devices/{deviceId}/allocate-multiple-to-pool")]
    Task<ApiResponse<List<Device>>> AllocateMultipleUnassignedDevices(Guid tenantId, Guid deviceId, [Body] AllocateMultipleToUnassignedPoolCommand command);

    /// <summary>
    /// Moves devices to a node above or below their current node. The route needs a device id but the server
    /// ignores it; pass any of <see cref="MoveDevicesCommand.DeviceIds"/>.
    /// </summary>
    [Put("/api/tenants/{tenantId}/device-config/devices/{deviceId}/movedevices")]
    Task<ApiResponse<List<Device>>> MoveDevices(Guid tenantId, Guid deviceId, [Body] MoveDevicesCommand command);

    /// <summary>
    /// Atomically replaces a failed device (<paramref name="deviceId"/>) with an unassigned replacement device: the replacement takes
    /// the failed device's node (disabled), configuration and additional fields, and the failed device is decommissioned.
    /// </summary>
    [Put("/api/tenants/{tenantId}/device-config/devices/{deviceId}/replace")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<Device>> ReplaceDevice(Guid tenantId, Guid deviceId, [Body] ReplaceDeviceCommand command);

    // ---- Registry (machine clients only) ----

    /// <summary>Gets a device's legacy key (machine clients only). 204 when the device or key does not exist.</summary>
    [Get("/api/tenants/{tenantId}/device-config/registry/device/{deviceId}/legacyKey")]
    Task<ApiResponse<string>> GetDeviceLegacyKey(Guid tenantId, Guid deviceId);

    /// <summary>Gets whether a device is enabled (machine clients only). 204 when the device does not exist.</summary>
    [Get("/api/tenants/{tenantId}/device-config/registry/device/{deviceId}/isEnabled")]
    Task<ApiResponse<bool>> GetDeviceIsEnabled(Guid tenantId, Guid deviceId);

    /// <summary>Gets all devices in the tenant without live data (machine clients only).</summary>
    [Get("/api/tenants/{tenantId}/device-config/registry/mapping/tenantDevices")]
    Task<List<Device>> GetRegistryTenantDevices(Guid tenantId);

    /// <summary>Gets a serial number → tenant id map for every device in every tenant. Root (super) tenant machine clients only.</summary>
    [Get("/api/tenants/{tenantId}/device-config/registry/mapping/tenantIdSerialNumberMap")]
    Task<Dictionary<string, string>> GetSerialNumberToTenantIdMap(Guid tenantId);

    /// <summary>Gets a serial number → device id map for every device in every tenant. Root (super) tenant machine clients only.</summary>
    [Get("/api/tenants/{tenantId}/device-config/registry/mapping/deviceIdSerialNumberMap")]
    Task<Dictionary<string, string>> GetSerialNumberToDeviceIdMap(Guid tenantId);

    /// <summary>Gets a page of devices across all tenants (Gridify). Root (super) tenant machine clients only.</summary>
    [Get("/api/tenants/{tenantId}/device-config/registry/devices")]
    [Headers("X-Api-Version: 2.0")]
    Task<PagingResponse<Device>> GetRegistryDevices(Guid tenantId, [Query] GridifyQuery? gridifyQuery = null);

    // ---- Device configuration by serial number (machine clients only) ----

    /// <summary>
    /// Gets the raw configuration data of a given type ("Boot", "Platform", "Application" or "Clock") for a device by serial number
    /// (machine clients only). 204 when the device or configuration does not exist.
    /// </summary>
    [Get("/api/tenants/{tenantId}/device-config/deviceconfiguration/{serialNumber}/{configType}")]
    Task<ApiResponse<string>> GetDeviceConfigurationDataBySerialNumber(Guid tenantId, string serialNumber, string configType);

    // ---- Upload ----

    /// <summary>
    /// Uploads a file as a configuration of a "Configuration" file type on a node (multipart/form-data, asset.edit on the node).
    /// <paramref name="fileTypeId"/> is a GUID string and must be a file type whose assignment type is Configuration.
    /// </summary>
    [Multipart]
    [Post("/api/tenants/{tenantId}/device-config/upload")]
    Task<IApiResponse> UploadDeviceConfigurationFile(Guid tenantId,
        [AliasAs("nodeId")] int nodeId,
        [AliasAs("displayName")] string displayName,
        [AliasAs("fileTypeId")] string fileTypeId,
        [AliasAs("file")] StreamPart file);

    // ---- Metrics ----

    /// <summary>Gets online/offline/unassigned device counts for a node and its children (cached 2 minutes unless forceCacheUpdate).</summary>
    [Get("/api/tenants/{tenantId}/device-config/metrics/aggregate-devicestate/{nodeId}")]
    Task<DeviceStateAggregate> GetDeviceStateAggregate(Guid tenantId, int nodeId, [Query] bool forceCacheUpdate = false);

    /// <summary>Gets online device counts by device type for a node and its children. 400 when live state is unavailable.</summary>
    [Get("/api/tenants/{tenantId}/device-config/metrics/aggregate-devicestate/{nodeId}/by-type")]
    Task<Dictionary<string, int>> GetDeviceStateAggregateByType(Guid tenantId, int nodeId);

    /// <summary>Gets device counts by lifecycle state for a node and its children (cached 5 minutes unless forceCacheUpdate).</summary>
    [Get("/api/tenants/{tenantId}/device-config/metrics/aggregate-lifecycle/{nodeId}")]
    Task<Dictionary<string, int>> GetDeviceLifecycleAggregate(Guid tenantId, int nodeId, [Query] bool forceCacheUpdate = false);

    /// <summary>Gets device counts by device type and lifecycle state for a node and its children (cached 5 minutes unless forceCacheUpdate).</summary>
    [Get("/api/tenants/{tenantId}/device-config/metrics/aggregate-type-lifecycle/{nodeId}")]
    Task<List<DeviceTypeLifecycleCount>> GetDeviceTypeLifecycleAggregate(Guid tenantId, int nodeId, [Query] bool forceCacheUpdate = false);
}
