using Microsoft.AspNetCore.Mvc;

namespace DayFlags.Core.Exceptions;

/// <summary>
/// A exception which should delivered as <see cref="ProblemDetails" /> to the
/// user
/// </summary>
public abstract class ARestException : Exception
{
    /// <summary>
    /// Title of this Exception
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Additional Details
    /// </summary>
    public string? Detail { get; set; }

    /// <summary>
    /// Response Status Code
    /// </summary>
    public abstract int StatusCode { get; }

    public ARestException(string title) : base(title)
    {
        Title = title;
    }

    /// <summary>
    /// Returns this exception as ProblemDetails
    /// </summary>
    public ProblemDetails ProblemDetails => new ProblemDetails
    {
        Detail = Detail,
        Title = Title,
        Status = StatusCode
    };

}