using DayFlags.Core.Models;
using DayFlags.Core.Util;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Core.Repositories;

internal class DayFlagRepository(DayFlagsDb db) : IDayFlagRepository
{
    public async Task<DayFlag> AddDayFlagAsync(DayFlag dayFlag)
    {
        db.DayFlags.Add(dayFlag);
        await db.SaveChangesAsync();
        return dayFlag;
    }

    public Task<int> CountDayFlagsAsync(FlagType type, DateRange dateRange)
    {
        return db.DayFlags.CountAsync(f =>
            f.FlagType == type
            && f.Date >= dateRange.Start
            && f.Date <= dateRange.End
        );
    }

    public async Task<DayFlag> DeleteDayFlagAsync(DayFlag dayFlag)
    {
        db.DayFlags.Remove(dayFlag);
        await db.SaveChangesAsync();
        return dayFlag;
    }

    private IQueryable<DayFlag> GetNextDayFlagQuery(FlagType type, DateRange? dateRange)
    {
        IQueryable<DayFlag> query = db.DayFlags
            .Where(e => e.FlagType == type)
            .OrderBy(e => e.Date);

        if (dateRange != null)
        {
            query = query.Where(e => e.Date >= dateRange.Start && e.Date <= dateRange.End);
        }

        return query;
    }

    public Task<DayFlag?> FindFirstFlagAsync(FlagType type, DateRange? dateRange = null)
    {
        return GetNextDayFlagQuery(type, dateRange)
            .FirstOrDefaultAsync();
    }

    public Task<DayFlag?> FindLastFlagAsync(FlagType type, DateRange? dateRange = null)
    {
        return GetNextDayFlagQuery(type, dateRange)
           .LastOrDefaultAsync();
    }

    public ValueTask<DayFlag?> GetDayFlagAsync(Guid flagId)
    {
        return db.DayFlags.FindAsync(flagId);
    }

    public IQueryable<DayFlag> GetDayFlagsQuery(IEnumerable<FlagType> flagTypes, DateRange dateRange)
    {
        return db.DayFlags.Where(f =>
            flagTypes.Contains(f.FlagType)
            && f.Date >= dateRange.Start
            && f.Date <= dateRange.End
        );
    }

    public IQueryable<DayFlag> GetDayFlagsQuery(Realm realm, DateRange dateRange)
    {
        return db.DayFlags.Where(f =>
            f.FlagType!.Realm == realm
            && f.Date >= dateRange.Start
            && f.Date <= dateRange.End
        );
    }
}