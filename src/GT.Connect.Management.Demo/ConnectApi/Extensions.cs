using Gridify;
using Refit;
using System.Text.Json;

namespace GT.Connect.Management.Demo.ConnectApi;

/// <summary>
/// Helpers for handling GtConnect API responses.
/// Several collection endpoints return HTTP 204 No Content rather than an empty JSON list.
/// </summary>
public static class Extensions
{
    public static async Task<T?> Refit204FirstOrDefaultAsync<T>(this ApiResponse<List<T>> response) where T : class
    {
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return default;
        }
        else
        {
            await response.EnsureSuccessStatusCodeAsync();
            return response.Content?.FirstOrDefault();
        }
    }

    public static async Task<List<T>> Refit204ListOrEmptyListAsync<T>(this ApiResponse<List<T>> response) where T : class
    {
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return [];
        }
        else
        {
            await response.EnsureSuccessStatusCodeAsync();
            return response.Content ?? [];
        }
    }

    public static async Task Refit204EnsureSuccessStatusCodeAsync<T>(this ApiResponse<List<T>> response) where T : class
    {
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return;
        }
        else
        {
            await response.EnsureSuccessStatusCodeAsync();
            return;
        }
    }

    public static async Task<List<T>> GetExtendedDataEntities<T>(this IConnectApi connectApi, Guid tenantId, int nodeId, string dataTypeName, GridifyQuery? gridifyModel = null)
    {
        var response = await connectApi.GetExtendedDataItems(tenantId, nodeId, dataTypeName, gridifyModel);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = response.Content;
            var statusCode = response.StatusCode;

            throw new HttpRequestException($"Failed to get extended data entities. StatusCode: {statusCode}, Error: {errorContent}");
        }

        if (response?.Content == null || response.Content.Data == null)
        {
            return [];
        }

        return response.Content.Data
            .Select(x =>
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(x.EntityData);
                }
                catch (Exception ex)
                {
                    throw new JsonException($"Failed to deserialize entity data to type {typeof(T).Name}: {ex.Message}", ex);
                }
            })
            .Where(x => x != null)
            .ToList()!;
    }
}
