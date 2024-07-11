namespace DayFlags.Core.Models;

/// <summary>
/// Defines an Priority for the exposed value
/// </summary>
/// <param name="value">Value</param>
public struct Priority(int value)
{
    /// <summary>
    /// Value of this Priority (higher is better)
    /// </summary>
    public int Value { get; } = value;

    /// <summary>
    /// Default Priority
    /// </summary>
    public static Priority Default => new(100);

    /// <summary>
    /// Default Priority for Database entities
    /// </summary>
    public static Priority Database => new(300);
}