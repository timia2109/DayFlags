using DayFlags.Core.Models;
using DayFlags.Core.Util;

namespace DayFlags.Core.Repositories;

public interface IDayFlagRepository
{
    ValueTask<DayFlag?> GetDayFlagAsync(Guid flagId);
    IQueryable<DayFlag> GetDayFlagsQuery(
        Realm realm,
        DateRange dateRange);
    IQueryable<DayFlag> GetDayFlagsQuery(
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