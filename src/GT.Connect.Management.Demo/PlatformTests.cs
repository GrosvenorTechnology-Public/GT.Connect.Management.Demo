using GT.Connect.Management.Demo.ConnectApi.Auth;

namespace GT.Connect.Management.Demo;

public class PlatformTests
{
    [Fact]
    public async Task GetAccessToken()
    {
        var authApi = RestService.For<IConnectApiAuth>(
            new HttpClient()
            {
                BaseAddress = new Uri(Settings.GtConnectUrl)
            });

        var response = await authApi.GetAuthToken(Settings.TenantId, Settings.TokenRequest);

        Assert.Equal("Bearer", response.Token_Type);
    }

}
