# GT.Connect.Management.Demo
Demos for how to use the GtConnect Management API

## Requirements
- .NET 11 SDK

## Setup
The demos read the details provided to you from [.NET user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets),
so nothing sensitive is stored in the repository. From the `src/GT.Connect.Management.Demo` folder run:

```shell
dotnet user-secrets set Url "https://<your GtConnect api url>"
dotnet user-secrets set TenantId "<your tenant id>"
dotnet user-secrets set ClientId "<client id>"
dotnet user-secrets set ClientSecret "<client secret>"
```

This uses machine (client credentials) authentication. To authenticate as a user instead (password grant), also set:

```shell
dotnet user-secrets set Username "<username>"
dotnet user-secrets set Password "<password>"
```

Alternatively set the same keys as environment variables with a `GTCONNECT_` prefix, e.g. `GTCONNECT_TenantId`.

See `Core/Settings.cs` for how these are loaded.

## The code
The ConnectApi folder contains a [Refit](https://github.com/reactiveui/refit) implementation of the GtConnect Management API.
`IConnectApi` covers People, Organisation, Device Configuration, Application Packages, Integration Edge, Forwarding,
Device Comms, Clock Connect, Platform, Meta Data and the Audit Log; the token endpoint is on the separate `IConnectApiAuth`.

A few things to be aware of when using it:
- Methods marked `[Obsolete]` target deprecated server endpoints, the message names the replacement (usually a `...V2` method).
- The V2 list endpoints are paged with [Gridify](https://alirezanet.github.io/Gridify/). A `GridifyQuery` defaults to a page size of 20,
  so always set `Page`/`PageSize` and page through the results if you need them all (see `SyncEmployeeInCompany`).
- Several collection endpoints return HTTP 204 No Content rather than an empty list, `ConnectApi/Extensions.cs` has helpers for this.
- Updates replace the whole record, so read the record, map it to the update request with `MapperClasses`, change what you need and send it back.

The ApiDemos.cs file contains the following demos. Each is an xUnit test (running on Microsoft.Testing.Platform), so can be run from the Test Explorer
or from the repository root with `dotnet test --filter-method "*<name>"`, e.g. `dotnet test --filter-method "*CreateTestCompany"`
- CreateTestCompany : Add a channel partner and company and set up some sample users. Run this first, the other demos use the company it creates.
- AllocateAndAssignDevice : Takes a test device and assigns it into the company created above
- UnassignAndUnallocateDevice : Returns a device to the partner pool of devices
- CreateAndSendConfiguration : Creates a simple configuration on the company node above and sends it to all devices assigned in the company.
- UploadApplicationAndSendToDevice : Uploads a GT EasyClock application package and sends it to all devices in the company.
- UploadBackgroundAndSendToDevice : Uploads a background image asset and sends it to all devices in the company.
- RebootDevice : Sends a reboot command to a device.
- EnableDevices : Enables all devices under the partner node.
- CreateAndSendConsentPolicyForCompany : Creates a biometric consent policy, activates it on the company and sends it to all devices.
- SyncEmployeeInCompany : Shows how to synchronise clock groups and employees from an external system.

Note that the device demos use hard-coded serial numbers, update these to match devices in your tenant.
