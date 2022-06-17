using DayFlags.Core.Models;
using DayFlags.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Core.MatchProvider;

/// <summary>
/// Default <see cref="IMatchProvider"/> that works against the database
/// </summary>
public class DatabaseMatchProvider : IMatchProvider
{
    private readonly IServiceProvider _services;
    private readonly DayFlagsDb _db;

    public DatabaseMatchProvider(DayFlagsDb db, IServiceProvider services)
    {
        _db = db;
        _services = services;
    }

    public bool RequireDatabase => true;

    private Task<List<EntryType>> FindEntryTypes()
    {
        return _db.EntryTypes.Where(e => e.ProviderId == null)
            .ToListAsync();
    }

    private EntryTypeMatcher GetEntryTypeMatcher(EntryType entryType)
    {
        var requirement = entryType.Requirement;

        Type targetType = requirement switch
        {
            EntryTypeRequirement.Threshold => typeof(ThresholdEntryTypeMatcher),
            EntryTypeRequirement.OnceADay => typeof(OnceADayEntryTypeMatcher),
            _ => throw new NotImplementedException(requirement.ToString())
        };

        return (EntryTypeMatcher)ActivatorUtilities
            .CreateInstance(_services, targetType, entryType);
    }

    public Task<IList<DateOnly>> FindAllMatchesAsync(EntryType entityType)
    {
        var matcher = GetEntryTypeMatcher(entityType);
        return matcher.FindAllMatchesAsync();
    }

    public Task<IList<DateOnly>> FindMatchesAsync(DateOnly rangeBegin,
        DateOnly rangeEnd, EntryType entityType)
    {
        var matcher = GetEntryTypeMatcher(entityType);
        return matcher.FindMatchesAsync(rangeBegin, rangeEnd);
    }

    public async Task<IDictionary<string, IList<DateOnly>>> FindMatchesAsync(DateOnly rangeBegin, DateOnly rangeEnd)
    {
        var entryTypes = await FindEntryTypes();

        var dict = new Dictionary<string, IList<DateOnly>>();
        foreach (var entryType in entryTypes)
        {
            var days = await FindMatchesAsync(rangeBegin, rangeEnd, entryType);
            dict.Add(entryType.EntryTypeId, days);
        }

        return dict;
    }

    public bool IsResponsibleFor(EntryType entryType)
    {
        return entryType.ProviderId == null;
    }
}