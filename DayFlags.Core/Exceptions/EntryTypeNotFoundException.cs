using DayFlags.Core.Models;
using Microsoft.AspNetCore.Http;

namespace DayFlags.Core.Exceptions;

/// <summary>
/// Signals, that a <see cref="EntryType"/> was not found
/// </summary>
[System.Serializable]
public class EntryTypeNotFoundException : ARestException
{
    public EntryTypeNotFoundException(string entryTypeId) : base(
        $"{nameof(EntryType)} not found"
    )
    {
        Detail = $"The {nameof(EntryType)} with the id '{entryTypeId}' was not found";
    }

    public override int StatusCode => StatusCodes.Status404NotFound;
}