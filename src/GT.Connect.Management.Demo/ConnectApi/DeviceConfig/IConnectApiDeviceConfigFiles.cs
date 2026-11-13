using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>
/// Device-config endpoints for configuration, assets, asset links, packages, file types, inherited files and commands.
/// Configuration endpoints live here; the other areas are in the inherited sub-interfaces.
/// </summary>
public interface IConnectApiDeviceConfigFiles :
    IConnectApiDeviceConfigAssets,
    IConnectApiDeviceConfigPackages,
    IConnectApiDeviceConfigCommands,
    IConnectApiDeviceConfigInheritedFiles
{
    /// <summary>Lists configurations assigned directly to a node (v1, Sieve). Returns 204 when empty.</summary>
    [Obsolete("The v1 endpoint is deprecated on the server, use GetConfigurationOnNodeV2.")]
    [Get("/api/tenants/{tenantId}/device-config/configuration/node/{nodeId}")]
    Task<ApiResponse<List<DeviceConfiguration>>> GetConfigurationOnNode(Guid tenantId, int nodeId);

    /// <summary>Lists configurations on a node (v2, Gridify paged). <paramref name="getChildren"/> includes all child nodes.</summary>
    [Get("/api/tenants/{tenantId}/device-config/configuration/node/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagedResponse<DeviceConfiguration>>> GetConfigurationOnNodeV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null,
        [Query] bool? getChildren = null);

    /// <summary>Lists configurations assigned directly to a device (v1, Sieve). Returns 204 when empty.</summary>
    [Obsolete("The v1 endpoint is deprecated on the server, use GetConfigurationOnDeviceV2.")]
    [Get("/api/tenants/{tenantId}/device-config/configuration/device/{deviceId}")]
    Task<ApiResponse<List<DeviceConfiguration>>> GetConfigurationOnDevice(Guid tenantId, Guid deviceId);

    /// <summary>Lists configurations assigned directly to a device (v2, Gridify paged).</summary>
    [Get("/api/tenants/{tenantId}/device-config/configuration/device/{deviceId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagedResponse<DeviceConfiguration>>> GetConfigurationOnDeviceV2(Guid tenantId, Guid deviceId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a configuration by id (v1). Returns 204 when not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/configuration/{configurationId}")]
    Task<ApiResponse<DeviceConfiguration>> GetConfigurationById(Guid tenantId, Guid configurationId);

    /// <summary>Gets a configuration by id including <c>TargetDeviceTypes</c> (v2). Returns 204 when not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/configuration/{configurationId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<DeviceConfiguration>> GetConfigurationByIdV2(Guid tenantId, Guid configurationId);

    /// <summary>Gets the materialised (inheritance-merged) configuration of a file type for a device (v2).</summary>
    [Get("/api/tenants/{tenantId}/device-config/configuration/device/{deviceId}/{fileTypeId}/materialized")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<DeviceConfiguration>> GetMaterializedConfigurationForDeviceV2(Guid tenantId, Guid deviceId, Guid fileTypeId);

    /// <summary>Gets the questionnaire configuration for a device on a node (machine client only). Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/configuration/device/{deviceId}/node/{nodeId}")]
    Task<ApiResponse<DeviceConfiguration>> GetQuestionnaireConfiguration(Guid tenantId, Guid deviceId, int nodeId);

    /// <summary>Adds a configuration to a node or device (v1). Returns 201.</summary>
    [Post("/api/tenants/{tenantId}/device-config/configuration")]
    Task<ApiResponse<DeviceConfiguration>> AddConfigurationToNodeOrDevice(Guid tenantId, [Body] AddConfigurationCommand command);

    /// <summary>Adds a configuration to a node or device, honouring <c>TargetDeviceTypes</c> (v2). Returns 201.</summary>
    [Post("/api/tenants/{tenantId}/device-config/configuration")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<DeviceConfiguration>> AddConfigurationToNodeOrDeviceV2(Guid tenantId, [Body] AddConfigurationCommand command);

    /// <summary>Updates a configuration's description and data (v1).</summary>
    [Put("/api/tenants/{tenantId}/device-config/configuration/{configurationId}")]
    Task<ApiResponse<DeviceConfiguration>> UpdateConfiguration(Guid tenantId, Guid configurationId, [Body] UpdateConfigurationCommand command);

    /// <summary>Updates a configuration's description, data and <c>TargetDeviceTypes</c> (v2).</summary>
    [Put("/api/tenants/{tenantId}/device-config/configuration/{configurationId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<DeviceConfiguration>> UpdateConfigurationV2(Guid tenantId, Guid configurationId, [Body] UpdateConfigurationCommand command);

    /// <summary>Deletes a configuration.</summary>
    [Delete("/api/tenants/{tenantId}/device-config/configuration/{configurationId}")]
    Task<IApiResponse> RemoveConfiguration(Guid tenantId, Guid configurationId);
}
