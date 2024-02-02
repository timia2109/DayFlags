using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayFlags.Core.Models;

/// <summary>
///     A FlagType is a type of flags 
/// </summary> 
public record FlagType
{

    /// <summary>
    /// Technical Id of this FlagType
    /// </summary>
    [Key]
    public Guid FlagTypeId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// User-friendly Key
    /// Used for Access and API calls
    /// </summary>
    [StringLength(64)]
    public required string FlagTypeKey { get; set; }

    /// <summary>
    /// A human friendly description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Relation to <see cref="Realm"/>
    /// </summary> 
    public Guid RealmId { get; init; }

    /// <summary>
    /// Relation to Realm
    /// </summary>
    [ForeignKey(nameof(RealmId))]
    public Realm? Realm { get; set; }

    /// <summary>
    /// Parent group
    /// </summary>
    public Guid? FlagGroupId { get; set; }

    /// <summary>
    /// Parent group relation
    /// </summary>
    [ForeignKey(nameof(FlagGroupId))]
    public FlagGroup? FlagGroup { get; set; }
}