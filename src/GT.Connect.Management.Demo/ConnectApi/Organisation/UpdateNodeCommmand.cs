namespace GT.Connect.Management.Demo.ConnectApi.Organisation;

//All fields must be set when updating a node, otherwise they will be cleared.
// Use the mapper from a node response to an update node command to ensure all fields are set correctly.
// Notes on server behaviour:
//  - NodeType and ParentId are not changed by an update (moving nodes is not supported).
//  - Null AdditionalFieldN values are left unchanged; values equal to the inherited value are not stored on the node.
//  - EnabledAutoMobileCredentialAssignment and EnableAnonymousCredentials (when non-null) are applied to the node AND all of its descendants.
//  - EnableAnonymousCredentials may only be set (non-null) on Partner, ChannelPartner or Company nodes, otherwise the update fails validation.
//  - The *Inherited flags on NodeResponse are read-only and cannot be updated.
public record UpdateNodeCommand(int Id, Guid TenantId, int? ParentId, string NodeType, string Name,
    string? AdditionalField1,
    string? AdditionalField2,
    string? AdditionalField3,
    string? AdditionalField4,
    string? AdditionalField5,
    string? AdditionalField6,
    string? AdditionalField7,
    string? AdditionalField8,
    string? AdditionalField9,
    string? AdditionalField10,
    bool AdditionalField1_IsSecure,
    bool AdditionalField2_IsSecure,
    bool AdditionalField3_IsSecure,
    bool AdditionalField4_IsSecure,
    bool AdditionalField5_IsSecure,
    bool AdditionalField6_IsSecure,
    bool AdditionalField7_IsSecure,
    bool AdditionalField8_IsSecure,
    bool AdditionalField9_IsSecure,
    bool AdditionalField10_IsSecure,
    int PersonTombstonePeriod,
    int TransactionRetentionPeriod,
    bool EnabledAutoMobileCredentialAssignment,
    bool? EnableAnonymousCredentials = null);
