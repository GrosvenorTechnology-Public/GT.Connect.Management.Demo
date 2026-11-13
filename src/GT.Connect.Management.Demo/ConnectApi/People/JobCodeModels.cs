using GT.Connect.Management.Demo.ConnectApi.Core;

namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>Job category (server <c>JobCategoryDto</c>).</summary>
public record JobCategoryResponse(Guid Id, Guid CompanyId, Guid TenantId, int NodeId, string Name, int Order, string? ExternalId,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

/// <summary>Body for creating a job category (server <c>JobCategoryAddCommand</c>). TenantId and NodeId must match the route.</summary>
public record JobCategoryRequest(Guid TenantId, int NodeId, Guid CompanyId, int Order, string Name, string? ExternalId);

/// <summary>Body for updating a job category (server <c>JobCategoryUpdateCommand</c>). Id must match the route id.</summary>
public record JobCategoryUpdateRequest(Guid TenantId, Guid Id, int NodeId, Guid CompanyId, int Order, string Name, string? ExternalId);

/// <summary>Job code (server <c>JobCodeDto</c>).</summary>
public record JobCodeResponse(Guid Id, Guid JobCategoryId, Guid CompanyId, Guid TenantId, int NodeId, string Name, string Code,
    string? ExternalId, string? JobCategoryExternalId,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

/// <summary>Job code with its category order, as returned for a device (server <c>JobCodeWithOrderDto</c>).</summary>
public record JobCodeWithOrderResponse(Guid Id, Guid JobCategoryId, Guid CompanyId, Guid TenantId, int NodeId, string Name, string Code,
    int Order, string? ExternalId,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

/// <summary>Body for creating a job code (server <c>JobCodeAddCommand</c>); also the item type of the batch create.</summary>
public record JobCodeRequest(Guid TenantId, Guid JobCategoryId, int NodeId, Guid CompanyId, string Name, string Code, string? ExternalId);

/// <summary>Body for updating a job code (server <c>JobCodeUpdateCommand</c>); also the item type of the batch update. Id must match the route id.</summary>
public record JobCodeUpdateRequest(Guid TenantId, Guid Id, Guid JobCategoryId, int NodeId, Guid CompanyId, string Name, string Code, string? ExternalId);

/// <summary>Result of a job code batch create/update (server <c>JobCodeBatchDto</c>).</summary>
public record JobCodeBatchResponse(List<JobCodeResponse> JobCodes, List<JobCodeBatchFailure> Failures);

/// <summary>A job code that failed in a batch create/update (server <c>FailedJobCodeDto</c>).</summary>
public record JobCodeBatchFailure(string? ExternalId, string? Name, string? ErrorMessage);

/// <summary>Result of a job code batch delete (server <c>DeleteBatchResponseDto</c>).</summary>
public record JobCodeDeleteBatchResponse(int Deleted, List<JobCodeDeleteBatchFailure> Failures);

/// <summary>A job code id that failed in a batch delete (server <c>FailedDataIdDto</c>).</summary>
public record JobCodeDeleteBatchFailure(Guid Id, string? ErrorCode);

/// <summary>Person-to-job-code link (server <c>PersonToJobCodeDto</c>).</summary>
public record PersonToJobCodeResponse(Guid PersonId, Guid JobCodeId);
