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

    /// <summary>
    /// Affected Date
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Affected Time (optional; reserved for later)
    /// </summary>
    public TimeOnly? Time { get; set; }

    /// <summary>
    /// Field to work with DateTime. Mapped to the fields <see cref="Date" /> 
    /// and <see cref="Time" />. If <see cref="Time" /> is null 00:00:00 is used
    /// </summary>
    [NotMapped]
    public DateTime DateTime
    {
        get => Date.ToDateTime(
                Time ?? new TimeOnly()
            );

        set
        {
            Date = DateOnly.FromDateTime(value);
            Time = TimeOnly.FromDateTime(value);
        }
    }

    /// <summary>
    /// Creation Time
    /// </summary>
    public DateTime Created { get; set; } = DateTime.Now;

    public DayEntry(string entryTypeId, DateOnly date)
    {
        EntryTypeId = entryTypeId;
        Date = date;
    }
}