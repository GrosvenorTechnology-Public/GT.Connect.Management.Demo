using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>Sieve query parameters for the v1 device list endpoint (filters, sorts, page, pageSize, getChildren).</summary>
public class DeviceQueryParams : SieveQueryParams
{
    [AliasAs("getChildren")]
    public bool? GetChildren { get; set; }

    /// <summary>Sieve sort expression, e.g. "name" or "-lastModifiedOn".</summary>
    [AliasAs("sorts")]
    public string? Sorts { get; set; }

    public DeviceQueryParams() : base()
    {

    }

    public DeviceQueryParams(Filter filter) : base(filter)
    {
    }

}

