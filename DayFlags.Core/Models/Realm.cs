using System.ComponentModel.DataAnnotations;

namespace DayFlags.Core.Models;

public record Realm
{
    /// <summary>
    ///     Id of this Realm
    /// </summary>
    [Key]
    public Guid RealmId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Owner of this Realm
    /// </summary>
    public Guid? Owner { get; init; }

    /// <summary>
    /// A meaningful label of the realm
    /// </summary>
    [StringLength(64)]
    public string? Label { get; set; }
}