using System.Text.Json.Serialization;
using GT.Connect.Management.Demo.ConnectApi.Core;
using Gridify;
using Refit;

namespace GT.Connect.Management.Demo.ConnectApi.IntegrationEdge;

public interface IConnectApiIntegrationEdge
{
    [Get("/api/tenants/{tenantId}/integrationedge/exportcomponents")]
    Task<ApiResponse<List<ExportComponentResponse>>> GetExportComponents(Guid tenantId);

    [Get("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}")]
    Task<ApiResponse<ExportComponentResponse>> GetExportComponent(Guid tenantId, Guid componentId);

    [Get("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}/properties")]
    Task<ApiResponse<List<ExportComponentPropertyResponse>>> GetExportComponentProperties(Guid tenantId, Guid componentId);

    [Get("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}/jobproperties")]
    Task<ApiResponse<List<ExportComponentJobPropertyResponse>>> GetExportComponentJobProperties(Guid tenantId, Guid componentId);

    [Obsolete("The v1 server endpoint is deprecated. Use GetExportJobsPagedV2 instead.")]
    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/exportjobs")]
    Task<ApiResponse<List<ExportJobResponse>>> GetExportJobs(Guid tenantId, int nodeId);

    [Obsolete("Despite its name this calls the 'allproperties' endpoint without paging/filtering. Use GetExportJobsWithAllPropertiesV2 (or GetExportJobsPagedV2 for the plain list).")]
    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/exportjobs/allproperties")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagedResponse<ExportJobResponseV2>>> GetExportJobsV2(Guid tenantId, int nodeId);

