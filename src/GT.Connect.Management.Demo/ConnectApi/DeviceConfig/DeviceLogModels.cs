namespace GT.Connect.Management.Demo.ConnectApi.DeviceConfig;

/// <summary>
/// Filter for historic device log queries. Every field is optional; the server defaults the time range to the
/// last 24 hours and trims ranges wider than 30 days. Times are UTC.
/// </summary>
public record DeviceLogQueryRequest(
    DateTime? StartUtc = null,
    DateTime? EndUtc = null,
    int? BinSeconds = null,
    string? SearchTerm = null,
    int? LowLevel = null,
    int? HighLevel = null,
    List<string>? Sources = null);

/// <summary>A single historic device log line.</summary>
public record DeviceLogMessage(DateTime Timestamp, int Index, string? DeviceId, string? Content, int Level, string? Process);

/// <summary>Log line count for one time bin at one syslog severity.</summary>
public record DeviceLogSummaryRow(DateTime Timestamp, int Level, long Count);

/// <summary>Earliest and latest log timestamps matching a filter; both null when nothing matched.</summary>
public record DeviceLogTimespan(DateTime? MinTimestamp, DateTime? MaxTimestamp);
