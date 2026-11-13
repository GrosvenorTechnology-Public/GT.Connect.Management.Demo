using Gridify;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>People endpoints for job categories, job codes and person-to-job-code membership.</summary>
public interface IConnectApiPeopleJobs
{
    // ---- Job categories ----

    /// <summary>Lists the job categories on a company node (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/jobCategories")]
    Task<ApiResponse<PagingResponse<JobCategoryResponse>>> GetJobCategories(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a job category by id (v2). Returns 404 when not found.</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/jobCategories/{jobCategoryId}")]
    Task<ApiResponse<JobCategoryResponse>> GetJobCategory(Guid tenantId, int nodeId, Guid jobCategoryId);

    /// <summary>Creates a job category (201). Body TenantId and NodeId must match the route.</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/jobCategories")]
    Task<JobCategoryResponse> AddJobCategory(Guid tenantId, int nodeId, [Body] JobCategoryRequest jobCategory);

    /// <summary>Updates a job category. Body Id must match <paramref name="jobCategoryId"/>.</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/jobCategories/{jobCategoryId}")]
    Task<JobCategoryResponse> UpdateJobCategory(Guid tenantId, int nodeId, Guid jobCategoryId, [Body] JobCategoryUpdateRequest jobCategory);

    /// <summary>Deletes a job category.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/jobCategories/{jobCategoryId}")]
    Task<bool> DeleteJobCategory(Guid tenantId, int nodeId, Guid jobCategoryId);

    /// <summary>Lists the people that hold a job code in the job category (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/jobCategories/{jobCategoryId}/members")]
    Task<ApiResponse<PagingResponse<PersonResponse>>> GetPeopleInJobCategory(Guid tenantId, int nodeId, Guid jobCategoryId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists the job codes in a job category (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/jobCategories/{jobCategoryId}/jobCodes")]
    Task<ApiResponse<PagingResponse<JobCodeResponse>>> GetJobCodesInJobCategory(Guid tenantId, int nodeId, Guid jobCategoryId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists the job categories of a person's job codes (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/jobCategories/member/{personId}")]
    Task<ApiResponse<PagingResponse<JobCategoryResponse>>> GetJobCategoriesForPerson(Guid tenantId, int nodeId, Guid personId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists the job categories available to a device (machine client only).</summary>
    [Get("/api/tenants/{tenantId}/people/jobCategories/{deviceId}")]
    Task<List<JobCategoryResponse>> GetJobCategoriesForDevice(Guid tenantId, Guid deviceId);

    // ---- Job categories: machine-to-machine (/internal/) routes, used by the Integration Edge ----

    /// <summary>Lists the job categories on a company node via the machine-client route (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/internal/jobCategories")]
    Task<ApiResponse<PagingResponse<JobCategoryResponse>>> GetJobCategoriesInternal(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Creates a job category via the machine-client route (201).</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/internal/jobCategories")]
    Task<JobCategoryResponse> AddJobCategoryInternal(Guid tenantId, int nodeId, [Body] JobCategoryRequest jobCategory);

    /// <summary>Updates a job category via the machine-client route. Body Id must match <paramref name="jobCategoryId"/>.</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/internal/jobCategories/{jobCategoryId}")]
    Task<JobCategoryResponse> UpdateJobCategoryInternal(Guid tenantId, int nodeId, Guid jobCategoryId, [Body] JobCategoryUpdateRequest jobCategory);

    /// <summary>Deletes a job category via the machine-client route.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/internal/jobCategories/{jobCategoryId}")]
    Task<bool> DeleteJobCategoryInternal(Guid tenantId, int nodeId, Guid jobCategoryId);

    // ---- Job codes ----

    /// <summary>Lists the job codes on a company node (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/jobCodes")]
    Task<ApiResponse<PagingResponse<JobCodeResponse>>> GetJobCodes(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Gets a job code by id. Returns 404 when not found.</summary>
    [Get("/api/tenants/{tenantId}/people/{nodeId}/jobCodes/{jobCodeId}")]
    Task<ApiResponse<JobCodeResponse>> GetJobCode(Guid tenantId, int nodeId, Guid jobCodeId);

    /// <summary>Creates a job code (201). Body TenantId and NodeId must match the route.</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/jobCodes")]
    Task<JobCodeResponse> AddJobCode(Guid tenantId, int nodeId, [Body] JobCodeRequest jobCode);

    /// <summary>Updates a job code. Body Id must match <paramref name="jobCodeId"/>.</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/jobCodes/{jobCodeId}")]
    Task<JobCodeResponse> UpdateJobCode(Guid tenantId, int nodeId, Guid jobCodeId, [Body] JobCodeUpdateRequest jobCode);

    /// <summary>Deletes a job code.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/jobCodes/{jobCodeId}")]
    Task<bool> DeleteJobCode(Guid tenantId, int nodeId, Guid jobCodeId);

    /// <summary>Lists the people holding a job code (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/jobCodes/{jobCodeId}/members")]
    Task<ApiResponse<PagingResponse<PersonResponse>>> GetPeopleWithJobCode(Guid tenantId, int nodeId, Guid jobCodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists the job categories containing a job code (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/jobCodes/{jobCodeId}/jobCategories")]
    Task<ApiResponse<PagingResponse<JobCategoryResponse>>> GetJobCategoriesWithJobCode(Guid tenantId, int nodeId, Guid jobCodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists the job codes held by a person (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/jobCodes/member/{personId}")]
    Task<ApiResponse<PagingResponse<JobCodeResponse>>> GetJobCodesForPerson(Guid tenantId, int nodeId, Guid personId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists the job codes (with category order) available to a device (machine client only).</summary>
    [Get("/api/tenants/{tenantId}/people/jobCodes/{deviceId}")]
    Task<List<JobCodeWithOrderResponse>> GetJobCodesForDevice(Guid tenantId, Guid deviceId);

    // ---- Job codes: machine-to-machine (/internal/) routes, used by the Integration Edge ----

    /// <summary>Lists the job codes on a company node via the machine-client route (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/internal/jobCodes")]
    Task<ApiResponse<PagingResponse<JobCodeResponse>>> GetJobCodesInternal(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Creates job codes in a batch (machine client). 201 when all succeed, 207 with failures listed, 204 when nothing was processed.</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/internal/jobCodes/batch")]
    Task<ApiResponse<JobCodeBatchResponse>> AddJobCodesBatch(Guid tenantId, int nodeId, [Body] List<JobCodeRequest> jobCodes);

    /// <summary>Updates job codes in a batch (machine client). 201 when all succeed, 207 with failures listed, 204 when nothing was processed.</summary>
    [Put("/api/tenants/{tenantId}/people/{nodeId}/internal/jobCodes/batch")]
    Task<ApiResponse<JobCodeBatchResponse>> UpdateJobCodesBatch(Guid tenantId, int nodeId, [Body] List<JobCodeUpdateRequest> jobCodes);

    /// <summary>Deletes job codes by id in a batch (machine client). 200 when all succeed, 207 with failures listed.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/internal/jobCodes/batch")]
    Task<ApiResponse<JobCodeDeleteBatchResponse>> DeleteJobCodesBatch(Guid tenantId, int nodeId, [Body] List<Guid> jobCodeIds);

    // ---- Person to job code ----

    /// <summary>Assigns a job code to a person (201, returns the job code).</summary>
    [Post("/api/tenants/{tenantId}/people/{nodeId}/jobCodes/{jobCodeId}/person/{personId}")]
    Task<JobCodeResponse> AddPersonToJobCode(Guid tenantId, int nodeId, Guid jobCodeId, Guid personId);

    /// <summary>Removes a job code from a person.</summary>
    [Delete("/api/tenants/{tenantId}/people/{nodeId}/jobCodes/{jobCodeId}/person/{personId}")]
    Task<bool> RemovePersonFromJobCode(Guid tenantId, int nodeId, Guid jobCodeId, Guid personId);

    /// <summary>Lists all person-to-job-code links on a company node (v2, Gridify paged).</summary>
    [Headers("X-Api-Version: 2.0")]
    [Get("/api/tenants/{tenantId}/people/{nodeId}/jobCodes/members")]
    Task<ApiResponse<PagingResponse<PersonToJobCodeResponse>>> GetPersonJobCodeMembers(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);
}
