using DayFlags.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Core.Services;

public class EntryTypeService
{

    private readonly DayFlagsDb _db;

    public EntryTypeService(DayFlagsDb db)
    {
        _db = db;
    }

    /// <summary>
    /// Checks if a <see cref="EntryType" /> with the given id exists
    /// </summary>
    /// <param name="entryTypeId">Id of the entry type</param>
    /// <returns>Exists?</returns>
    public Task<bool> HasEntryTypeAsync(string entryTypeId)
    {
        return _db.EntryTypes.AnyAsync(e => e.EntryTypeId == entryTypeId);
    }

    /// <summary>
    /// Returns the <see cref="EntryType" /> by it's ID
    /// </summary>
    /// <param name="entryTypeId">Id of the EntryType</param>
    /// <returns>EntryType</returns>
    /// <exception cref="InvalidOperationException">No EntryType found</exception>
    public Task<EntryType> GetEntryTypeById(string entryTypeId)
    {
        return _db.EntryTypes.FirstAsync(e => e.EntryTypeId == entryTypeId);
    }

    /// <summary>
    /// Adds a <see cref="EntryType" /> to the database
    /// </summary>
    /// <param name="entryType">EntryType</param>
    /// <returns>The created EntryType</returns>
    public async Task<EntryType> AddAsync(EntryType entryType)
    {
        _db.EntryTypes.Add(entryType);
        await _db.SaveChangesAsync();
        return entryType;
    }

    /// <summary>
    /// Created a <see cref="EntryType" /> with default configuration by it's id
    /// . The <see cref="EntryType" /> has the requirement 
    /// <see cref="Enums.EntryTypeRequirement.OnceADay"/> and no description
    /// </summary>
    /// <param name="entryTypeId">Id of the EntryType</param>
    /// <returns>Created entity</returns>
    public Task<EntryType> CreateByIdAsync(string entryTypeId)
    {
        var entry = new EntryType(entryTypeId,
            Enums.EntryTypeRequirement.OnceADay);
        return AddAsync(entry);
    }
}