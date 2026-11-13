using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>Device-config command endpoints (<c>device-config/Command</c>): sync, reload, structured commands, queue purges. v1 only.</summary>
public interface IConnectApiDeviceConfigCommands
{
    /// <summary>Synchronises the selected assets/firmware/applications/configs/consents to a node or device. Empty 200 on success.</summary>
    [Post("/api/tenants/{tenantId}/device-config/Command/syncSelected")]
    Task<IApiResponse> SynchroniseSelected(Guid tenantId, [Body] SynchroniseSelectedCommand command);

    /// <summary>Synchronises selected files to a node or device. The server returns no body, so <c>Content</c> is always null.</summary>
    [Obsolete("The server returns an empty 200; the AddAssetToDeviceOrNodeResponse content is always null. Use SynchroniseSelected.")]
    [Post("/api/tenants/{tenantId}/device-config/Command/syncSelected")]
    Task<ApiResponse<AddAssetToDeviceOrNodeResponse>> SynchroniseSelectedOnDeviceOrNode (Guid tenantId,
        [Body] SynchroniseSelectedCommand command);

    /// <summary>Submits a structured application command to a device. Returns 204 (content is empty).</summary>
    [Post("/api/tenants/{tenantId}/device-config/Command/structuredApplicationCommand/{deviceId}")]
    Task<ApiResponse<string>> SubmitStructuredApplicationCommand(Guid tenantId, Guid deviceId, [Body] StructuredCommand data,
            [Header("x-gtc-onbehalfof")] Guid? onBehalfOf);

    /// <summary>Submits a structured firmware command to a device. Returns 204 (the bool content is always false).</summary>
    [Post("/api/tenants/{tenantId}/device-config/Command/structuredFirmwareCommand/{deviceId}")]
    Task<ApiResponse<bool>> SendStructuredFirmwareCommand(Guid tenantId, Guid deviceId,
        [Body] StructuredCommand command);

    /// <summary>Synchronises firmware to a node or device. Empty 200 on success.</summary>
    [Obsolete("Deprecated on the server, use SynchroniseSelected with Firmwares.")]
    [Post("/api/tenants/{tenantId}/device-config/Command/Firmware")]
    Task<ApiResponse<string>> SynchroniseFirmwareOnDeviceOrNode(Guid tenantId,
       [Body] SynchroniseCommand command);

    /// <summary>Synchronises applications to a node or device. Empty 200 on success.</summary>
    [Obsolete("Deprecated on the server, use SynchroniseSelected with Applications.")]
    [Post("/api/tenants/{tenantId}/device-config/Command/Application")]
    Task<ApiResponse<string>> SynchroniseApplicationOnDeviceOrNode(Guid tenantId,
        [Body] SynchroniseCommand command);

    /// <summary>Synchronises configuration and assets (not applications/firmware) to a node or device. Empty 200 on success.</summary>
    [Obsolete("Deprecated on the server, use SynchroniseSelected with Assets.")]
    [Post("/api/tenants/{tenantId}/device-config/Command/File")]
    Task<ApiResponse<string>> SynchroniseFileOnDeviceOrNode(Guid tenantId,
        [Body] SynchroniseCommand command);

    /// <summary>Synchronises configuration to a node or device. Empty 200 on success.</summary>
    [Obsolete("Deprecated on the server, use SynchroniseSelected with Configs.")]
    [Post("/api/tenants/{tenantId}/device-config/Command/Configuration")]
    Task<ApiResponse<string>> SynchroniseConfigurationOnDeviceOrNode(Guid tenantId,
        [Body] SynchroniseCommand command);

    /// <summary>Asks a device to reload.</summary>
    [Post("/api/tenants/{tenantId}/device-config/Command/Reload")]
    Task<IApiResponse> ReloadDevice(Guid tenantId, [Body] ReloadDeviceCommand command);

    /// <summary>Clears the desired package state for a device.</summary>
    [Post("/api/tenants/{tenantId}/device-config/Command/clearDesiredPackages/{deviceId}")]
    Task<IApiResponse> ClearDesiredPackages(Guid tenantId, Guid deviceId);

    /// <summary>Asks a device to upload a file. Body tenant/device ids must match the route.</summary>
    [Post("/api/tenants/{tenantId}/device-config/Command/requestFile/{deviceId}")]
    Task<IApiResponse> RequestFileFromDevice(Guid tenantId, Guid deviceId, [Body] RequestFileFromDeviceCommand command);

    /// <summary>Sends a named action request to a device. Body tenant/device ids must match the route.</summary>
    [Post("/api/tenants/{tenantId}/device-config/Command/actionRequest/{deviceId}")]
    Task<IApiResponse> SendActionRequestToDevice(Guid tenantId, Guid deviceId, [Body] DeviceActionRequestCommand command);

    /// <summary>Temporarily boosts firmware logging on a device. Returns 204.</summary>
    [Post("/api/tenants/{tenantId}/device-config/Command/firmwareBoostLog/{deviceId}")]
    Task<IApiResponse> SendFirmwareBoostLogCommand(Guid tenantId, Guid deviceId);

    /// <summary>Purges specific queued messages for a device. 400 when <c>MessageIds</c> is empty.</summary>
    [Post("/api/tenants/{tenantId}/device-config/Command/purgeQueuedMessages")]
    Task<IApiResponse> PurgeQueuedMessagesById(Guid tenantId, [Body] PurgeQueuedMessagesCommand command);

    /// <summary>Purges all queued messages for a device.</summary>
    [Post("/api/tenants/{tenantId}/device-config/Command/purgeQueuedMessages/device/{deviceId}")]
    Task<IApiResponse> PurgeQueuedMessagesOnDevice(Guid tenantId, Guid deviceId, [Body] DeviceCommandIdRequest command);

    /// <summary>Purges all queued messages for devices on a node.</summary>
    [Post("/api/tenants/{tenantId}/device-config/Command/purgeQueuedMessages/node/{nodeId}")]
    Task<IApiResponse> PurgeQueuedMessagesOnNode(Guid tenantId, int nodeId, [Body] DeviceCommandIdRequest command);
}
