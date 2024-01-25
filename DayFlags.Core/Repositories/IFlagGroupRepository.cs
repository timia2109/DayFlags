using DayFlags.Core.Models;
using DayFlags.Core.Util;

namespace DayFlags.Core.Repositories;

public interface IFlagGroupRepository
{
    Task<FlagGroup> GetFlagGroupAsync(Guid flagGroupId);
    Task<IEnumerable<FlagGroup>> GetFlagGroupsAsync(Realm realm);
    Task<FlagGroup> AddFlagGroupAsync(FlagGroup flagGroup);
    Task<FlagGroup> UpdateFlagGroupAsync(FlagGroup flagGroup);
    Task<FlagGroup> DeleteFlagGroupAsync(FlagGroup flagGroup);
    Task<bool> IsValidDayFlagAsync(DayFlag dayFlag);
    Task<IEnumerable<FlagType>> GetChildrenFlagTypesAsync(
        FlagGroup flagGroup);
    Task<IEnumerable<DayFlag>> GetChildrenDayFlagsAsync(
        FlagGroup flagGroup, DateRange dateRange);
}