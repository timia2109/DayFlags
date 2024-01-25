using DayFlags.Core.Models;
using DayFlags.Core.Util;

namespace DayFlags.Core.Repositories;

public interface IDayFlagRepository
{
    Task<DayFlag> GetDayFlagAsync(Guid flagId);
    Task<IEnumerable<DayFlag>> GetDayFlagsAsync(Realm realm,
        DateRange dateRange);
    Task<IEnumerable<DayFlag>> GetDayFlagsAsync(FlagType flagType,
        DateRange dateRange);
    Task<IEnumerable<DayFlag>> GetDayFlagsAsync(
        IEnumerable<FlagType> flagTypes,
        DateRange dateRange);
    Task<DayFlag> AddDayFlagAsync(DayFlag dayFlag);
    Task<DayFlag> DeleteDayFlagAsync(DayFlag dayFlag);
    Task<IEnumerable<DayFlag>> FindDayFlagsForDateAsync(
        Realm realm,
        DateRange dateRange);
    Task<DayFlag?> FindLastFlagAsync(FlagType type);
    Task<DayFlag?> FindFirstFlagAsync(FlagType type);
    Task<int?> CountDayFlagsAsync(FlagType type,
        DateRange dateRange);
}