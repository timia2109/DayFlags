using DayFlags.Core.Models;
using DayFlags.Core.Util;

namespace DayFlags.Core.Repositories;

public interface IFlagGroupRepository
{
    ValueTask<FlagGroup?> GetFlagGroupAsync(Guid flagGroupId);
    ValueTask<FlagGroup?> GetFlagGroupAsync(Realm realm, string flagGroupKey);
    ValueTask<FlagGroup?> GetFlagGroupAsync(DayFlag flag);
    IQueryable<FlagGroup> GetFlagGroupsQuery(Realm realm);
    Task<FlagGroup> AddFlagGroupAsync(FlagGroup flagGroup);
    Task<FlagGroup> UpdateFlagGroupAsync(FlagGroup flagGroup);
    Task<FlagGroup> DeleteFlagGroupAsync(FlagGroup flagGroup);
    IQueryable<FlagType> GetChildrenFlagTypesQuery(FlagGroup flagGroup);
    Task<IEnumerable<DayFlag>> GetChildrenDayFlagsAsync(
        FlagGroup flagGroup, DateRange dateRange);
}