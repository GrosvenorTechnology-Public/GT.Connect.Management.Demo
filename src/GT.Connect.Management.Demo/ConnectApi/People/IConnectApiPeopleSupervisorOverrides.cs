using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>People-service supervisor overrides (the per-person queries live on <see cref="IConnectApiPeople"/>).</summary>
public interface IConnectApiPeopleSupervisorOverrides
{
    /// <summary>Gets the supervisor overrides of a company (Sieve filters/sorts/paging supported).</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/SupervisorOverride/company/{companyId}")]
    Task<ApiResponse<List<SupervisorOverride>>> GetSupervisorOverridesForCompany(Guid tenantId, int nodeId, Guid companyId, [Query] SieveQueryParams? query = null);

    /// <summary>Gets the supervisor overrides created on a device.</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/SupervisorOverride/device/{deviceId}")]
    Task<ApiResponse<List<SupervisorOverride>>> GetSupervisorOverridesForDevice(Guid tenantId, int nodeId, Guid deviceId);

    /// <summary>Gets the supervisor overrides created on a device, by device serial number.</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/SupervisorOverride/device/{serialNumber}")]
    Task<ApiResponse<List<SupervisorOverride>>> GetSupervisorOverridesForDeviceSerialNumber(Guid tenantId, int nodeId, string serialNumber);

    /// <summary>Creates a supervisor override.</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/SupervisorOverride")]
    Task<ApiResponse<SupervisorOverride>> CreateSupervisorOverride(Guid tenantId, int nodeId, [Body] SupervisorOverrideAddRequest request);

    /// <summary>Updates a supervisor override (requires PeopleAdmin).</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/SupervisorOverride/{supervisorOverrideId}")]
    Task<ApiResponse<SupervisorOverride>> UpdateSupervisorOverride(Guid tenantId, int nodeId, Guid supervisorOverrideId, [Body] SupervisorOverrideUpdateRequest request);

    /// <summary>Sets the status of a supervisor override (requires PeopleAdmin). The status is sent as a JSON string.</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/SupervisorOverride/{supervisorOverrideId}/status")]
    Task<ApiResponse<SupervisorOverride>> UpdateSupervisorOverrideStatus(Guid tenantId, int nodeId, Guid supervisorOverrideId, [Body] SupervisorOverrideStatus status);

    /// <summary>Deletes a supervisor override (requires PeopleAdmin).</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/SupervisorOverride/{supervisorOverrideId}")]
    Task<IApiResponse> DeleteSupervisorOverride(Guid tenantId, int nodeId, Guid supervisorOverrideId);
}
