namespace DayFlags.Server.Rest;

/// <summary>
/// Generic Pagination Response
/// </summary>
/// <typeparam name="TPayload">Type of data</typeparam>
/// <param name="Page">Page number</param>
/// <param name="PageSize">Size of pages</param>
/// <param name="TotalPages">Total Pages</param>
/// <param name="Items">Item response</param>
public record PagingResponse<TPayload>(
    int Page,
    int PageSize,
    int TotalPages,
    IEnumerable<TPayload> Items
) : PagingParameters(Page, PageSize);