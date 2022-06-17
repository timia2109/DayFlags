using DayFlags.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Core.MatchProvider;

internal abstract class EntryTypeMatcher
{
    protected DbSet<DayEntry> Collection { get; }
    protected EntryType EntryType { get; }
    protected IQueryable<DayEntry> Query => Collection
        .Where(e => e.EntryType == EntryType);

    protected EntryTypeMatcher(DayFlagsDb db, EntryType entryType)
    {
        Collection = db.DayEntries;
        EntryType = entryType;
    }

    public abstract Task<IList<DateOnly>> FindAllMatchesAsync();

    public abstract Task<IList<DateOnly>> FindMatchesAsync(DateOnly rangeBegin, DateOnly rangeEnd);

}

internal class ThresholdEntryTypeMatcher : EntryTypeMatcher
{
    public ThresholdEntryTypeMatcher(DayFlagsDb db, EntryType entryType)
        : base(db, entryType)
    {
    }

    private async Task<IList<DateOnly>> BaseQuery(IQueryable<DayEntry> query)
    {
        return await query
            .GroupBy(e => e.Date)
            .Where(e => e.Count() >= EntryType.Threshold)
            .Select(e => e.Key)
            .ToListAsync();
    }

    public override Task<IList<DateOnly>> FindAllMatchesAsync()
    {
        return BaseQuery(
            Collection.Where(e => e.EntryType == EntryType)
        );
    }

    public override Task<IList<DateOnly>> FindMatchesAsync(DateOnly rangeBegin, DateOnly rangeEnd)
    {
        return BaseQuery(
            Collection.Where(e => e.EntryType == EntryType
                && e.Date >= rangeBegin
                && e.Date <= rangeEnd
            )
        );
    }
}

internal class OnceADayEntryTypeMatcher : EntryTypeMatcher
{
    public OnceADayEntryTypeMatcher(DayFlagsDb db, EntryType entryType)
        : base(db, entryType)
    {
    }

    public override async Task<IList<DateOnly>> FindAllMatchesAsync()
    {
        return await Query
            .GroupBy(e => e.Date)
            .Select(e => e.Key)
            .ToListAsync();
    }

    public override async Task<IList<DateOnly>> FindMatchesAsync(DateOnly rangeBegin,
        DateOnly rangeEnd)
    {
        return await Query
            .Where(e => e.Date >= rangeBegin && e.Date <= rangeEnd)
            .GroupBy(e => e.Date)
            .Select(e => e.Key)
            .ToListAsync();
    }
}
