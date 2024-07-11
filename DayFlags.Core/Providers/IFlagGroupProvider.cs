using DayFlags.Core.Models;
using DayFlags.Core.Util;

namespace DayFlags.Core.Providers;

public interface IFlagGroupProvider
{
    ValueTask<FlagGroup?> GetFlagGroupAsync(Guid flagGroupId);
    
    ValueTask<FlagGroup?> GetFlagGroupAsync(string flagGroupKey);
    ValueTask<FlagGroup?> GetFlagGroupAsync(DayFlag flag);
    ValueTask<IEnumerable<FlagGroup>> GetFlagGroupsAsync();
}