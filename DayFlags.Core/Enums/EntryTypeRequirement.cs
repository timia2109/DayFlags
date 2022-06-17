namespace DayFlags.Core.Enums;

/// <summary>
/// Defining how often a DayEntry must occur until this EntryType is true for a given date
/// </summary>
public enum EntryTypeRequirement
{
    /// <summary>
    /// Using a entry defined threshold
    /// </summary>
    Threshold,

    //// <summary>
    /// Once a day
    /// </summary>
    OnceADay,

    /// <summary>
    /// Once a week. (Every day in the week is matched)
    /// </summary>
    OnceAWeek,

    /// <summary>
    /// Once a month. (Every day in the month is matched)
    /// </summary>
    OnceAMonth
}