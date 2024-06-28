using DayFlags.Core.Models;

namespace DayFlags.Core.Providers;

public interface IDayFlagProvider
{
    /// <summary>
    /// Returns a list of all DayFlags by this provider
    /// </summary>
    /// <param name="query">Query</param>
    /// <returns>List of DayFlags</returns>
    ValueTask<IEnumerable<DayFlag?>> GetDayFlagsAsync(DayFlagQuery query);
}

/// <summary>
/// A <see cref="IDayFlagProvider"/> which can change DayFlags
/// </summary>
public interface IChangeableDayFlagProvider : IDayFlagProvider
{
    ValueTask<DayFlag?> DeleteDayFlag(DayFlag flag);

    ValueTask<DayFlag> AddDayFlag(DayFlag flag);

    ValueTask<DayFlag?> ModifyDayFlag(DayFlag flag);
}