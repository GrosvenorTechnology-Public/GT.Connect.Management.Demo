using System.Text.Json;
using GT.Connect.Management.Demo.ConnectApi.Core;

namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

public record Device
(
    Guid Id,
    Guid TenantId,
    int NodeId,
    string NodeType,
    string Name,
    string DeviceFamily,
    string DeviceType,
    string? PartNumber,
    string SerialNumber,
    bool IsSubscription,
    bool AzIoTEnabled,
    string DeviceLifecycleState,
    string? LegacyKey,
    string? LegacyKey2,
    DateTimeOffset? PurchasedDate,
    DateTimeOffset? ValidTo,
    string Features,
    string? BillingReference1,
    string? BillingReference2,
    string? BillingReference3,
    string? AdditionalField1,
    string? AdditionalField2,
    string? AdditionalField3,
    string? AdditionalField4,
    string? AdditionalField5,
    string? AdditionalField6,
    string? AdditionalField7,
    string? AdditionalField8,
    string? AdditionalField9,
    string? AdditionalField10,
    bool AdditionalField1_IsSecure,
    bool AdditionalField2_IsSecure,
    bool AdditionalField3_IsSecure,
    bool AdditionalField4_IsSecure,
    bool AdditionalField5_IsSecure,
    bool AdditionalField6_IsSecure,
    bool AdditionalField7_IsSecure,
    bool AdditionalField8_IsSecure,
    bool AdditionalField9_IsSecure,
    bool AdditionalField10_IsSecure,
    bool IsEnabled,
    int? PartnerNodeId,
    int? ChannelPartnerNodeId,
    int? CompanyNodeId,
    int? StructuralNodeId,
    string? Tags,
    string? BioFilterTags,
     bool IsOnline,
    bool IsOnlineWeb,
    bool IsOnlineRS21,
    bool IsOnlineIOT,
    DateTimeOffset? LastConnectionTimeWeb,
    DateTimeOffset? ConsideredOfflineAtWeb,
    DateTimeOffset? LastConnectionTimeRS21,
    DateTimeOffset? ConsideredOfflineAtRS21,
    DateTimeOffset? LastConnectionTimeIOT,
    DateTimeOffset? ConsideredOfflineAtIOT,
    TimeSpan PeriodForOffline,
    string? IpAddress,
    List<Guid>? ClockGroupIds,

    Guid CreatedById,
    DateTimeOffset CreatedOn,
    Guid LastModifiedById,
    DateTimeOffset LastModifiedOn
    ) : BaseModifiyableObject<Guid>(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn)
{
    /// <summary>Enables legacy (RS protocol) file based template distribution. IT family devices on legacy firmware only.</summary>
    public bool LegacyDataDistributionEnabled { get; init; }

    public DateTimeOffset? LegacyKey_LastChangedOn { get; init; }
    public DateTimeOffset? LegacyKey2_LastChangedOn { get; init; }

    /// <summary>True when the device has a legacy key. The key itself is only returned to machine clients.</summary>
    public bool HasLegacyKey { get; init; }

    /// <summary>True when the device has a second legacy key. The key itself is only returned to machine clients.</summary>
    public bool HasLegacyKey2 { get; init; }

    public string? Notes { get; init; }

    /// <summary>Applications installed on the device as reported via IoT (single-device GETs on IoT-enabled devices only).</summary>
    public Dictionary<string, DeviceAppVersion>? AppVersions { get; init; }

    /// <summary>Firmware version name reported via IoT (single-device GETs on IoT-enabled devices only).</summary>
    public string? FirmwareVersion { get; init; }

    /// <summary>Firmware version code reported via IoT (single-device GETs on IoT-enabled devices only).</summary>
    public string? FirmwareVersionCode { get; init; }

    /// <summary>The deviceInfo block reported via the IoT twin; structure varies by device type.</summary>
    public JsonElement? DeviceInfo { get; init; }

    /// <summary>Active connection types derived from the IoT deviceInfo, e.g. "ethernet", "wifi" or "ethernet,wifi".</summary>
    public string? ConnectionType { get; init; }
}
