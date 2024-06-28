using System.ComponentModel.DataAnnotations;

namespace DayFlags.Core.Database.Models;

/// <summary>
/// Represent a entry for a day
/// </summary>
public record DbDayFlag
{
    [Key]
    public Guid FlagId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Affected <see cref="FlagType"/>
    /// </summary>
    public required Guid FlagTypeId { get; init; }

    /// <summary>
    /// Relation to <see cref="FlagType"/>
    /// </summary>
    public DbFlagType? FlagType { get; set; }

    /// <summary>
    /// Affected Date
    /// </summary>
    public required DateOnly Date { get; init; }

    /// <summary>
    /// Creation Time
    /// </summary>
    public DateTime Created { get; init; } = DateTime.Now;

    /// <summary>
    /// Creator of entry
    /// </summary>
    public Guid? Creator { get; init; }
}