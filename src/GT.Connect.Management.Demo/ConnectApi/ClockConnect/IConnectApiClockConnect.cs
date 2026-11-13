using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.ClockConnect;

public interface IConnectApiClockConnect
{
    /// <summary>Validates a device JWT against the device's serial number. Requires DeviceView on the device's node.</summary>
    [Post("/api/tenants/{tenantId}/clock-connect/device/{serialNumber}/token/validate")]
    Task<ValidateTokenResponse> ValidateDeviceToken(Guid tenantId, string serialNumber, [Body] ValidateTokenRequest request);

    /// <summary>Publishes (adds or replaces) a ClockConnect document for a node or device (machine client only).</summary>
    [Post("/api/tenants/{tenantId}/clock-connect/Documents/command")]
    Task<IApiResponse> PublishClockConnectDocument(Guid tenantId, [Body] PublishClockConnectDocumentCommand command);

    /// <summary>Lists the documents published on the node and its child nodes, paged with Gridify (v2).</summary>
    [Get("/api/tenants/{tenantId}/clock-connect/{nodeId}/Documents")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<PublishedDocumentEntry>>> GetPublishedClockConnectDocuments(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Removes all published company documents from the node and walks down the tree. 400 (with message) on domain errors.</summary>
    [Delete("/api/tenants/{tenantId}/clock-connect/{nodeId}/Documents/walkdown")]
    Task<IApiResponse> DeleteAllPublishedCompanyDocuments(Guid tenantId, int nodeId);

    /// <summary>Removes a published company document resource from the node and walks down the tree. 400 (with message) on domain errors.</summary>
    [Delete("/api/tenants/{tenantId}/clock-connect/{nodeId}/Documents/resource/{resource}/walkdown")]
    Task<IApiResponse> DeletePublishedCompanyDocument(Guid tenantId, int nodeId, string resource);

    /// <summary>Removes a published document resource from a node. 400 (with message) on domain errors.</summary>
    [Delete("/api/tenants/{tenantId}/clock-connect/{nodeId}/Documents/resource/{resource}")]
    Task<IApiResponse> DeletePublishedDocumentOnNode(Guid tenantId, int nodeId, string resource);

    /// <summary>Removes a published document resource from a device. 400 (with message) on domain errors.</summary>
    [Delete("/api/tenants/{tenantId}/clock-connect/{deviceId}/Documents/resource/{resource}")]
    Task<IApiResponse> DeletePublishedDocumentOnDevice(Guid tenantId, Guid deviceId, string resource);
}

public record ValidateTokenRequest(string Token);

public record ValidateTokenResponse(bool IsValid);

/// <summary>Publish request: set either <c>NodeId</c> or <c>DeviceId</c>. <c>UserId</c> is the user the change is attributed to.</summary>
public record PublishClockConnectDocumentCommand
(
    Guid TenantId,
    Guid UserId,
    Guid EntityId,
    int? NodeId,
    Guid? DeviceId,
    Guid FileTypeId,
    string Data
);

public record PublishedDocumentEntry
(
    Guid Id,
    Guid TenantId,
    Guid? DeviceId,
    int? NodeId,
    Guid FileTypeId,
    string ResourceName,
    string[]? DeviceTypes,
    string? ETag
);
