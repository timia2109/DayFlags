using DayFlags.Core.EntryTypes;
using DayFlags.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Server.EntryTypes;

/// <summary>
/// A <see cref="AEntryTypeProvider"/> that fetches entries from the database
/// </summary>
public class DbEntryTypesProvider : AEntryTypeProvider
{
    private readonly DayFlagsDb _db;

    public DbEntryTypesProvider(DayFlagsDb db)
    {
        _db = db;
    }

    public override Task<List<EntryType>> GetEntryTypesAsync()
    {
        return _db.EntryTypes.ToListAsync();
    }
}