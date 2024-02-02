using System.ComponentModel.DataAnnotations;

namespace DayFlags.Server.Rest;

public record RealmPayload
{
    /// <summary>
    /// Label for this realm
    /// </summary>
    [StringLength(64)]
    public required string Label { get; set; }
}

public record RealmResponse : RealmPayload
{
    /// <summary>
    /// The id of this realm
    /// </summary>
    public Guid RealmId { get; init; }
}