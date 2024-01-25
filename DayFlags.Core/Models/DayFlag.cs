using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayFlags.Core.Models;

/// <summary>
/// Represent a entry for a day
/// </summary>
public record DayFlag
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
    public FlagType? FlagType { get; set; }

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