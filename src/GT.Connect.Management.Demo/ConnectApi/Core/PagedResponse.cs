namespace GT.Connect.Management.Demo.ConnectApi.Core;

public record PagedResponse<T>(List<T> Data, int Count);
