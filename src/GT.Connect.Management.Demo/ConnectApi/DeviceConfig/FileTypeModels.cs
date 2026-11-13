namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>
/// A device-config file type (server <c>Foundation.Models.DTOs.FileTypeDto</c>). Enum-like fields are sent as strings
/// (e.g. <c>DeviceFamily</c> "GT", <c>DeviceTypes</c> ["GT10", ...]).
/// </summary>
public record FileTypeResponse
(
    Guid Id,
    string? FileAssignmentType,
    string? DeviceFamily,
    string[]? DeviceTypes,
    string? DisplayName,
    bool AllowMultipleFiles,
    string? FormatType,
    string? DefaultDestinationName,
    int? DownloadOrder,
    string? IconName,
    bool? IsInternal,
    bool? AzIoTCompatible,
    string? ResourceName
);
