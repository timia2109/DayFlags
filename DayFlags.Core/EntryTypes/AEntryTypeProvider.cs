using DayFlags.Core.Models;

namespace DayFlags.Core.EntryTypes;

/// <summary>
/// Provides <see cref="EntryType"/>s
/// </summary>
public abstract class AEntryTypeProvider
{
    /// <summary>
    /// Provides all EntryTypes that this Provider knows
    /// </summary>
    /// <returns>List of EntryTypes</returns>
    public abstract Task<List<EntryType>> GetEntryTypesAsync();

}