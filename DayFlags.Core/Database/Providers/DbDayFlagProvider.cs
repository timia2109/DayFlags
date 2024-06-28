using DayFlags.Core.Database.Models;
using DayFlags.Core.Models;
using DayFlags.Core.Providers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Core.Database.Providers;

public class DbDayFlagProvider(Realm realm, DayFlagsDb db) : IChangeableDayFlagProvider
{
    private static DayFlag Convert(DbDayFlag dbDayFlag)
        => dbDayFlag.Adapt<DayFlag>();
    
    public async ValueTask<IEnumerable<DayFlag?>> GetDayFlagsAsync(DayFlagQuery query)
    {
        var dateRange = query.DateRange!;
        
        IQueryable<DbDayFlag> dbQuery = db.DayFlags
            .Where(e => e.Date <= dateRange.Start && e.Date >= dateRange.End)
            .OrderBy(e => e.Date)
            .ThenBy(e => e.FlagTypeId);

        if (query.FlagTypeKeys is not null)
        {
            dbQuery = dbQuery.Where(e => query.FlagTypeKeys.Contains(e.FlagType!.FlagTypeKey));
        }

        if (query.FlagGroupKeys is not null)
        {
            dbQuery = dbQuery.Where(e => query.FlagGroupKeys.Contains(e.FlagType.FlagGroup.FlagGroupKey));
        }

        var items = await dbQuery.ToListAsync();
        return items.Select(Convert);
    }

    public async ValueTask<DayFlag?> DeleteDayFlag(DayFlag flag)
    {
        var dbDayFlag = await SearchDayFlag(flag);
        if (dbDayFlag is null) return null;

        db.DayFlags.Remove(dbDayFlag);
        await db.SaveChangesAsync();
        return Convert(dbDayFlag);
    }

    public async ValueTask<DayFlag> AddDayFlag(DayFlag flag)
    {
        var dbEntity = flag.Adapt<DbDayFlag>();
        db.DayFlags.Add(dbEntity);
        await db.SaveChangesAsync();

        return Convert(dbEntity);
    }

    public async ValueTask<DayFlag?> ModifyDayFlag(DayFlag flag)
    {
        var dbDayFlag = await SearchDayFlag(flag);
        if (dbDayFlag is null) return null;

        flag.Adapt(dbDayFlag);
        await db.SaveChangesAsync();
        return Convert(dbDayFlag);
    }

    private async ValueTask<DbDayFlag?> SearchDayFlag(DayFlag flag)
    {
        if (!Guid.TryParse(flag.FlagId, out var dbFlag)) return null;
        return await db.DayFlags.FindAsync(dbFlag);
    }
}