    /// <summary>Lists the export jobs on a node, paged/filtered with Gridify (v2).</summary>
    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/exportjobs")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<ExportJobResponse>>> GetExportJobsPagedV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    /// <summary>Lists the export jobs on a node including their component and all job/component properties, paged with Gridify (v2).</summary>
    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/exportjobs/allproperties")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<ExportJobResponseV2>>> GetExportJobsWithAllPropertiesV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/exportjobs/{exportJobId}")]
    Task<ExportJobResponse> GetExportJob(Guid tenantId, int nodeId, Guid exportJobId);

    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/exportjobs/{exportJobId}/properties")]
    Task<ApiResponse<List<ExportJobPropertyResponse>>> GetExportJobProperties(Guid tenantId, int nodeId, Guid exportJobId);

    [Post("/api/tenants/{tenantId}/integrationedge/{nodeId}/exportjobs")]
    Task<ExportJobResponse> AddExportJob(Guid tenantId, int nodeId, [Body] AddExportJobRequest request);

    [Post("/api/tenants/{tenantId}/integrationedge/exportjob/{nodeId}/logtransaction")]
    [Headers("X-Api-Version: 2.0")]
    Task<IApiResponse> AddTransactionResultV2(Guid tenantId, int nodeId, [Body] AddExportJobTransactionStatusV2 request);

    [Get("/api/tenants/{tenantId}/integrationedge/importcomponents")]
    Task<ApiResponse<List<ImportComponentResponse>>> GetImportComponents(Guid tenantId);

    [Get("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}")]
    Task<ApiResponse<ImportComponentResponse>> GetImportComponent(Guid tenantId, Guid componentId);

    // The server has no node-scoped component properties route; nodeId is sent as an (ignored) query parameter.
    [Obsolete("The server has no node-scoped import component properties route; nodeId is ignored. Use GetImportComponentProperties(tenantId, componentId) instead.")]
    [Get("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/properties")]
    Task<ApiResponse<List<ImportComponentPropertyResponse>>> GetImportComponentProperties(Guid tenantId, int nodeId, Guid componentId);

    [Obsolete("Use GetImportComponentJobPropertiesByNodeId(tenantId, componentId, nodeId) instead.")]
    [Get("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/jobproperties/{nodeId}")]
    Task<ApiResponse<List<ImportComponentJobPropertyResponse>>> GetImportComponentJobProperties(Guid tenantId, int nodeId, Guid componentId);

    [Obsolete("The v1 server endpoint is deprecated. Use GetImportJobsV2 instead.")]
    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs")]
    Task<ApiResponse<List<ImportJobResponse>>> GetImportJobs(Guid tenantId, int nodeId);

    /// <summary>Lists the import jobs on a node, paged/filtered with Gridify (v2).</summary>
    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<ImportJobResponse>>> GetImportJobsV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs/{importJobId}")]
    Task<ImportJobResponse> GetImportJob(Guid tenantId, int nodeId, Guid importJobId);

    /// <summary>Lists the properties of an import job (204 when there are none, 404 when the job does not exist).</summary>
    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs/{importJobId}/properties")]
    Task<ApiResponse<List<ImportJobPropertyResponse>>> GetImportJobProperties(Guid tenantId, int nodeId, Guid importJobId);

    /// <summary>Regenerates the API key of an import job and returns the updated job (including the new ApiKey).</summary>
    [Post("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs/{importJobId}/regenerate-api-key")]
    Task<ApiResponse<ImportJobResponse>> RegenerateImportJobApiKey(Guid tenantId, int nodeId, Guid importJobId);

    [Post("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs")]
    Task<ImportJobResponse> AddImportJob(Guid tenantId, int nodeId, [Body] AddImportJobRequest request);

    [Post("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs/{importJobId}")]
    Task<bool> StartImportJob(Guid tenantId, int nodeId, Guid importJobId);

    // Server-Actions

    [Obsolete("The v1 server endpoint is deprecated. Use GetServerActionsV2 instead.")]
    [Get("/api/tenants/{tenantId}/integrationedge/serveractions/node/{nodeId}")]
    Task<ApiResponse<List<ServerActionResponse>>> GetServerActions(Guid tenantId, int nodeId, [Query] bool getChildren = false);

    /// <summary>Lists the server actions on a node (optionally including child nodes), paged/filtered with Gridify (v2).</summary>
    [Get("/api/tenants/{tenantId}/integrationedge/serveractions/node/{nodeId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<ServerActionResponse>>> GetServerActionsV2(Guid tenantId, int nodeId, [Query] bool getChildren = false, [Query] GridifyQuery? gridifyQuery = null);

    [Get("/api/tenants/{tenantId}/integrationedge/serveractions/{serverActionId}")]
    Task<ApiResponse<ServerActionResponse>> GetServerActionById(Guid tenantId, Guid serverActionId);

    [Post("/api/tenants/{tenantId}/integrationedge/serveractions")]
    Task<ApiResponse<ServerActionResponse>> AddServerAction(Guid tenantId, [Body] AddServerActionRequest request);

    [Put("/api/tenants/{tenantId}/integrationedge/serveractions/{serverActionId}")]
    Task<ApiResponse<ServerActionResponse>> UpdateServerAction(Guid tenantId, Guid serverActionId, [Body] UpdateServerActionRequest request);

    [Delete("/api/tenants/{tenantId}/integrationedge/serveractions/{serverActionId}")]
    Task<IApiResponse> DeleteServerAction(Guid tenantId, Guid serverActionId);

    [Obsolete("The server resolves the device from the X-Gt-Serialnumber header, which this overload does not send, so it always fails with 400. Use TriggerServerAction(tenantId, actionName, serialNumber, request).")]
    [Post("/api/tenants/{tenantId}/serveractions/serveractionrequest/{actionName}")]
    Task<ApiResponse<ServerActionRequestResponse>> TriggerServerAction(Guid tenantId, string actionName, [Body] ServerActionDeviceRequest request);

    /// <summary>Executes a server action on behalf of the device with the given serial number (machine client only). The action executed is request.Action; actionName is informational.</summary>
    [Post("/api/tenants/{tenantId}/serveractions/serveractionrequest/{actionName}")]
    Task<ApiResponse<ServerActionRequestResponse>> TriggerServerAction(Guid tenantId, string actionName, [Header("X-Gt-Serialnumber")] string serialNumber, [Body] ServerActionDeviceRequest request);

    // Server-Action properties

    /// <summary>Lists the properties of a server action (404 when the server action does not exist).</summary>
    [Get("/api/tenants/{tenantId}/integrationedge/serveractions/{serverActionId}/properties")]
    Task<ApiResponse<List<ServerActionPropertyResponse>>> GetServerActionProperties(Guid tenantId, Guid serverActionId);

    /// <summary>Gets a single server action property.</summary>
    [Get("/api/tenants/{tenantId}/integrationedge/serveractions/{serverActionId}/properties/{propertyId}")]
    Task<ApiResponse<ServerActionPropertyResponse>> GetServerActionPropertyById(Guid tenantId, Guid serverActionId, Guid propertyId);

    /// <summary>Adds a property (header, query string or body value) to a server action; returns 201 with the created property.</summary>
    [Post("/api/tenants/{tenantId}/integrationedge/serveractions/{serverActionId}/properties")]
    Task<ApiResponse<ServerActionPropertyResponse>> AddServerActionProperty(Guid tenantId, Guid serverActionId, [Body] AddServerActionPropertyRequest request);

    /// <summary>Updates a server action property.</summary>
    [Put("/api/tenants/{tenantId}/integrationedge/serveractions/{serverActionId}/properties/{propertyId}")]
    Task<ApiResponse<ServerActionPropertyResponse>> UpdateServerActionProperty(Guid tenantId, Guid serverActionId, Guid propertyId, [Body] UpdateServerActionPropertyRequest request);

    /// <summary>Deletes a server action property (204).</summary>
    [Delete("/api/tenants/{tenantId}/integrationedge/serveractions/{serverActionId}/properties/{propertyId}")]
    Task<IApiResponse> DeleteServerActionProperty(Guid tenantId, Guid serverActionId, Guid propertyId);

    //Streamed NDJSON response of ServerActionLogResponse items
    [Post("/api/tenants/{tenantId}/integrationedge/serveractions/logs/stream")]
    Task<HttpResponseMessage> StreamServerActionLogs(Guid tenantId, [Body] ServerActionLogRequest request);

    // Discovery

    [Get("/api/tenants/{tenantId}/integrationedge/discovery/keys")]
    Task<HttpResponseMessage> GetDiscoveryKeys(Guid tenantId);

    // Import components

    [Get("/api/tenants/{tenantId}/integrationedge/importcomponents/names")]
    Task<ApiResponse<List<ComponentNameResponse>>> GetImportComponentNames(Guid tenantId);

    [Post("/api/tenants/{tenantId}/integrationedge/importcomponents")]
    Task<ApiResponse<ImportComponentResponse>> AddImportComponent(Guid tenantId, [Body] AddImportComponentRequest request);

    [Put("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}")]
    Task<ApiResponse<ImportComponentResponse>> UpdateImportComponent(Guid tenantId, Guid componentId, [Body] UpdateImportComponentRequest request);

    [Delete("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}")]
    Task<IApiResponse> DeleteImportComponent(Guid tenantId, Guid componentId);

    [Get("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/properties")]
    Task<ApiResponse<List<ImportComponentPropertyResponse>>> GetImportComponentProperties(Guid tenantId, Guid componentId);

    [Get("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/properties/{propertyId}")]
    Task<ApiResponse<ImportComponentPropertyResponse>> GetImportComponentPropertyById(Guid tenantId, Guid componentId, Guid propertyId);

    [Post("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/properties")]
    Task<ApiResponse<ImportComponentPropertyResponse>> AddImportComponentProperty(Guid tenantId, Guid componentId, [Body] AddImportComponentPropertyRequest request);

    [Put("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/properties/{propertyId}")]
    Task<ApiResponse<ImportComponentPropertyResponse>> UpdateImportComponentProperty(Guid tenantId, Guid componentId, Guid propertyId, [Body] UpdateImportComponentPropertyRequest request);

    [Delete("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/properties/{propertyId}")]
    Task<IApiResponse> DeleteImportComponentProperty(Guid tenantId, Guid componentId, Guid propertyId);

    [Get("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/jobproperties")]
    Task<ApiResponse<List<ImportComponentJobPropertyResponse>>> GetImportComponentJobProperties(Guid tenantId, Guid componentId);

    [Get("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/jobproperties/{nodeId}")]
    Task<ApiResponse<List<ImportComponentJobPropertyResponse>>> GetImportComponentJobPropertiesByNodeId(Guid tenantId, Guid componentId, int nodeId);

    [Get("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/jobproperties/{propertyId}")]
    Task<ApiResponse<ImportComponentJobPropertyResponse>> GetImportComponentJobPropertyById(Guid tenantId, Guid componentId, Guid propertyId);

    [Post("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/jobproperties")]
    Task<ApiResponse<ImportComponentJobPropertyResponse>> AddImportComponentJobProperty(Guid tenantId, Guid componentId, [Body] AddImportComponentJobPropertyRequest request);

    [Put("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/jobproperties/{propertyId}")]
    Task<ApiResponse<ImportComponentJobPropertyResponse>> UpdateImportComponentJobProperty(Guid tenantId, Guid componentId, Guid propertyId, [Body] UpdateImportComponentJobPropertyRequest request);

    [Put("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/jobproperties/node/{nodeId}/{propertyId}")]
    Task<ApiResponse<ImportComponentJobPropertyResponse>> UpdateImportComponentJobPropertyByNodeId(Guid tenantId, Guid componentId, int nodeId, Guid propertyId, [Body] UpdateImportComponentJobPropertyRequest request);

    [Delete("/api/tenants/{tenantId}/integrationedge/importcomponents/{componentId}/jobproperties/{propertyId}")]
    Task<IApiResponse> DeleteImportComponentJobProperty(Guid tenantId, Guid componentId, Guid propertyId);

    // Export components

    [Get("/api/tenants/{tenantId}/integrationedge/exportcomponents/names")]
    Task<ApiResponse<List<ComponentNameResponse>>> GetExportComponentNames(Guid tenantId);

    [Post("/api/tenants/{tenantId}/integrationedge/exportcomponents")]
    Task<ApiResponse<ExportComponentResponse>> AddExportComponent(Guid tenantId, [Body] AddExportComponentRequest request);

    [Put("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}")]
    Task<ApiResponse<ExportComponentResponse>> UpdateExportComponent(Guid tenantId, Guid componentId, [Body] UpdateExportComponentRequest request);

    [Delete("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}")]
    Task<IApiResponse> DeleteExportComponent(Guid tenantId, Guid componentId);

    [Get("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}/properties/{propertyId}")]
    Task<ApiResponse<ExportComponentPropertyResponse>> GetExportComponentPropertyById(Guid tenantId, Guid componentId, Guid propertyId);

    [Post("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}/properties")]
    Task<ApiResponse<ExportComponentPropertyResponse>> AddExportComponentProperty(Guid tenantId, Guid componentId, [Body] AddExportComponentPropertyRequest request);

    [Put("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}/properties/{propertyId}")]
    Task<ApiResponse<ExportComponentPropertyResponse>> UpdateExportComponentProperty(Guid tenantId, Guid componentId, Guid propertyId, [Body] UpdateExportComponentPropertyRequest request);

    [Delete("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}/properties/{propertyId}")]
    Task<IApiResponse> DeleteExportComponentProperty(Guid tenantId, Guid componentId, Guid propertyId);

    [Get("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}/jobproperties/{nodeId}")]
    Task<ApiResponse<List<ExportComponentJobPropertyResponse>>> GetExportComponentJobPropertiesByNodeId(Guid tenantId, Guid componentId, int nodeId);

    [Get("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}/jobproperties/{propertyId}")]
    Task<ApiResponse<ExportComponentJobPropertyResponse>> GetExportComponentJobPropertyById(Guid tenantId, Guid componentId, Guid propertyId);

    [Post("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}/jobproperties")]
    Task<ApiResponse<ExportComponentJobPropertyResponse>> AddExportComponentJobProperty(Guid tenantId, Guid componentId, [Body] AddExportComponentJobPropertyRequest request);

    [Put("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}/jobproperties/{propertyId}")]
    Task<ApiResponse<ExportComponentJobPropertyResponse>> UpdateExportComponentJobProperty(Guid tenantId, Guid componentId, Guid propertyId, [Body] UpdateExportComponentJobPropertyRequest request);

    [Put("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}/jobproperties/node/{nodeId}/{propertyId}")]
    Task<ApiResponse<ExportComponentJobPropertyResponse>> UpdateExportComponentJobPropertyByNodeId(Guid tenantId, Guid componentId, int nodeId, Guid propertyId, [Body] UpdateExportComponentJobPropertyRequest request);

    [Delete("/api/tenants/{tenantId}/integrationedge/exportcomponents/{componentId}/jobproperties/{propertyId}")]
    Task<IApiResponse> DeleteExportComponentJobProperty(Guid tenantId, Guid componentId, Guid propertyId);

    // Import jobs

    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs/all")]
    Task<ApiResponse<List<ImportJobResponse>>> GetImportJobsFromNode(Guid tenantId, int nodeId);

    [Put("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs/{importJobId}")]
    Task<ApiResponse<ImportJobResponse>> UpdateImportJob(Guid tenantId, int nodeId, Guid importJobId, [Body] UpdateImportJobRequest request);

    [Delete("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs/{importJobId}")]
    Task<IApiResponse> DeleteImportJob(Guid tenantId, int nodeId, Guid importJobId);

    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs/{importJobId}/properties/{propertyId}")]
    Task<ApiResponse<ImportJobPropertyResponse>> GetImportJobPropertyById(Guid tenantId, int nodeId, Guid importJobId, Guid propertyId);

    [Post("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs/{importJobId}/properties")]
    Task<ApiResponse<ImportJobPropertyResponse>> AddImportJobProperty(Guid tenantId, int nodeId, Guid importJobId, [Body] AddImportJobPropertyRequest request);

    [Put("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs/{importJobId}/properties/{propertyId}")]
    Task<ApiResponse<ImportJobPropertyResponse>> UpdateImportJobProperty(Guid tenantId, int nodeId, Guid importJobId, Guid propertyId, [Body] UpdateImportJobPropertyRequest request);

    [Delete("/api/tenants/{tenantId}/integrationedge/{nodeId}/importjobs/{importJobId}/properties/{propertyId}")]
    Task<IApiResponse> DeleteImportJobProperty(Guid tenantId, int nodeId, Guid importJobId, Guid propertyId);

    // Export jobs

    [Put("/api/tenants/{tenantId}/integrationedge/{nodeId}/exportjobs/{exportJobId}")]
    Task<ApiResponse<ExportJobResponse>> UpdateExportJob(Guid tenantId, int nodeId, Guid exportJobId, [Body] UpdateExportJobRequest request);

    [Delete("/api/tenants/{tenantId}/integrationedge/{nodeId}/exportjobs/{exportJobId}")]
    Task<IApiResponse> DeleteExportJob(Guid tenantId, int nodeId, Guid exportJobId);

    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/exportjobs/{exportJobId}/properties/{propertyId}")]
    Task<ApiResponse<ExportJobPropertyResponse>> GetExportJobPropertyById(Guid tenantId, int nodeId, Guid exportJobId, Guid propertyId);

    [Post("/api/tenants/{tenantId}/integrationedge/{nodeId}/exportjobs/{exportJobId}/properties")]
    Task<ApiResponse<ExportJobPropertyResponse>> AddExportJobProperty(Guid tenantId, int nodeId, Guid exportJobId, [Body] AddExportJobPropertyRequest request);

    [Put("/api/tenants/{tenantId}/integrationedge/{nodeId}/exportjobs/{exportJobId}/properties/{propertyId}")]
    Task<ApiResponse<ExportJobPropertyResponse>> UpdateExportJobProperty(Guid tenantId, int nodeId, Guid exportJobId, Guid propertyId, [Body] UpdateExportJobPropertyRequest request);

    [Delete("/api/tenants/{tenantId}/integrationedge/{nodeId}/exportjobs/{exportJobId}/properties/{propertyId}")]
    Task<IApiResponse> DeleteExportJobProperty(Guid tenantId, int nodeId, Guid exportJobId, Guid propertyId);

    // Import process

    [Obsolete("The v1 server endpoint is deprecated. Use GetImportProcessesByNodeIdV2 instead.")]
    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/importprocess")]
    Task<ApiResponse<List<ImportProcessResponse>>> GetImportProcessesByNodeId(Guid tenantId, int nodeId);

    /// <summary>Lists import processes for a node (aggregated over child companies for partner nodes), paged with Gridify (v2). Filtering on currentState is not supported.</summary>
    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/importprocess")]
    [Headers("X-Api-Version: 2.0")]
    Task<ApiResponse<PagingResponse<ImportProcessResponse>>> GetImportProcessesByNodeIdV2(Guid tenantId, int nodeId, [Query] GridifyQuery? gridifyQuery = null);

    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/importprocess/issues")]
    Task<ApiResponse<List<ImportProcessResponse>>> GetImportProcessesWithIssues(Guid tenantId, int nodeId, [Query] bool forceCacheUpdate = false);

    /// <summary>Gets an import process with its messages (204 when not found).</summary>
    [Get("/api/tenants/{tenantId}/integrationedge/{nodeId}/importprocess/{importProcessId}")]
    Task<ApiResponse<ImportProcessDetailResponse>> GetImportProcessById(Guid tenantId, int nodeId, Guid importProcessId);

    /// <summary>Starts an import process for the node (machine client only) and returns the storage URI to upload the import files to.</summary>
    [Post("/api/tenants/{tenantId}/integrationedge/{nodeId}/importprocess/start")]
    Task<ApiResponse<ImportStartResponse>> StartImportProcess(Guid tenantId, int nodeId, [Body] StartImportProcessRequest request);

    // Progress and reporting

    [Post("/api/tenants/{tenantId}/integrationedge/progress/importing/{importProcessId}")]
    Task<IApiResponse> ReportImportProgress(Guid tenantId, Guid importProcessId, [Body] ImportProgressOutcomeRequest request);

    [Post("/api/tenants/{tenantId}/integrationedge/progress/processing/complete")]
    Task<IApiResponse> ReportImportProcessingComplete(Guid tenantId, [Body] ProcessNotificationRequest request);

    [Post("/api/tenants/{tenantId}/integrationedge/reporting/{importProcessId}/add")]
    Task<IApiResponse> AddImportProcessMessage(Guid tenantId, Guid importProcessId, [Body] ImportProcessMessageRequest request);

    // Transaction logging

    [Obsolete("The v1 server endpoint is deprecated. Use AddTransactionResultV2 instead.")]
    [Post("/api/tenants/{tenantId}/integrationedge/exportjob/{nodeId}/logtransaction")]
    Task<IApiResponse> AddTransactionResult(Guid tenantId, int nodeId, [Body] AddExportJobTransactionStatus request);

    //Streamed NDJSON responses of TransactionDeliveryLogResponse items
    [Post("/api/tenants/{tenantId}/integrationedge/{nodeId}/transactionresult")]
    [Headers("X-Api-Version: 2.0")]
    Task<HttpResponseMessage> GetTransactionDeliveryLogsByNodeId(Guid tenantId, int nodeId, [Body] TransactionDeliveryLogRequest request);

    [Post("/api/tenants/{tenantId}/integrationedge/{nodeId}/transactionresult/correlation/{correlationId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<HttpResponseMessage> GetTransactionDeliveryLogsByCorrelationId(Guid tenantId, int nodeId, Guid correlationId, [Body] TransactionDeliveryLogRequest request);

    [Post("/api/tenants/{tenantId}/integrationedge/{nodeId}/transactionresult/message/{messageId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<HttpResponseMessage> GetTransactionDeliveryLogsByMessageId(Guid tenantId, int nodeId, Guid messageId, [Body] TransactionDeliveryLogRequest request);

    [Post("/api/tenants/{tenantId}/integrationedge/{nodeId}/transactionresult/source/{sourceId}")]
    [Headers("X-Api-Version: 2.0")]
    Task<HttpResponseMessage> GetTransactionDeliveryLogsBySourceId(Guid tenantId, int nodeId, Guid sourceId, [Body] TransactionDeliveryLogRequest request);

    // Metrics

    [Get("/api/tenants/{tenantId}/integrationedge/metrics/aggregate-serveractions/{nodeId}")]
    Task<ApiResponse<List<ServerActionAggregateResponse>>> GetDailyAggregateServerActions(Guid tenantId, int nodeId, [Query] bool forceCacheUpdate = false);

    [Get("/api/tenants/{tenantId}/integrationedge/metrics/aggregate-serveractions/{nodeId}/failures")]
    Task<ApiResponse<List<ServerActionFailureResponse>>> GetFailedServerActionLogs(Guid tenantId, int nodeId, [Query] bool forceCacheUpdate = false);

    /// <summary>Gets the daily aggregate failure counts per server action for a node (204 when no data).</summary>
    [Get("/api/tenants/{tenantId}/integrationedge/metrics/aggregate-serveractions/{nodeId}/failurecounts")]
    Task<ApiResponse<List<ServerActionAggregateResponse>>> GetDailyAggregateServerActionFailureCounts(Guid tenantId, int nodeId, [Query] bool forceCacheUpdate = false);

    [Obsolete("Deprecated on the server; use the 'GetDailyAggregateTransactions...' endpoints of the People metrics API instead.")]
    [Get("/api/tenants/{tenantId}/integrationedge/metrics/aggregate-transactions/{nodeId}/allstates")]
    Task<ApiResponse<List<TransactionStatusesResponse>>> GetMonthlyTransactionStates(Guid tenantId, int nodeId, [Query] bool forceCacheUpdate = false);

    [Get("/api/tenants/{tenantId}/integrationedge/metrics/importJobsTotal/{nodeId}")]
    Task<ApiResponse<int>> GetImportJobsTotal(Guid tenantId, int nodeId);

    // Assistant

    [Obsolete("This endpoint no longer exists in the GtConnect API (the AI assistant was moved to the separate GTC Agent service, which is not exposed through the management API) and always returns 404.")]
    [Post("/api/tenants/{tenantId}/integrationedge/assistant/chat/{nodeId}")]
    Task<HttpResponseMessage> AssistantChat(Guid tenantId, int nodeId, [Body] AssistantChatRequest request);

    // Import scheduling

    [Post("/api/tenants/{tenantId}/integrationedge/ImportScheduler/pause")]
    Task<IApiResponse> PauseImportScheduling(Guid tenantId);

    [Post("/api/tenants/{tenantId}/integrationedge/ImportScheduler/resume")]
    Task<IApiResponse> ResumeImportScheduling(Guid tenantId);
}


public record ImportComponentResponse(Guid TenantId, Guid Id,
    string Name, string BaseUri, string Files, string PersonOverrides,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedBy, DateTimeOffset LastModifiedOn,
    int FailureDisableThreshold = 0)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedBy, LastModifiedOn);

public record ImportComponentPropertyResponse(Guid TenantId, Guid ImportComponentId, string Name,
    string FieldType, string Value, bool IsSecret,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedBy, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedBy, LastModifiedOn);

public record ImportComponentJobPropertyResponse(Guid TenantId, Guid ImportComponentId, string Name, string FieldType,
    string DefaultValue, bool IsSecret, bool IsOptional, string CustomValidator, string CustomValidatorMessage,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedBy, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedBy, LastModifiedOn);

public record ImportJobResponse(Guid TenantId, int NodeId, Guid Id,
    Guid ImportComponentId, string Name, string HostName, string Username, string Password,
    int UpdateFrequencyInterval, string FileExclusions, bool IsEnabled,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedBy, DateTimeOffset LastModifiedOn,
    string? Uri = null, DateTime NextRunTime = default, int? FailureDisableThreshold = null,
    string? DisableReason = null, string? ApiKey = null)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedBy, LastModifiedOn);

public record ImportJobPropertyResponse(Guid TenantId, Guid ImportJobId, string Name, string Value,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedBy, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedBy, LastModifiedOn);

public record AddImportJobRequest(Guid TenantId, int NodeId,
    Guid ImportComponentId, string Name, string HostName, string Username, string Password,
    int UpdateFrequencyInterval, string FileExclusions, bool IsEnabled, int? FailureDisableThreshold = null);

public record ExportComponentResponse(Guid TenantId, Guid Id, string Name, string uri,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedBy, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedBy, LastModifiedOn);

public record ExportComponentPropertyResponse(Guid TenantId, Guid ExportComponentId, string Name,
    string FieldType, string Value, bool IsSecret, Guid Id,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedBy, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedBy, LastModifiedOn);

public record ExportComponentJobPropertyResponse(Guid TenantId, Guid ExportComponentId, string Name, string FieldType,
    string DefaultValue, bool IsSecret, bool IsOptional, string CustomValidator, string CustomValidatorMessage,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedBy, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedBy, LastModifiedOn);

public record ExportJobResponse(Guid TenantId, Guid CompanyId, int NodeId, Guid Id,
    Guid ExportComponentId, string Name, string HostName, string Username, string Password,
    Core.Version Version,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedBy, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedBy, LastModifiedOn);

public record ExportJobResponseV2(Guid TenantId, Guid CompanyId, int NodeId, Guid Id,
    Guid ExportComponentId, string Name, string HostName, string Username, string Password,
    Core.Version Version, ExportComponentResponse ExportComponent, List<ExportJobPropertyResponse> ExportJobProperties,
    List<ExportComponentPropertyResponse> ExportComponentProperties, List<ExportComponentJobPropertyResponse> ExportComponentJobProperties,
    Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedBy, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedBy, LastModifiedOn);

public record ExportJobPropertyResponse(Guid TenantId, Guid ExportJobId, string Name, string Value,
    Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedBy, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject(Id, CreatedById, CreatedOn, LastModifiedBy, LastModifiedOn);

public record AddExportJobRequest(Guid TenantId, int NodeId,
    Guid ExportComponentId, string Name, string HostName, string Username, string Password);

//SourceType=Forwarder : SourceName=Name of the forwarder, SourceId=Guid of the forwarder
//SourceType=ExportAgent : SourceName=Name of the export job, SourceId=Guid of the export job
//SourceType=ServerAction : SourceName=Name of the server action, SourceId=Guid of the server action (Only valid for Online-Transactions)
public record AddExportJobTransactionStatusV2(
    Guid MessageId,  //Transaction Id 
    Guid CorrelationId,
    string Message,
    [property: JsonConverter(typeof(JsonStringEnumConverter))] SourceType SourceType, string SourceName, Guid SourceId,
    Outcome OperationState, ErrorClass OperationErrorClass);

public enum SourceType
{
    Forwarder,
    ExportAgent,
    ServerAction
}


//If you add new values here they need to be matched in GTC for transaction logging to work correctly
public enum Outcome
{
    Unknown,
    Success,
    RetryableError,
    NonRetryableError,
    Discard
}

//If you add new values here they need to be matched in GTC for transaction logging to work correctly
public enum ErrorClass
{
    None,
    Server,
    Clock,
    User
}