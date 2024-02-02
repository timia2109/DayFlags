using System.ComponentModel.DataAnnotations;

namespace DayFlags.Server.Rest;

public record FlagTypePayload
{
    [StringLength(64)]
    public required string FlagTypeKey { get; set; }

    /// <summary>
    /// A human friendly description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Parent group (if any)
    /// </summary>
    public Guid? FlagGroupId { get; set; }
}