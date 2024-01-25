using DayFlags.Core.Models;
using DayFlags.Core.Util;

namespace DayFlags.Core.Repositories;

public interface IDayFlagRepository
{
    ValueTask<DayFlag?> GetDayFlagAsync(Guid flagId);
    Task<IEnumerable<DayFlag>> GetDayFlagsAsync(Realm realm,
        DateRange dateRange);
    Task<IEnumerable<DayFlag>> GetDayFlagsAsync(FlagType flagType,
        DateRange dateRange);
    Task<IEnumerable<DayFlag>> GetDayFlagsAsync(
        IEnumerable<FlagType> flagTypes,
        DateRange dateRange);
    Task<DayFlag> AddDayFlagAsync(DayFlag dayFlag);
    Task<DayFlag> DeleteDayFlagAsync(DayFlag dayFlag);
    Task<DayFlag?> FindLastFlagAsync(FlagType type,
        DateRange? dateRange = null);
    Task<DayFlag?> FindFirstFlagAsync(FlagType type,
        DateRange? dateRange = null);
    Task<int> CountDayFlagsAsync(FlagType type,
        DateRange dateRange);
}