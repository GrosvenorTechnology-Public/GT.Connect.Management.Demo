namespace GT.Connect.Management.Demo.ConnectApi.Organisation;

public record AddUserGroupCommand(Guid TenantId, string Name, int NodeId, bool IsSystem);
