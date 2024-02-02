using DayFlags.Core.Models;
using DayFlags.Core.Util;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Core.Repositories;

internal class FlagGroupRepository(DayFlagsDb db) : IFlagGroupRepository
{
    public async Task<FlagGroup> AddFlagGroupAsync(FlagGroup flagGroup)
    {
        db.FlagGroups.Add(flagGroup);
        await db.SaveChangesAsync();
        return flagGroup;
    }

    public async Task<FlagGroup> DeleteFlagGroupAsync(FlagGroup flagGroup)
    {
        db.FlagGroups.Remove(flagGroup);
        await db.SaveChangesAsync();
        return flagGroup;
    }

    public async Task<IEnumerable<DayFlag>> GetChildrenDayFlagsAsync(FlagGroup flagGroup, DateRange dateRange)
    {
        var flagTypes = await GetChildrenFlagTypesAsync(flagGroup);
        return await db.DayFlags
            .Where(e => flagTypes.Contains(e.FlagType))
            .OrderBy(e => e.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<FlagType>> GetChildrenFlagTypesAsync(FlagGroup flagGroup)
    {
        return await
            db.FlagTypes
            .Where(e => e.FlagGroup == flagGroup)
            .OrderBy(e => e.FlagTypeKey)
            .ToListAsync();
    }

    public ValueTask<FlagGroup?> GetFlagGroupAsync(Guid flagGroupId)
    {
        return db.FlagGroups.FindAsync(flagGroupId);
    }

    public IQueryable<FlagGroup> GetFlagGroupsQuery(Realm realm)
    {
        return db.FlagGroups
            .Where(e => e.Realm == realm);
    }

    public Task<bool> IsValidDayFlagAsync(DayFlag dayFlag)
    {
        // TODO
        throw new NotImplementedException();
    }

    public async Task<FlagGroup> UpdateFlagGroupAsync(FlagGroup flagGroup)
    {
        db.Entry(flagGroup).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return flagGroup;
    }
}