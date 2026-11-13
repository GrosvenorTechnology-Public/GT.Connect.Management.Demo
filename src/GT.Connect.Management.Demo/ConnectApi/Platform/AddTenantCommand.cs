namespace GT.Connect.Management.Demo.ConnectApi.Platform;

public record AddTenantCommand (
    Guid Id,
    string Name,
    string DisplayName,
    Guid UserId,
    string UserTitle,
    string UserFirstname,
    string UserSurname,
    string UserEmail,
    string UserPassword,
    Guid? MachineClientUserId = null
);
