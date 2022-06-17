using DayFlags.Core.Exceptions;
using DayFlags.Core.Models;

namespace DayFlags.Server.Services;

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
    /// <returns>Created entity</returns>
    public async ValueTask<DayEntry> AddDayEntryAsync(DayEntry dayEntry)
    {
        _db.DayEntries.Add(dayEntry);
        await _db.SaveChangesAsync();
        return dayEntry;
    }

    /// <summary>
    /// Creates a <see cref="DayEntry"/> at the given time
    /// </summary>
    /// <param name="entryType">EntryType</param>
    /// <param name="timestamp">Timestamp</param>
    /// <returns>Created entity</returns>
    public async ValueTask<DayEntry> AddDayEntryAtAsync(
        EntryType entryType,
        DateTime timestamp
    )
    {
        return await AddDayEntryAsync(new DayEntry(entryType.EntryTypeId, new DateOnly())
        {
            DateTime = timestamp
        });
    }

    /// <summary>
    /// Like <see cref="AddDayEntryAtAsync(EntryType, DateTime)"/> with
    /// options like <see cref="AddCurrentOfEntryType(string, bool)"/>
    /// </summary>
    /// <param name="entryTypeId">Id of the <see cref="EntryType"/></param>
    /// <param name="timestamp">Timestamp</param>
    /// <param name="create">Should create EntryType?</param>
    /// <returns>Created entity</returns>
    public async ValueTask<DayEntry> AddDayEntryAtAsync(
        string entryTypeId,
        DateTime timestamp,
        bool create = true
    )
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

        return await AddDayEntryAtAsync(entryType, timestamp);
    }

    /// <summary>
    /// Adds a <see cref="DayEntry"/> at current date and time to the given type
    /// </summary>
    /// <param name="type"><see cref="DayEntry"/> of the entry</param>
    /// <returns>The created DayEntry</returns>
    public ValueTask<DayEntry> AddCurrentOfEntryType(EntryType type)
        => AddDayEntryAtAsync(type, DateTime.Now);

    /// <summary>
    /// Adds a <see cref="DayEntry"/> at the current date and time with the 
    /// given entryTypeId. You can specify if this <see cref="EntryType"/> should
    /// created, if it dosn't exists
    /// </summary>
    /// <param name="entryTypeId">Id of <see cref="EntryType"/></param>
    /// <param name="create">Should create?</param>
    /// <returns>Created <see cref="DayEntry"/></returns>
    public ValueTask<DayEntry> AddCurrentOfEntryType(string entryTypeId,
        bool create = true)
        => AddDayEntryAtAsync(entryTypeId, DateTime.Now, create);
}