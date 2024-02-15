using DayFlags.Server.Rest;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Server.Utils;

public static class PaginationHelper
{
    /// <summary>
    /// Applies Paging to an EF Core query
    /// </summary>
    /// <typeparam name="TPayload">Payload Type</typeparam>
    /// <param name="query">Query</param>
    /// <param name="pagingParameters">Paging Parameters</param>
    /// <returns>A paginated result for the requested data</returns>
    public static async ValueTask<PagingResponse<TPayload>> AsPaginationResponseAsync<TPayload>(
        this IQueryable<TPayload> query,
        PagingParameters pagingParameters
    )
    {
        var elementCount = await query.CountAsync();
        var skip = pagingParameters.PageSize * (pagingParameters.Page - 1);
        var dataQuery = query
            .Skip(skip)
            .Take(pagingParameters.PageSize)
            .ToListAsync();

        var totalPages = Math.Ceiling((decimal)elementCount / pagingParameters.PageSize);

        return new PagingResponse<TPayload>(
            pagingParameters.Page,
            pagingParameters.PageSize,
            (int)totalPages,
            await dataQuery
        );
    }

    /// <summary>
    /// Applies Paging and an Mapster based converting on the data
    /// </summary>
    /// <typeparam name="TResponse">Type of response</typeparam>
    /// <typeparam name="TPayload">Type of data</typeparam>
    /// <param name="query">Query</param>
    /// <param name="pagingParameters">Paging Parameters</param>
    /// <returns>Expected model</returns>
    public static ValueTask<PagingResponse<TResponse>> AsConvertedPaginationResponseAsync<TResponse, TPayload>(
        this IQueryable<TPayload> query,
        PagingParameters pagingParameters
    )
    {
        return AsConvertedPaginationResponseAsync<TResponse, TPayload>(
            query,
            d => d.Adapt<TResponse>(),
            pagingParameters
        );
    }

    public static async ValueTask<PagingResponse<TResponse>> AsConvertedPaginationResponseAsync<TResponse, TPayload>(
        this IQueryable<TPayload> query,
        Func<TPayload, TResponse> converter,
        PagingParameters pagingParameters
    )
    {
        var data = await query.AsPaginationResponseAsync(pagingParameters);

        return new PagingResponse<TResponse>(
            data.Page,
            data.PageSize,
            data.TotalPages,
            data.Items
                .Select(converter)
        );
    }

}