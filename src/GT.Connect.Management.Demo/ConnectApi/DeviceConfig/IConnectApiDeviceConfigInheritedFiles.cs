using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>Device-config file types (<c>device-config/FileTypes</c>) and inherited files (<c>device-config/InheritedFiles</c>). v1 only.</summary>
public interface IConnectApiDeviceConfigInheritedFiles
{
    // ---- File types ----

    /// <summary>Gets a (non-internal) file type by id. Returns 204 when not found or internal.</summary>
    [Get("/api/tenants/{tenantId}/device-config/FileTypes/{fileTypeId}")]
    Task<ApiResponse<FileTypeResponse>> GetFileType(Guid tenantId, Guid fileTypeId);

    /// <summary>Lists (non-internal) file types for a device family. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/FileTypes/family/{family}")]
    Task<ApiResponse<List<FileTypeResponse>>> GetFileTypesByFamily(Guid tenantId, DeviceFamily family);

    /// <summary>Lists (non-internal) file types applicable to a device type. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/FileTypes/devicetype/{deviceType}")]
    Task<ApiResponse<List<FileTypeResponse>>> GetFileTypesByDeviceType(Guid tenantId, DeviceType deviceType);

    /// <summary>Lists all file types including internal ones (machine client only). Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/FileTypes")]
    Task<ApiResponse<List<FileTypeResponse>>> GetAllFileTypes(Guid tenantId);

    /// <summary>Lists file types that have a resource name (machine client only). Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/FileTypes/resources")]
    Task<ApiResponse<List<FileTypeResponse>>> GetFileTypesWithResourceName(Guid tenantId);

    // ---- Inherited files (node and its ancestors) ----

    /// <summary>Lists asset files that apply to a node, including inherited ones. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/node/{nodeId}/files")]
    Task<ApiResponse<List<InheritedFile>>> GetInheritedFilesOnNode(Guid tenantId, int nodeId);

    /// <summary>Lists asset files that apply to a device, including inherited ones. Returns 204 when none or device not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/device/{deviceId}/files")]
    Task<ApiResponse<List<InheritedFile>>> GetInheritedFilesOnDevice(Guid tenantId, Guid deviceId);

    /// <summary>Lists configurations that apply to a node, including inherited ones. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/node/{nodeId}/config")]
    Task<ApiResponse<List<InheritedFile>>> GetInheritedConfigurationOnNode(Guid tenantId, int nodeId);

    /// <summary>Lists configurations that apply to a device, including inherited ones. Returns 204 when none or device not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/device/{deviceId}/config")]
    Task<ApiResponse<List<InheritedFile>>> GetInheritedConfigurationOnDevice(Guid tenantId, Guid deviceId);

    /// <summary>Lists asset files and configurations that apply to a node, including inherited ones. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/node/{nodeId}/FilesAndConfiguration")]
    Task<ApiResponse<List<InheritedFile>>> GetInheritedFilesAndConfigurationOnNode(Guid tenantId, int nodeId);

    /// <summary>Lists asset files and configurations that apply to a device, including inherited ones. Returns 204 when none or device not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/device/{deviceId}/FilesAndConfiguration")]
    Task<ApiResponse<List<InheritedFile>>> GetInheritedFilesAndConfigurationOnDevice(Guid tenantId, Guid deviceId);

    /// <summary>Lists applications that apply to a node, including inherited ones. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/node/{nodeId}/applications")]
    Task<ApiResponse<List<InheritedFile>>> GetInheritedApplicationsOnNode(Guid tenantId, int nodeId);

    /// <summary>Lists applications that apply to a device, including inherited ones. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/device/{deviceId}/applications")]
    Task<ApiResponse<List<InheritedFile>>> GetInheritedApplicationsOnDevice(Guid tenantId, Guid deviceId);

    /// <summary>Lists firmware that applies to a node, including inherited ones. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/node/{nodeId}/firmware")]
    Task<ApiResponse<List<InheritedFile>>> GetInheritedFirmwareOnNode(Guid tenantId, int nodeId);

    /// <summary>Lists firmware that applies to a device, including inherited ones. Returns 204 when none or device not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/device/{deviceId}/firmware")]
    Task<ApiResponse<List<InheritedFile>>> GetInheritedFirmwareOnDevice(Guid tenantId, Guid deviceId);

    /// <summary>Lists consents that apply to a node, including inherited ones. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/node/{nodeId}/consents")]
    Task<ApiResponse<List<InheritedFile>>> GetInheritedConsentsOnNode(Guid tenantId, int nodeId);

    /// <summary>Lists consents that apply to a device's type on its node. Returns 204 when none, 404 when device not found.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/device/{deviceId}/consents")]
    Task<ApiResponse<List<InheritedFile>>> GetInheritedConsentsOnDevice(Guid tenantId, Guid deviceId);

    // ---- Node summaries (active device counts per node) ----

    /// <summary>Summarises active device counts per node for configuration under a node. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/node/{nodeId}/config-summary")]
    Task<ApiResponse<List<NodeDeviceSummary>>> GetConfigurationSummaryOnNode(Guid tenantId, int nodeId);

    /// <summary>Summarises active device counts per node for asset files under a node. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/node/{nodeId}/file-summary")]
    Task<ApiResponse<List<NodeDeviceSummary>>> GetFileSummaryOnNode(Guid tenantId, int nodeId);

    /// <summary>Summarises active device counts per node for applications under a node. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/node/{nodeId}/application-summary")]
    Task<ApiResponse<List<NodeDeviceSummary>>> GetApplicationSummaryOnNode(Guid tenantId, int nodeId);

    /// <summary>Summarises active device counts per node for firmware under a node. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/node/{nodeId}/firmware-summary")]
    Task<ApiResponse<List<NodeDeviceSummary>>> GetFirmwareSummaryOnNode(Guid tenantId, int nodeId);

    /// <summary>Summarises active device counts per node for consents under a node. Returns 204 when none.</summary>
    [Get("/api/tenants/{tenantId}/device-config/InheritedFiles/node/{nodeId}/consent-summary")]
    Task<ApiResponse<List<NodeDeviceSummary>>> GetConsentSummaryOnNode(Guid tenantId, int nodeId);
}
