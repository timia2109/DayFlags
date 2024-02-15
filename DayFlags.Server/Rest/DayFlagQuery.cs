using DayFlags.Core.Util;

namespace DayFlags.Server.Rest;

public record DayFlagQuery : PagingParameters
{
    /// <summary>
    /// Filter Flags with the FlagTypes Keys
    /// </summary>
    public string[]? FlagTypeKeys { get; init; }

    /// <summary>
    /// Filter Flags with the FlagGroup Keys
    /// </summary>
    public string[]? FlagGroupKeys { get; init; }

    /// <summary>
    /// Specify the MinDate (including)
    /// </summary>
    public DateOnly? MinDate { get; init; }

    /// <summary>
    /// Specify the MaxDate (including)
    /// </summary>
    public DateOnly? MaxDate { get; init; }

    public DateRange AsDateRange()
    {
        return new DateRange(MinDate ?? DateOnly.MinValue,
            MaxDate ?? DateOnly.MaxValue);
    }
}