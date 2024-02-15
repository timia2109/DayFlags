using System.ComponentModel.DataAnnotations;

namespace DayFlags.Server.Rest;

public record FlagGroupPayload
{
    /// <summary>
    /// User-friendly Key of this <see cref="FlagGroup"/> 
    /// Used for Access and API calls
    [StringLength(64)]
    public required string FlagGroupKey { get; set; }

    /// <summary>
    /// A human friendly description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Controls if only a single entry per day is allowed
    /// </summary>
    public bool SingleFlagPerDay { get; set; } = false;
}