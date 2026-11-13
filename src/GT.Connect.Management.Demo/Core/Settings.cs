using GT.Connect.Management.Demo.ConnectApi.Auth;
using Microsoft.Extensions.Configuration;

namespace GT.Connect.Management.Demo.Core;

/// <summary>
/// Connection details for the GtConnect API.
/// Values are read from .NET user secrets, or from environment variables prefixed with <c>GTCONNECT_</c>.
/// See README.md for the list of keys.
/// </summary>
public static class Settings
{
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddUserSecrets(typeof(Settings).Assembly, optional: true)
        .AddEnvironmentVariables("GTCONNECT_")
        .Build();

    /// <summary>The base url of the GtConnect API, e.g. https://api.example.gtconnect.cloud</summary>
    public static string GtConnectUrl => Required("Url");

    /// <summary>Your GtConnect tenant id.</summary>
    public static Guid TenantId => Guid.Parse(Required("TenantId"));

    /// <summary>
    /// The request used to obtain a bearer token from the GtConnect auth endpoint.
    /// If a Username is configured a password grant is used, otherwise machine (client_credentials) credentials.
    /// </summary>
    public static TokenRequest TokenRequest => string.IsNullOrEmpty(Configuration["Username"])
        ? new(Required("ClientId"), Required("ClientSecret"), "client_credentials", null, null)
        : new(Required("ClientId"), Required("ClientSecret"), "password", Required("Username"), Required("Password"));

    private static string Required(string key) =>
        Configuration[key] is { Length: > 0 } value
            ? value
            : throw new InvalidOperationException(
                $"GtConnect setting '{key}' is not configured. Set it with " +
                $"'dotnet user-secrets set {key} <value>' or the GTCONNECT_{key} environment variable.");
}
