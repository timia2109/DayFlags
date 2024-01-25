using DayFlags.Core.Models;

namespace DayFlags.Core.Repositories;

public interface IDayFlagRepository
{
    Task<DayFlag> GetDayFlagAsync(Guid flagId);
    Task<IEnumerable<DayFlag>> GetDayFlagsAsync(Realm realm,
        DateOnly rangeStart,
        DateOnly rangeEnd);
    Task<IEnumerable<DayFlag>> GetDayFlagsAsync(FlagType flagType,
        DateOnly rangeStart,
        DateOnly rangeEnd);
    Task<IEnumerable<DayFlag>> GetDayFlagsAsync(
        IEnumerable<FlagType> flagTypes,
        DateOnly rangeStart,
        DateOnly rangeEnd);
    Task<DayFlag> AddDayFlagAsync(DayFlag dayFlag);
    Task<DayFlag> DeleteDayFlagAsync(DayFlag dayFlag);
    Task<IEnumerable<DayFlag>> FindDayFlagsForDateAsync(
        Realm realm,
        DateOnly date);
    Task<DayFlag?> FindLastFlagAsync(FlagType type);
    Task<DayFlag?> FindFirstFlagAsync(FlagType type);
    Task<int?> CountDayFlagsAsync(FlagType type,
        DateOnly rangeStart,
        DateOnly rangeEnd);
}