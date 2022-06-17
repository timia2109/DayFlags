using System.ComponentModel.DataAnnotations;
using DayFlags.Core.Enums;

namespace DayFlags.Core.Models;

/// <summary>
/// A EntryType represent a Type of Entries that can occour on a day
/// </summary>
public class EntryType
{
    public const int IdLength = 64;

    /// <summary>
    /// Id of this EntryType
    /// </summary>
    [Key]
    [StringLength(IdLength)]
    public string EntryTypeId { get; set; }

    /// <summary>
    /// Id of the Provider (if this is a EntryType delivered by a provider)
    /// </summary>
    [StringLength(128)]
    public string? ProviderId { get; set; }

    /// <summary>
    /// Beschreibung
    /// </summary>
    /// <value></value>
    public string? Description { get; set; }

    /// <summary>
    /// Requirement of this EntryType
    /// </summary>
    public EntryTypeRequirement Requirement { get; set; }

    /// <summary>
    /// Threshold (only used on Requirement = Threshold)
    /// </summary>
    public int? Threshold { get; set; }

    public EntryType(string entryTypeId,
        EntryTypeRequirement requirement,
        string? providerId = null,
        string? description = null)
    {
        EntryTypeId = entryTypeId;
        Requirement = requirement;
        ProviderId = providerId;
        Description = description;
    }
}