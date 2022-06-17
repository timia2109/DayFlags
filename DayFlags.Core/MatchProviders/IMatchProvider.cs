using DayFlags.Core.Models;

namespace DayFlags.Core.MatchProvider;

/// <summary>
/// A MatchProvider checks different TimeRanges to check for
/// matches
/// </summary>
public interface IMatchProvider
{

    /// <summary>
    /// Returns if this Provider required Database access.
    /// If not, it get's called parallel
    /// </summary>
    public bool RequireDatabase { get; }

    public bool IsResponsibleFor(EntryType entryType);

    /// <summary>
    /// Finds matches in the given range
    /// </summary>
    /// <param name="rangeBegin">Begin of range</param>
    /// <param name="rangeEnd">End of range</param>
    /// <param name="entryType">Searched <see cref="EntryType"/></param>
    /// <returns>Set with all matched days</returns>
    public Task<IList<DateOnly>> FindMatchesAsync(
        DateOnly rangeBegin,
        DateOnly rangeEnd,
        EntryType entityType
    );

    /// <summary>
    /// Find all Matches of all this IMatchProvider 
    /// </summary>
    /// <param name="rangeBegin">Begin of range</param>
    /// <param name="rangeEnd">End of range</param>
    /// <returns>All Matches (string = EntryProviderId, value = All matched ids)</returns>
    public Task<IDictionary<string, IList<DateOnly>>> FindMatchesAsync(
        DateOnly rangeBegin,
        DateOnly rangeEnd
    );

    /// <summary>
    /// Find all Matches of the given <see cref="EntryTypeId"/>
    /// </summary>
    /// <param name="entryType">Searched <see cref="EntryType"/></param>
    /// <returns>Set with all matched days</returns>
    public Task<IList<DateOnly>> FindAllMatchesAsync(
        EntryType entityType
    );
}