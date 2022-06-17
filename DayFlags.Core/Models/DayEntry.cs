using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayFlags.Core.Models;

/// <summary>
/// Represent a entry for a day
/// </summary>
public class DayEntry
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Id of the EntryType
    /// </summary>
    [StringLength(EntryType.IdLength)]
    public string EntryTypeId { get; set; }

    /// <summary>
    /// Reference Entry Type
    /// </summary>
    /// <value></value>
    [ForeignKey(nameof(EntryTypeId))]
    public EntryType? EntryType { get; set; }

    public DayEntry(string entryTypeId)
    {
        EntryTypeId = entryTypeId;
    }
}