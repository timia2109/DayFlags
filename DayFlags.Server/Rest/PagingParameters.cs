using System.ComponentModel.DataAnnotations;

namespace DayFlags.Server.Rest;

/// <summary>
/// Parameters for Pagination
/// </summary>
/// <param name="Page">Page number</param>
/// <param name="PageSize">Size of pages</param>
public record PagingParameters(
    int Page = 1,
    [Range(1, 200)] int PageSize = 50
);