using GT.Connect.Management.Demo.ConnectApi.Auth;

namespace GT.Connect.Management.Demo.Core;

public class ApiTestBase
{
    protected async Task<IConnectApi> GetClient()
    {
        //The token endpoint is anonymous, so is called with a client that has no auth handler
        var authApi = RestService.For<IConnectApiAuth>(
            new HttpClient()
            {
                BaseAddress = new Uri(Settings.GtConnectUrl)
            });

        var tokenResponse = await authApi.GetAuthToken(Settings.TenantId, Settings.TokenRequest);

        //You should cache the token at this point, this is not done here to keep the example simple
        Assert.Equal("Bearer", tokenResponse.Token_Type);

        return RestService.For<IConnectApi>(
            new HttpClient(new AuthHeaderHandler(tokenResponse.Access_Token))
            {
                BaseAddress = new Uri(Settings.GtConnectUrl)
            });
    }
}
