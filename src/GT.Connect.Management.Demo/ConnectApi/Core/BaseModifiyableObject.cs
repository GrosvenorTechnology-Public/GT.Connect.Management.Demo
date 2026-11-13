namespace GT.Connect.Management.Demo.ConnectApi.Core;

public abstract record BaseModifiyableObject 
    (Guid Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseModifiyableObject<Guid>(Id, CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

public abstract record BaseModifiyableObject<T>
    (T Id, Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn)
    : BaseObject(CreatedById, CreatedOn, LastModifiedById, LastModifiedOn);

public abstract record BaseObject
    (Guid CreatedById, DateTimeOffset CreatedOn, Guid LastModifiedById, DateTimeOffset LastModifiedOn);

public record Version (string Name);