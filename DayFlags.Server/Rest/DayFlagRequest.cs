using System.ComponentModel.DataAnnotations;

namespace DayFlags.Server.Rest;

public class DayFlagPayload
{

    /// <summary>
    /// Related DayFlag
    /// </summary>
    [StringLength(64)]
    public required string FlagTypeKey { get; set; }

    /// <summary>
    /// Affected Date
    /// </summary>
    public required DateOnly Date { get; init; }
}

public class DayFlagResponse : DayFlagPayload
{
    /// <summary>
    /// Id of this DayFlag
    /// </summary>
    public Guid FlagId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Creation Time
    /// </summary>
    public DateTime Created { get; init; } = DateTime.Now;
}