namespace GT.Connect.Management.Demo.ConnectApi.People;

/// <summary>Body for creating (server <c>PersonAddCommand</c>) or updating (server <c>PersonUpdateRequest</c>) a person.</summary>
/// <remarks>An update replaces every field, so supply Email, Telephone and BlockBiometrics to keep their current values.</remarks>
public record PersonRequest(int NodeId, Guid CompanyId,
    string Firstname, string Surname, string ExternalId, string BadgeCode, string KeyPad, string PinCode,
    string Language, string Roles, string VerifyBy, string? DisplayItems, string ExternalRevisionId, string? Token, 
    string? BioFilterTags, string? ExtendedData,
    string? Email = null, string? Telephone = null, bool BlockBiometrics = false);
