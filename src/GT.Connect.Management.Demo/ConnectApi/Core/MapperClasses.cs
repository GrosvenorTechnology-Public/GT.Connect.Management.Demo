using GT.Connect.Management.Demo.ConnectApi.DeviceConfig;
using GT.Connect.Management.Demo.ConnectApi.Organisation;
using GT.Connect.Management.Demo.ConnectApi.People;
using Riok.Mapperly.Abstractions;

namespace GT.Connect.Management.Demo.ConnectApi;

[Mapper]
public partial class MapperClasses
{
    [MapperIgnoreSource(nameof(NodeResponse.HasChildren))]
    [MapperIgnoreSource(nameof(NodeResponse.EnableAnonymousCredentials))]
    [MapperIgnoreSource(nameof(NodeResponse.PersonTombstonePeriodInherited))]
    [MapperIgnoreSource(nameof(NodeResponse.TransactionRetentionPeriodInherited))]
    [MapperIgnoreTarget(nameof(UpdateNodeCommand.EnableAnonymousCredentials))]
    [MapperIgnoreSource(nameof(Device.CreatedById))]
    [MapperIgnoreSource(nameof(Device.CreatedOn))]
    [MapperIgnoreSource(nameof(Device.LastModifiedById))]
    [MapperIgnoreSource(nameof(Device.LastModifiedOn))]
    public partial UpdateNodeCommand MapToNodeUpdateCommand (NodeResponse source);


    [MapperIgnoreSource(nameof(Device.NodeId))]
    [MapperIgnoreSource(nameof(Device.NodeType))]
    [MapperIgnoreSource(nameof(Device.DeviceType))]
    [MapperIgnoreSource(nameof(Device.DeviceFamily))]
    [MapperIgnoreSource(nameof(Device.PartNumber))]
    [MapperIgnoreSource(nameof(Device.SerialNumber))]
    [MapperIgnoreSource(nameof(Device.IsSubscription))]
    [MapperIgnoreSource(nameof(Device.DeviceLifecycleState))]
    [MapperIgnoreSource(nameof(Device.Features))]
    [MapperIgnoreSource(nameof(Device.PartnerNodeId))]
    [MapperIgnoreSource(nameof(Device.ChannelPartnerNodeId))]
    [MapperIgnoreSource(nameof(Device.CompanyNodeId))]
    [MapperIgnoreSource(nameof(Device.StructuralNodeId))]
    [MapperIgnoreSource(nameof(Device.PurchasedDate))]
    [MapperIgnoreSource(nameof(Device.ValidTo))]
    [MapperIgnoreSource(nameof(Device.IsOnlineIOT))]
    [MapperIgnoreSource(nameof(Device.IsOnlineRS21))]
    [MapperIgnoreSource(nameof(Device.IsOnlineWeb))]
    [MapperIgnoreSource(nameof(Device.IsOnline))]
    [MapperIgnoreSource(nameof(Device.LastConnectionTimeWeb))]
    [MapperIgnoreSource(nameof(Device.ConsideredOfflineAtWeb))]
    [MapperIgnoreSource(nameof(Device.LastConnectionTimeRS21))]
    [MapperIgnoreSource(nameof(Device.ConsideredOfflineAtRS21))]
    [MapperIgnoreSource(nameof(Device.LastConnectionTimeIOT))]
    [MapperIgnoreSource(nameof(Device.ConsideredOfflineAtIOT))]
    [MapperIgnoreSource(nameof(Device.PeriodForOffline))]
    [MapperIgnoreSource(nameof(Device.CreatedById))]
    [MapperIgnoreSource(nameof(Device.CreatedOn))]
    [MapperIgnoreSource(nameof(Device.LastModifiedById))]
    [MapperIgnoreSource(nameof(Device.LastModifiedOn))]
    [MapperIgnoreSource(nameof(Device.IpAddress))]
    [MapperIgnoreSource(nameof(Device.ClockGroupIds))]
    [MapperIgnoreSource(nameof(Device.AppVersions))]
    [MapperIgnoreSource(nameof(Device.ConnectionType))]
    [MapperIgnoreSource(nameof(Device.DeviceInfo))]
    [MapperIgnoreSource(nameof(Device.FirmwareVersion))]
    [MapperIgnoreSource(nameof(Device.FirmwareVersionCode))]
    [MapperIgnoreSource(nameof(Device.HasLegacyKey))]
    [MapperIgnoreSource(nameof(Device.HasLegacyKey2))]
    [MapperIgnoreSource(nameof(Device.LegacyKey_LastChangedOn))]
    [MapperIgnoreSource(nameof(Device.LegacyKey2_LastChangedOn))]
    public partial UpdateDeviceCommand Map(Device device);

    [MapperIgnoreSource(nameof(PersonResponse.Id))]
    [MapperIgnoreSource(nameof(PersonResponse.TenantId))]
    [MapperIgnoreSource(nameof(PersonResponse.IsDeleted))]
    [MapperIgnoreSource(nameof(PersonResponse.CreatedById))]
    [MapperIgnoreSource(nameof(PersonResponse.CreatedOn))]
    [MapperIgnoreSource("LastModifiedBy")]
    [MapperIgnoreSource(nameof(PersonResponse.LastModifiedById))]
    [MapperIgnoreSource(nameof(PersonResponse.LastModifiedOn))]
    public partial PersonRequest Map(PersonResponse person);

    // Not in GT.CS.ManagementApi.Client, added for the SyncEmployeeInCompany demo
    [MapperIgnoreSource(nameof(ClockGroupResponse.CreatedById))]
    [MapperIgnoreSource(nameof(ClockGroupResponse.CreatedOn))]
    [MapperIgnoreSource("LastModifiedBy")]
    [MapperIgnoreSource(nameof(ClockGroupResponse.LastModifiedById))]
    [MapperIgnoreSource(nameof(ClockGroupResponse.LastModifiedOn))]
    [MapperIgnoreTarget(nameof(ClockGroupUpdateRequest.SuspendTagging))]
    public partial ClockGroupUpdateRequest Map(ClockGroupResponse clockGroup);
}
