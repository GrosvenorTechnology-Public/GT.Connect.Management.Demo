using System.Diagnostics;
using System.Net;
using System.Reflection;
using Bogus;
using GT.Connect.Management.Demo.ConnectApi.ApplicationPackage;
using GT.Connect.Management.Demo.ConnectApi.DeviceConfig;
using GT.Connect.Management.Demo.ConnectApi.Organisation;
using GT.Connect.Management.Demo.ConnectApi.People;
using Gridify;

namespace GT.Connect.Management.Demo;

public class ApiDemos : ApiTestBase
{
    /// <summary>
    /// Creates a channel partner and company node, a default clock group and some sample people.
    /// Most of the other demos expect this to have been run first.
    /// </summary>
    [Fact]
    public async Task CreateTestCompany()
    {
        var api = await GetClient();

        //Find the partner node
        var rootNode = (await api.GetNodes(Settings.TenantId,
            new SieveQueryParams(new Filter(nameof(NodeResponse.NodeType), nameof(NodeType.Partner), Operators.Equals))))
            .Single();

        //Look for the 'Api Demo' node directly under the partner node and create if not found
        //  GetChildNodesV2 returns the node itself plus its descendants down to nodeDepth levels
        var childNodes = await api.GetChildNodesV2(Settings.TenantId, rootNode.Id, nodeDepth: 1);
        var cpNode = childNodes.SingleOrDefault(x => x.Name == "Api Demo");
        if (cpNode is null)
        {
            cpNode = await api.AddNode(Settings.TenantId,
                new AddNodeCommand(Settings.TenantId, rootNode.Id, nameof(NodeType.ChannelPartner), "Api Demo"));
        }

        //Now Look for the 'InGen' node and create if not found
        childNodes = await api.GetChildNodesV2(Settings.TenantId, cpNode.Id, nodeDepth: 1);
        var cmpNode = childNodes.SingleOrDefault(x => x.Name == "InGen");
        if (cmpNode is null)
        {
            cmpNode = await api.AddNode(Settings.TenantId,
                new AddNodeCommand(Settings.TenantId, cpNode.Id, nameof(NodeType.Company), "InGen"));
        }

        CompanyResponse company;
        var retryCount = 3;
        while (true)
        {
            try
            {
                //Find the Company so we can get its ID, note that it can take a few seconds to appear
                company = await api.GetCompanyByNodeId(Settings.TenantId, cmpNode.Id);
                break;
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound && retryCount > 0)
            {
                await Task.Delay(1000, TestContext.Current.CancellationToken);
                retryCount--;
            }
        }

        //Add a default clock group (ExternalId is your ref for a clock group), the lookup returns 404 when not found
        var clockGroupResponse = await api.GetClockGroupByExternalId(Settings.TenantId, cmpNode.Id, "1");
        ClockGroupResponse clockGroup;
        if (clockGroupResponse.StatusCode == HttpStatusCode.OK)
        {
            clockGroup = clockGroupResponse.Content!;
        }
        else
        {
            clockGroup = await api.AddClockGroup(Settings.TenantId, cmpNode.Id,
                new ClockGroupRequest(Settings.TenantId, cmpNode.Id, company.Id, "1", "Default Group", ""));
        }

        //Now lets add some People to the Group
        Randomizer.Seed = new Random(1905);
        for (var i = 0; i < 10; i++)
        {
            var apiResponse = await api.GetPersonByExternalId(Settings.TenantId, company.NodeId, i.ToString());
            PersonResponse person;
            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                person = apiResponse.Content!;
            }
            else
            {
                var p = new Person();  //This is the source of fake data
                person = await api.AddPerson(Settings.TenantId, company.NodeId,
                    new(cmpNode.Id, company.Id, p.FirstName, p.LastName, i.ToString(),
                        (1000 + i).ToString(), (1000 + i).ToString(), "", "EN", "", "", "", "", "", null, null));
            }

            await api.AddPersonToClockGroup(Settings.TenantId, company.NodeId, clockGroup.Id, person.Id);
        }
    }

    /// <summary>
    /// This test will find a device in the partner unallocated pool and transfer it to the company
    /// assign it and add it to a clock group.
    /// </summary>
    [Fact]
    public async Task AllocateAndAssignDevice()
    {
        var api = await GetClient();

        //Get the company node, this is where we want to send the device
        var cmpNode = (await api.GetNodes(Settings.TenantId,
            new SieveQueryParams(new Filter(nameof(NodeResponse.NodeType), nameof(NodeType.Company), Operators.Equals))))
            .Single(x => x.Name == "InGen");

        //find the device by its serial number, the server returns 204 (so null here) if it is not found
        var device = await api.GetDeviceBySerial(Settings.TenantId, "FP-GT8-BL~909000020");
        if (device is null)
        {
            Assert.Fail("No device found");
        }

        //Allocate the device into the company node, this returns an updated device record
        var allocateResponse = await api.AllocateUnassignedDevice(Settings.TenantId, device.Id,
            new(device.Id, Settings.TenantId, cmpNode.Id));
        await allocateResponse.EnsureSuccessfulAsync();
        device = allocateResponse.Content!;

        //In this example we're just adding the device at the root of the company node,
        //  but this could have been a structural node under the company
        var assignResponse = await api.AssignDevice(Settings.TenantId, device.Id,
            new(device.Id, Settings.TenantId, cmpNode.Id));
        await assignResponse.EnsureSuccessfulAsync();
        device = assignResponse.Content!;

        //Enable the device
        //  It's important to supply all the fields currently, as the update operation
        //  is a full record update, we do not currently support patches.
        //  To make this easier, we use a mapper here
        var mapper = new MapperClasses();

        if (!device.IsEnabled)
        {
            var updateCommand = mapper.Map(device) with
            {
                IsEnabled = true,
            };

            await api.UpdateDevice(Settings.TenantId, device.Id, updateCommand);
        }

        //Add the device to the clock group
        var clockGroupResponse = await api.GetClockGroupByExternalId(Settings.TenantId, cmpNode.Id, "1");
        await clockGroupResponse.EnsureSuccessfulAsync();
        var linkResponse = await api.LinkDeviceToClockGroup(Settings.TenantId, cmpNode.Id,
            clockGroupResponse.Content!.Id,
            device.Id);
        await linkResponse.EnsureSuccessfulAsync();
    }

    /// <summary>
    /// This test will unassign a device and return it to the partner root node.
    /// </summary>
    [Fact]
    public async Task UnassignAndUnallocateDevice()
    {
        var api = await GetClient();

        //Get the root node, this is where unallocated devices start
        var rootNode = (await api.GetNodes(Settings.TenantId,
            new SieveQueryParams(new Filter(nameof(NodeResponse.NodeType), nameof(NodeType.Partner), Operators.Equals))))
            .Single();

        //find the device by its serial number, the server returns 204 (so null here) if it is not found
        var device = await api.GetDeviceBySerial(Settings.TenantId, "fp-gt8~99990001");
        if (device is null)
        {
            Assert.Fail("No device found");
        }

        if (device.DeviceLifecycleState == "Assigned")
        {
            //Unassign the device, this returns an updated device record
            device = await api.UnassignDevice(Settings.TenantId, device.Id,
                new(device.Id, Settings.TenantId));
        }

        //Allocate the device back to the root partner node
        var allocateResponse = await api.AllocateUnassignedDevice(Settings.TenantId, device.Id,
            new(device.Id, Settings.TenantId, rootNode.Id));
        await allocateResponse.EnsureSuccessfulAsync();
        device = allocateResponse.Content!;

        Assert.Equal(nameof(NodeType.Partner), device.NodeType);
        Assert.Equal("Unassigned", device.DeviceLifecycleState);
    }

    /// <summary>
    /// This example will create a simple config file on a node and send it.
    /// </summary>
    [Fact]
    public async Task CreateAndSendConfiguration()
    {
        //Note for simplicity we've hardcoded the FileTypeId here, but you can get them by
        //  calling the /api/tenants/{{tenantId}}/device-config/fileTypes/family/{{deviceFamily}} or
        //  /api/tenants/{{tenantId}}/device-config/fileTypes/deviceType/{{deviceType}} apis.
        var policyFileTypeId = new Guid("5BEBD327-D6E1-427D-8A34-38C717B50A3E");
        var policyName = "Api Demo Policy";
        var policyData = """
            <?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
            <configuration>
                <com.gtl.android.gteasyclock>
                    <settings>
                        <app_log>dbg</app_log>
                    </settings>
                </com.gtl.android.gteasyclock>
            </configuration>
            """;

        var api = await GetClient();

        //Get the company node, this is where we want to add the configuration
        var cmpNode = (await api.GetNodes(Settings.TenantId,
            new SieveQueryParams(new Filter(nameof(NodeResponse.NodeType), nameof(NodeType.Company), Operators.Equals))))
            .Single(x => x.Name == "InGen");

        //Look for an existing configuration with our name, using a Gridify filter
        var response = await api.GetConfigurationOnNodeV2(Settings.TenantId, cmpNode.Id,
            new GridifyQuery { Filter = $"{nameof(DeviceConfiguration.Description)}={policyName}", Page = 1, PageSize = 10 });
        await response.EnsureSuccessfulAsync();
        var policy = response.Content?.Data.FirstOrDefault();

        if (policy is not null)
        {
            //Policy exists, lets update instead
            var updateResponse = await api.UpdateConfiguration(Settings.TenantId, policy.Id,
                new(Settings.TenantId, policyName, policyData));
            await updateResponse.EnsureSuccessfulAsync();
            policy = updateResponse.Content!;
        }
        else
        {
            var addResponse = await api.AddConfigurationToNodeOrDevice(Settings.TenantId,
                new(Settings.TenantId, cmpNode.Id, null, policyName, policyFileTypeId, policyData));
            await addResponse.EnsureSuccessfulAsync();
            policy = addResponse.Content!;
        }

        //Now send the policy to all devices under the company node.
        var syncResponse = await api.SynchroniseSelected(Settings.TenantId,
            new SynchroniseSelectedCommand(Settings.TenantId, cmpNode.Id, null, Configs: [policy.Id]));
        await syncResponse.EnsureSuccessfulAsync();
    }

    /// <summary>
    /// This example will upload a GT EasyClock application package, link it to the company node
    /// and send it to all devices under that node.
    /// </summary>
    [Fact]
    public async Task UploadApplicationAndSendToDevice()
    {
        //Set this to the path of the GT8 application package, and the version details of that package
        var appFileName = @"c:\tmp\gteasyclock-v2.8.0-gtconnect.zip";
        var appDisplayName = "GT EasyClock (2.8.0)";
        var appVersionCode = "2080004";

        //Note for simplicity we've hardcoded the FileTypeId here, but you can get them by
        //  calling the /api/tenants/{{tenantId}}/device-config/fileTypes/family/{{deviceFamily}} or
        //  /api/tenants/{{tenantId}}/device-config/fileTypes/deviceType/{{deviceType}} apis.
        var gtApplicationFileType = new Guid("097C3986-BF3A-4E90-82EB-DE3FFD0FFB1B");

        var api = await GetClient();

        //Get the company node, this is where we want to send the application
        var cmpNode = (await api.GetNodes(Settings.TenantId,
            new SieveQueryParams(new Filter(nameof(NodeResponse.NodeType), nameof(NodeType.Company), Operators.Equals))))
            .Single(x => x.Name == "InGen");

        //First we need to request an upload url from the server,
        //  the package details (type, version etc.) are read from the package itself
        var uploadUrl = await api.GetUploadPackageUrl(Settings.TenantId,
            new GetPackageUploadUrlCommand(Settings.TenantId, Path.GetFileName(appFileName)));

        //Next we PUT the file to the server
        using var uploadClient = new HttpClient();
        await using var fileStream = File.OpenRead(appFileName);
        using var request = new HttpRequestMessage(HttpMethod.Put, uploadUrl.Url)
        {
            Content = new StreamContent(fileStream)
        };
        request.Headers.Add("x-ms-blob-type", "BlockBlob");  //the target is azure blob storage and needs this header
        using var uploadResponse = await uploadClient.SendAsync(request, TestContext.Current.CancellationToken);
        uploadResponse.EnsureSuccessStatusCode();

        //Once the file is uploaded, the server needs to process the file, so we need to poll till it's finished
        //  States: InProgress / Processing / Completed / Failed / Uncompress / Moving / Uploading
        var timeout = DateTime.UtcNow.AddMinutes(5);
        GetUploadStatusResponse statusResponse;
        do
        {
            statusResponse = await api.GetUploadStatus(Settings.TenantId, uploadUrl.Id);
            Debug.WriteLine(statusResponse.State);
            await Task.Delay(1000, TestContext.Current.CancellationToken);
        } while (statusResponse.State is not ("Completed" or "Failed") && DateTime.UtcNow < timeout);

        if (statusResponse.State != "Completed")
        {
            Assert.Fail($"{statusResponse.State} : {statusResponse.FaultSummary}");
        }

        //Now find the uploaded package, newest first in case it has been uploaded before
        var response = await api.GetAppPackages(Settings.TenantId, new GridifyQuery
        {
            Filter = $"{nameof(AppPackage.FileTypeId)}={gtApplicationFileType},{nameof(AppPackage.VersionCode)}={appVersionCode}",
            OrderBy = $"{nameof(AppPackage.CreatedOn)} desc",
            Page = 1,
            PageSize = 1
        });
        await response.EnsureSuccessfulAsync();
        var package = response.Content?.Data.FirstOrDefault();
        if (package is null)
        {
            Assert.Fail("Could not find package after upload");
        }

        //Link the application to a node so we can send to the device
        var link = await api.AddPackageToDeviceOrNode(Settings.TenantId,
            new AddPackageToDeviceOrNodeCommand(Settings.TenantId, package.Id, cmpNode.Id, null,
                appDisplayName, package.FileTypeId));

        //Now send the application to all devices under the company node.
        var syncResponse = await api.SynchroniseSelected(Settings.TenantId,
            new SynchroniseSelectedCommand(Settings.TenantId, cmpNode.Id, null,
                Applications: [link.Id],
                Assets: [], //Note in early versions, all the arrays must be non null, fixed dec-2023
                Configs: [],
                Consents: [],
                Firmwares: []));
        await syncResponse.EnsureSuccessfulAsync();
    }

    /// <summary>
    /// This example will upload a background image asset, link it to the company node
    /// and send it to all devices under that node.
    /// </summary>
    [Fact]
    public async Task UploadBackgroundAndSendToDevice()
    {
        //The image is copied to the output folder on build
        var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var imageFileName = Path.Combine(location, "Resources", "background.jpg");
        var assetDisplayName = "Example Background Image";

        //Note for simplicity we've hardcoded the FileTypeId here, but you can get them by
        //  calling the /api/tenants/{{tenantId}}/device-config/fileTypes/family/{{deviceFamily}} or
        //  /api/tenants/{{tenantId}}/device-config/fileTypes/deviceType/{{deviceType}} apis.
        var fileType = new Guid("61C66B63-E667-40AB-9D57-70088C7C4AB1");

        var api = await GetClient();

        //Get the company node, this is where we want to send the asset
        var cmpNode = (await api.GetNodes(Settings.TenantId,
            new SieveQueryParams(new Filter(nameof(NodeResponse.NodeType), nameof(NodeType.Company), Operators.Equals))))
            .Single(x => x.Name == "InGen");

        //First we need to request an upload url from the server, the asset is stored against the node
        var uploadUrl = await api.GetUploadAssetUrl(Settings.TenantId,
            new GetAssetUploadUrlCommand(Settings.TenantId, fileType, Path.GetFileName(imageFileName),
                assetDisplayName, cmpNode.Id));

        //Next we PUT the file to the server
        using var uploadClient = new HttpClient();
        await using var fileStream = File.OpenRead(imageFileName);
        using var request = new HttpRequestMessage(HttpMethod.Put, uploadUrl.Url)
        {
            Content = new StreamContent(fileStream)
        };
        request.Headers.Add("x-ms-blob-type", "BlockBlob");  //the target is azure blob storage and needs this header
        using var uploadResponse = await uploadClient.SendAsync(request, TestContext.Current.CancellationToken);
        uploadResponse.EnsureSuccessStatusCode();

        //Once the file is uploaded, the server needs to process the file, so we need to poll till it's finished
        //  States: InProgress / Processing / Completed / Failed / Uncompress / Moving / Uploading
        var timeout = DateTime.UtcNow.AddMinutes(5);
        GetUploadStatusResponse statusResponse;
        do
        {
            statusResponse = await api.GetUploadStatus(Settings.TenantId, uploadUrl.Id);
            Debug.WriteLine(statusResponse.State);
            await Task.Delay(1000, TestContext.Current.CancellationToken);
        } while (statusResponse.State is not ("Completed" or "Failed") && DateTime.UtcNow < timeout);

        if (statusResponse.State != "Completed")
        {
            Assert.Fail($"{statusResponse.State} : {statusResponse.FaultSummary}");
        }

        //Now find the uploaded asset, newest first in case this demo has been run before
        var response = await api.GetAssetsOnNodeV2(Settings.TenantId, cmpNode.Id, new GridifyQuery
        {
            Filter = $"{nameof(GetAssetsResponse.DisplayName)}={assetDisplayName}",
            OrderBy = $"{nameof(GetAssetsResponse.CreatedOn)} desc",
            Page = 1,
            PageSize = 1
        });
        await response.EnsureSuccessfulAsync();
        var asset = response.Content?.Data.FirstOrDefault();
        if (asset is null)
        {
            Assert.Fail("Could not find asset after upload");
        }

        //Link the asset to a node so we can send to the device
        var linkResponse = await api.AddAssetToDeviceOrNode(Settings.TenantId,
            new AddAssetToDeviceOrNodeCommand(Settings.TenantId, asset.Id, cmpNode.Id, null));
        await linkResponse.EnsureSuccessfulAsync();

        //Now send the asset to all devices under the company node.
        var syncResponse = await api.SynchroniseSelected(Settings.TenantId,
            new SynchroniseSelectedCommand(Settings.TenantId, cmpNode.Id, null,
                Applications: [],
                Assets: [linkResponse.Content!.Id],
                Configs: [],
                Consents: [],
                Firmwares: []));
        await syncResponse.EnsureSuccessfulAsync();
    }

    /// <summary>
    /// This example will send a reboot command to a device.
    ///  This same method can be used to send restart/reconnect/reset commands.
    /// </summary>
    [Fact]
    public async Task RebootDevice()
    {
        var api = await GetClient();

        //find the device by its serial number, the server returns 204 (so null here) if it is not found
        var device = await api.GetDeviceBySerial(Settings.TenantId, "fp-gt8~99990001");
        if (device is null)
        {
            Assert.Fail("No device found");
        }

        //The server accepts the command with a 204, a success here means the command was queued,
        //  not that the device has completed it. Use the device command history to track it.
        var commandResponse = await api.SendStructuredFirmwareCommand(Settings.TenantId, device.Id, new StructuredCommand
        {
            CommandName = "reboot",
            Entity = "device"
        });
        await commandResponse.EnsureSuccessStatusCodeAsync();
    }

    /// <summary>
    /// This test will find all devices and enable them.
    /// </summary>
    [Fact]
    public async Task EnableDevices()
    {
        var api = await GetClient();

        //Get the root node
        var rootNode = (await api.GetNodes(Settings.TenantId,
            new SieveQueryParams(new Filter(nameof(NodeResponse.NodeType), nameof(NodeType.Partner), Operators.Equals))))
            .Single();

        //Get all the devices on the root partner node and all of its child nodes, a page at a time
        var devices = new List<Device>();
        var query = new GridifyQuery { Page = 1, PageSize = 100 };
        while (true)
        {
            var apiResponse = await api.GetDevicesV2(Settings.TenantId, rootNode.Id, getChildren: true, gridifyModel: query);
            await apiResponse.EnsureSuccessfulAsync();
            var page = apiResponse.Content!;
            devices.AddRange(page.Data);

            if (!page.Data.Any() || devices.Count >= page.Count)
            {
                break;
            }
            query.Page++;
        }

        //Finally enable the devices
        //  It's important to supply all the fields currently, as the update operation
        //  is a full record update, we do not currently support patches.
        //  To make this easier, we use a mapper here
        var mapper = new MapperClasses();

        foreach (var device in devices)
        {
            if (!device.IsEnabled)
            {
                var updateCommand = mapper.Map(device) with
                {
                    IsEnabled = true,
                };

                await api.UpdateDevice(Settings.TenantId, device.Id, updateCommand);
                await Task.Delay(500, TestContext.Current.CancellationToken);
            }
        }
    }

    /// <summary>
    /// This example will create a consent policy, activate it on a company
    /// and send it to all devices under the company node.
    /// </summary>
    [Fact]
    public async Task CreateAndSendConsentPolicyForCompany()
    {
        var api = await GetClient();

        //Get the company node, this is where we want to send the policy
        var cmpNode = (await api.GetNodes(Settings.TenantId,
            new SieveQueryParams(new Filter(nameof(NodeResponse.NodeType), nameof(NodeType.Company), Operators.Equals))))
            .Single(x => x.Name == "InGen");

        var company = await api.GetCompanyByNodeId(Settings.TenantId, cmpNode.Id);

        //Create the consent policy
        var consentCommand = new CreateConsentCommand(Settings.TenantId, cmpNode.Id, company.Id,
            $"Api Demo Consent {DateTime.UtcNow:g}", new Guid("DBFF5D03-11CA-4539-C65A-08D97769F994"), 365, 30, 0);
        var agreementResponse = await api.AddConsentAgreementToCompany(Settings.TenantId, cmpNode.Id, consentCommand);
        await agreementResponse.EnsureSuccessfulAsync();
        var agreementId = agreementResponse.Content!.Id;

        //Create an English consent text, you can add as many languages as you like
        var consentTextCommand = new CreateConsentAgreementTextCommand(Settings.TenantId, cmpNode.Id,
            agreementId, "Api Demo Consent - EN",
            "EN", "This is the consent text for the Api Demo Consent");
        var textResponse = await api.AddConsentTextToAgreement(Settings.TenantId, cmpNode.Id,
                consentTextCommand);
        await textResponse.EnsureSuccessfulAsync();

        //Now make the consent active on the company.
        //  Only one consent can be active per credential type, the server returns 409 if one already exists
        var linkResponse = await api.ActivateConsentOnCompany(Settings.TenantId, cmpNode.Id,
            new ActivateConsentCommand(cmpNode.Id, agreementId));

        //Now send the policy to all devices under the company node.
        var syncResponse = await api.SynchroniseSelected(Settings.TenantId,
            new SynchroniseSelectedCommand(Settings.TenantId, cmpNode.Id, null,
                Applications: [],
                Assets: [],
                Configs: [],
                Consents: [linkResponse.Id],
                Firmwares: []));
        await syncResponse.EnsureSuccessfulAsync();
    }

    /// <summary>
    /// This example shows how to perform a sync of users from an external system.
    /// In this example we generate a sample list of employees and clock groups and sync.
    /// The sync process will first add or update the clock groups then remove any that should no longer exist.
    /// The process is then repeated for employees, adding or updating them as needed.
    /// In both cases, the external ID is used as the common identifier between the external system and GTC.
    /// It is assumed that the external ID is unique and immutable for each clock group/employee.
    /// For Employees, badge code and keypad id are also required to be unique if provided.
    /// </summary>
    [Fact]
    public async Task SyncEmployeeInCompany()
    {
        var api = await GetClient();
        var mapper = new MapperClasses();

        //Find the partner node
        var rootNode = (await api.GetNodes(Settings.TenantId,
            new SieveQueryParams(new Filter(nameof(NodeResponse.NodeType), nameof(NodeType.Partner), Operators.Equals))))
            .Single();

        //Look for the 'Api Demo' node
        var childNodes = await api.GetChildNodesV2(Settings.TenantId, rootNode.Id, nodeDepth: 1);
        var cpNode = childNodes.SingleOrDefault(x => x.Name == "Api Demo");
        if (cpNode is null)
        {
            Assert.Fail("Api Demo node not found, please run CreateTestCompany first.");
        }

        //Now look for the 'InGen' node
        childNodes = await api.GetChildNodesV2(Settings.TenantId, cpNode.Id, nodeDepth: 1);
        var cmpNode = childNodes.SingleOrDefault(x => x.Name == "InGen");
        if (cmpNode is null)
        {
            Assert.Fail("InGen node not found, please run CreateTestCompany first.");
        }

        //Find the Company so we can get its ID
        var company = await api.GetCompanyByNodeId(Settings.TenantId, cmpNode.Id);

        //Get the existing clock groups, a page at a time
        var clockGroups = new List<ClockGroupResponse>();
        var clockGroupQuery = new GridifyQuery { Page = 1, PageSize = 100 };
        while (true)
        {
            var clockGroupsResponse = await api.GetClockGroupsV2(Settings.TenantId, cmpNode.Id, clockGroupQuery);
            await clockGroupsResponse.EnsureSuccessfulAsync();
            var page = clockGroupsResponse.Content!;
            clockGroups.AddRange(page.Data);

            if (!page.Data.Any() || clockGroups.Count >= page.Count)
            {
                break;
            }
            clockGroupQuery.Page++;
        }

        //Add or update the clock groups based on the sample data
        foreach (var sampleClockGroup in SampleClockGroups)
        {
            var cg = clockGroups.SingleOrDefault(x => x.ExternalId == sampleClockGroup.ExternalId);
            if (cg is null)
            {
                //Does not exist, so create it
                var newClockGroup = await api.AddClockGroup(Settings.TenantId, cmpNode.Id,
                    new ClockGroupRequest(Settings.TenantId, cmpNode.Id, company.Id, sampleClockGroup.ExternalId, sampleClockGroup.Name, ""));
                clockGroups.Add(newClockGroup);
            }
            else if (cg.Name != sampleClockGroup.Name)
            {
                //Exists, but the name has changed so update it
                //  Note the update replaces every field (including Tags), so map the existing record and change what we need
                var cgUpdateRequest = mapper.Map(cg) with
                {
                    Name = sampleClockGroup.Name,
                };
                await api.UpdateClockGroup(Settings.TenantId, cmpNode.Id, cg.Id, cgUpdateRequest);
            }
        }

        //Delete clock groups that are no longer in the sample data
        var clockGroupsToDelete = clockGroups
            .Where(x => !SampleClockGroups.Any(y => x.ExternalId == y.ExternalId))
            .ToList();
        foreach (var cg in clockGroupsToDelete)
        {
            if (cg.ExternalId == "1")
            {
                //We don't delete the default clock group, so skip it
                continue;
            }

            //Delete the clock group - All users must be removed first
            await api.DeleteClockGroup(Settings.TenantId, cmpNode.Id, cg.Id);
            clockGroups.Remove(cg);
        }

        //Then loop over the list of sample employees and add or update them
        var sampleEmployees = GenerateSampleEmployees();
        foreach (var employee in sampleEmployees)
        {
            //The external ID is the common identifier between external systems and GTC, the lookup returns 404 when not found
            var apiResponse = await api.GetPersonByExternalId(Settings.TenantId, company.NodeId, employee.Id);
            PersonResponse person;
            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                person = apiResponse.Content ?? throw new InvalidOperationException("Person should not be null here.");

                //Do a simple check if anything has changed, and if so update
                if (person.Firstname != employee.GivenName ||
                    person.Surname != employee.FamilyName ||
                    person.BadgeCode != employee.BadgeCode ||
                    person.KeyPad != employee.KeypadId ||
                    person.Language != employee.Language)
                {
                    //Update the person with the new details
                    //  The update is a full record update, so we map the existing record and change the fields we need
                    var updateCommand = mapper.Map(person) with
                    {
                        Firstname = employee.GivenName,
                        Surname = employee.FamilyName,
                        BadgeCode = employee.BadgeCode,
                        KeyPad = employee.KeypadId,
                        Language = employee.Language
                    };
                    var updateResponse = await api.UpdatePerson(Settings.TenantId, company.NodeId, person.Id, updateCommand);
                    await updateResponse.EnsureSuccessfulAsync();
                    person = updateResponse.Content!;
                }
            }
            else if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                person = await api.AddPerson(Settings.TenantId, company.NodeId,
                    new(cmpNode.Id, company.Id, employee.GivenName, employee.FamilyName, employee.Id,
                        employee.BadgeCode, employee.KeypadId, "", employee.Language, "", "", "", "", "", null, null));
            }
            else
            {
                //Don't treat any other failure (auth, permissions, server error) as 'missing', or we'd create duplicates
                await apiResponse.EnsureSuccessfulAsync();
                continue;
            }

            foreach (var group in employee.DistributionGroups)
            {
                //Match the external ID to a clock Group, and add the person to it
                var cg = clockGroups.Single(x => x.ExternalId == group);
                await api.AddPersonToClockGroup(Settings.TenantId, company.NodeId, cg.Id, person.Id);
            }
        }

        //Reconcile employee list, removing anyone no longer in the source system
        //  First get everyone in the company, a page at a time
        var people = new List<PersonResponse>();
        var peopleQuery = new GridifyQuery { Page = 1, PageSize = 100 };
        while (true)
        {
            var peopleResponse = await api.GetPeopleV2(Settings.TenantId, company.NodeId, peopleQuery);
            await peopleResponse.EnsureSuccessfulAsync();
            var page = peopleResponse.Content!;
            people.AddRange(page.Data);

            if (!page.Data.Any() || people.Count >= page.Count)
            {
                break;
            }
            peopleQuery.Page++;
        }

        var peopleToDelete = people.Where(x => !sampleEmployees.Any(y => x.ExternalId == y.Id));
        foreach (var person in peopleToDelete)
        {
            await api.DeletePerson(Settings.TenantId, company.NodeId, person.Id);
        }
    }

    private static readonly List<(string ExternalId, string Name)> SampleClockGroups =
    [
        ("10", "Aberdeen"),
        ("11", "Bangor"),
        ("12", "Bath"),
        ("13", "Birmingham"),
        ("14", "Cambridge"),
        ("15", "Canterbury"),
        ("16", "Gloucester"),
        ("17", "Leicester"),
        ("18", "Milton Keynes"),
        ("19", "Nottingham"),
        ("20", "Ripon"),
        ("21", "Southampton"),
        ("22", "Wells"),
        ("23", "York")
    ];

    public record SampleEmployee(Guid RawId, string Id, string GivenName, string FamilyName, string Language,
        List<string> DistributionGroups, string BadgeCode, string KeypadId);

    /// <summary>
    /// Generates a repeatable set of sample employees, the subset returned changes through the day
    /// so that repeated runs of <see cref="SyncEmployeeInCompany"/> exercise adds, updates and deletes.
    /// </summary>
    public static List<SampleEmployee> GenerateSampleEmployees()
    {
        Randomizer.Seed = new Random(8675309);
        var data = Enumerable.Range(0, 100)
            .Select(_ => new Person())
            .Select(x =>
            {
                var id = string.Concat(x.Random.Digits(5));
                return new SampleEmployee(x.Random.Uuid(), id, x.FirstName, x.LastName, "en",
                    [SampleClockGroups[x.Random.Number(SampleClockGroups.Count - 1)].ExternalId],
                    id, id);
            })
            .ToList();

        //Remove any employees with duplicate external IDs
        var duplicateIds = data.GroupBy(x => x.Id).Where(x => x.Count() > 1).Select(x => x.Key).ToHashSet();
        data.RemoveAll(x => duplicateIds.Contains(x.Id));

        var itemsToSkip = DateTime.UtcNow.Hour * 2;
        return [.. data.Skip(itemsToSkip).Take(10)];
    }
}
