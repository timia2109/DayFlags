using DayFlags.Core.Exceptions;
using DayFlags.Core.Models;

namespace DayFlags.Core.Services;

/// <summary>
/// Provides methods to operate with DayEntries
/// </summary>
public class DayEntriesService
{

    private readonly DayFlagsDb _db;
    private readonly EntryTypeService _entryTypeService;

    public DayEntriesService(DayFlagsDb db, EntryTypeService entryTypeService)
    {
        _db = db;
        _entryTypeService = entryTypeService;
    }

    /// <summary>
    /// Adds a <see cref="DayEntry" /> to the Database
    /// </summary>
    /// <param name="dayEntry">DayEntry</param>
    /// <returns>Async Task</returns>
    public async Task AddDayEntryAsync(DayEntry dayEntry)
    {
        _db.DayEntries.Add(dayEntry);
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Adds a <see cref="DayEntry"/> at current date and time to the given type
    /// </summary>
    /// <param name="type"><see cref="DayEntry"/> of the entry</param>
    /// <returns>The created DayEntry</returns>
    public async ValueTask<DayEntry> AddCurrentOfEntryType(EntryType type)
    {
        var now = DateTime.Now;
        var entry = new DayEntry(
            type.EntryTypeId, new DateOnly()
        );
        entry.DateTime = now;
        await AddDayEntryAsync(entry);
        return entry;
    }

    /// <summary>
    /// Adds a <see cref="DayEntry"/> at the current date and time with the 
    /// given entryTypeId. You can specify if this <see cref="EntryType"/> should
    /// created, if it dosn't exists
    /// </summary>
    /// <param name="entryTypeId">Id of <see cref="EntryType"/></param>
    /// <param name="create">Should create?</param>
    /// <returns>Created <see cref="DayEntry"/></returns>
    public async ValueTask<DayEntry> AddCurrentOfEntryType(string entryTypeId,
        bool create = true)
    {
        EntryType entryType;

        var exists = await _entryTypeService.HasEntryTypeAsync(entryTypeId);
        if (!exists && create)
        {
            entryType = await _entryTypeService.CreateByIdAsync(entryTypeId);
        }
        else if (!exists && !create)
            throw new EntryTypeNotFoundException(entryTypeId);
        else
            entryType = await _entryTypeService.GetEntryTypeById(entryTypeId);

        return await AddCurrentOfEntryType(entryType);
    }
}