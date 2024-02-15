using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayFlags.Core.Models;

/// <summary>
/// A <see cref="FlagGroup"/> represents a logical group of related tags
/// </summary>
public record FlagGroup
{
    /// <summary>
    /// Technical Id
    /// </summary>
    [Key]
    public Guid FlagGroupId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// User-friendly Key of this <see cref="FlagGroup"/> 
    /// Used for Access and API calls
    /// </summary>
    [StringLength(64)]
    public required string FlagGroupKey { get; set; }

    /// <summary>
    /// A human friendly description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Controls if only a single entry per day is allowed
    /// </summary>
    public bool SingleFlagPerDay { get; set; }

    /// <summary>
    /// Relation to <see cref="Realm"/>
    /// </summary> 
    public required Guid RealmId { get; init; }

    /// <summary>
    /// Relation to Realm
    /// </summary>
    [ForeignKey(nameof(RealmId))]
    public Realm? Realm { get; set; }

    /// <summary>
    /// Children of this <see cref="FlagGroup"/> 
    /// </summary>
    public List<FlagType>? Children { get; set; }

}