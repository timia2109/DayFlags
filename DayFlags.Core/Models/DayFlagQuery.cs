using DayFlags.Core.Util;

namespace DayFlags.Core.Models;

public record DayFlagQuery
{
    /// <summary>
    /// Filter Flags with the FlagTypes Keys
    /// </summary>
    public string[]? FlagTypeKeys { get; set; }

    /// <summary>
    /// Filter Flags with the FlagGroup Keys
    /// </summary>
    public string[]? FlagGroupKeys { get; set; }

    /// <summary>
    /// Specify the MinDate (including)
    /// </summary>
    public DateOnly? MinDate { get; set; }

    /// <summary>
    /// Specify the MaxDate (including)
    /// </summary>
    public DateOnly? MaxDate { get; set; }

    /// <summary>
    /// Get the Date as Range
    /// </summary>
    public DateRange? DateRange
    {
        get => new DateRange(MinDate ?? DateOnly.MinValue, MaxDate ?? DateOnly.MaxValue);
        set
        {
            MinDate = value?.Start;
            MaxDate = value?.End;
        }
    }
}