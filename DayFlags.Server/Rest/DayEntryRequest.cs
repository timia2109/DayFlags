namespace DayFlags.Server.Models.Rest;

/// <summary>
/// Creation Request for a <see cref="DayEntry"/>
/// </summary>
/// <param name="EntryTypeId">Id of the <see cref="EntryType"/></param>
/// <param name="Timestamp">Timestamp of this entry. Null = now</param>
/// <param name="Create">Should create the EntryType</param>
public record DayEntryRequest(
    string EntryTypeId,
    DateTime? Timestamp,
    bool Create = true
);