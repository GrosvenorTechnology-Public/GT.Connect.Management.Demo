using GT.Connect.Management.Demo.ConnectApi.ApplicationPackage;
using GT.Connect.Management.Demo.ConnectApi.AuditLog;
using GT.Connect.Management.Demo.ConnectApi.ClockConnect;
using GT.Connect.Management.Demo.ConnectApi.DeviceComms;
using GT.Connect.Management.Demo.ConnectApi.DeviceConfig;
using GT.Connect.Management.Demo.ConnectApi.Forwarding;
using GT.Connect.Management.Demo.ConnectApi.IntegrationEdge;
using GT.Connect.Management.Demo.ConnectApi.MetaData;
using GT.Connect.Management.Demo.ConnectApi.Organisation;
using GT.Connect.Management.Demo.ConnectApi.People;
using GT.Connect.Management.Demo.ConnectApi.Platform;

namespace GT.Connect.Management.Demo.ConnectApi;

public interface IConnectApi :
    IConnectApiPeople,
    IConnectApiOrganisation,
    IConnectApiDeviceConfig,
    IConnectApiApplicationPackage,
    IConnectApiIntegrationEdge,
    IConnectApiDeviceComms,
    IConnectApiClockConnect,
    IConnectApiPlatform,
    IConnectApiForwarding,
    IConnectApiMetaData,
    IConnectApiAuditLog
{
    HttpClient Client { get; }
}
