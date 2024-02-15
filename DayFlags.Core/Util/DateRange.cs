namespace DayFlags.Core.Util;

public sealed record DateRange(DateOnly Start, DateOnly End)
{
    public static DateRange SingleDay(DateOnly date)
        => new(date, date);
}