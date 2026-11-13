using System.Text.Json.Serialization;

namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>An application installed on a device, as reported via IoT.</summary>
public record DeviceAppVersion(string? Label, string? VersionCode, string? VersionName);

/// <summary>The firmware section reported by a device via its IoT twin.</summary>
public record DeviceFirmwareVersion(string? VersionName, string? VersionCode)
{
    /// <summary>Any other values reported in the firmware section.</summary>
    [JsonExtensionData]
    public Dictionary<string, object>? AdditionalData { get; init; }
}

/// <summary>IoT Hub connection state of a device, e.g. "Connected" or "Disconnected".</summary>
public record DeviceConnectionStatus(string ConnectionState);

/// <summary>A single JSON Patch (RFC 6902) operation applied to a device, e.g. ("replace", "/name", "Front door").</summary>
public record DevicePatchOperation(
    string Op,
    string Path,
    object? Value = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? From = null);

/// <summary>Body for decommissioning devices (unassigns them and moves them back to the partner).</summary>
public record DecommissionDevicesCommand(List<Guid> DeviceIds, Guid TenantId);

/// <summary>Body for moving several unassigned devices into an unassigned device pool.</summary>
public record AllocateMultipleToUnassignedPoolCommand(List<Guid> DeviceIds, Guid TenantId, int DestinationNodeId);

/// <summary>Body for moving devices to a node above or below their current node (same channel partner only).</summary>
public record MoveDevicesCommand(List<Guid> DeviceIds, Guid TenantId, int DestinationNodeId, bool Assigned = false, bool Enabled = false);

/// <summary>Body for replacing a failed device (<see cref="OldDeviceId"/>, must match the route) with an unassigned replacement.</summary>
public record ReplaceDeviceCommand(Guid OldDeviceId, Guid ReplacementDeviceId, Guid TenantId);

/// <summary>Body for requesting a device file upload URL.</summary>
public record DeviceFileUploadUrlRequest(string FileName);

/// <summary>A pre-signed (SAS) upload URL, valid for one hour.</summary>
public record DeviceFileUploadUrlResponse(string Url);

/// <summary>Aggregate online/offline/unassigned device counts for a node and its children.</summary>
public record DeviceStateAggregate(int Online, int Offline, int Unassigned);

/// <summary>Device count for one device type / lifecycle state combination.</summary>
public record DeviceTypeLifecycleCount(string DeviceType, string DeviceLifecycleState, int Count);
