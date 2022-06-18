using DayFlags.Core.Exceptions;
using DayFlags.Core.Models;
using DayFlags.Core.Enums;
using Microsoft.EntityFrameworkCore;
using DayFlags.Core.EntryTypes;

namespace DayFlags.Server.Services;

public class EntryTypeService
{

    private readonly DayFlagsDb _db;
    private readonly IEnumerable<AEntryTypeProvider> _entryTypesProviders;

    public EntryTypeService(DayFlagsDb db,
        IEnumerable<AEntryTypeProvider> entryTypesProviders)
    {
        _db = db;
        _entryTypesProviders = entryTypesProviders;
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
    /// <exception cref="EntryTypeNotFoundException">No EntryType found</exception>
    public async Task<EntryType> GetEntryTypeById(string entryTypeId)
    {
        var entryType = await _db.EntryTypes.FirstOrDefaultAsync(
            e => e.EntryTypeId == entryTypeId);

        if (entryType == null)
            throw new EntryTypeNotFoundException(entryTypeId);

        return entryType;
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
            EntryTypeRequirement.OnceADay);
        return AddAsync(entry);
    }

    /// <summary>
    /// Fetches all EntryTypes including non-usercreated (like from plugins)
    /// </summary>
    /// <returns>List of all known EntryTypes</returns>
    public async Task<List<EntryType>> GetAllEntryTypesAsync()
    {
        var entryTypes = new List<EntryType>();

        foreach (var provider in _entryTypesProviders)
        {
            var results = await provider.GetEntryTypesAsync();
            entryTypes.AddRange(results);
        }

        return entryTypes;
    }
}