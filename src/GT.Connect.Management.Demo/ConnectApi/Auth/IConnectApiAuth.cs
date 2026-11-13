using GT.Connect.Management.Demo.ConnectApi.Organisation;
using Refit;
using System.Text.Json.Serialization;

namespace GT.Connect.Management.Demo.ConnectApi.Auth;

/// <summary>
/// Anonymous endpoints. Registered without a token handler.
/// </summary>
public interface IConnectApiAuth
{
    [Post("/api/tenants/{tenantId}/auth/token")]
    Task<TokenResponse> GetAuthToken(Guid tenantid, [Body] TokenRequest person);

    /// <summary>Accepts a pending system-user invite by email (anonymous, not tenant-scoped). 400 when no invite matches.</summary>
    [Post("/api/org/users/acceptinvite")]
    Task<UserResponse> AcceptUserInvite([Body] AcceptInviteRequest request);
}

public record TokenRequest (
    [property: JsonPropertyName("client_id")]
    string ClientId,
    [property: JsonPropertyName("client_secret")]
    string ClientSecret,
    [property: JsonPropertyName("grant_type")]
    string GrantType,
    [property: JsonPropertyName("username")]
    string? Username,
    [property: JsonPropertyName("password")]
    string? Password);

public record TokenResponse (string Access_Token, string Token_Type, DateTimeOffset Token_Expiry_UTC);
