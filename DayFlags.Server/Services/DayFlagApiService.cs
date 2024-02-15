using DayFlags.Core;
using DayFlags.Core.Exceptions;
using DayFlags.Core.Models;
using DayFlags.Core.Repositories;
using DayFlags.Server.Rest;
using DayFlags.Server.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Server.Services;

/// <summary>
/// Utility helper to use DayFlags threw the API
/// </summary>
/// <param name="dayFlagRepository"></param>
/// <param name="flagTypeRepository"></param>
/// <param name="db"></param>
public class DayFlagApiService(
    IDayFlagRepository dayFlagRepository,
    IFlagTypeRepository flagTypeRepository,
    DayFlagsDb db
)
{
    private async Task<DayFlag> ToDatabase(Realm realm, DayFlagPayload request)
    {
        var flagTypeId = await flagTypeRepository.GetFlagTypeAsync(realm,
            request.FlagTypeKey)
            ?? throw new ResultException(Results.NotFound(new ProblemDetails
            {
                Title = "Flag type not found",
                Detail = $"Flag type with key {request.FlagTypeKey} not found"
            }));

        return new DayFlag
        {
            Date = request.Date,
            FlagTypeId = flagTypeId.FlagTypeId,
            Created = DateTime.UtcNow,
        };
    }

    private async Task<DayFlagResponse> ToApi(DayFlag dayFlag)
    {
        var flagType = await flagTypeRepository
            .GetFlagTypeAsync(dayFlag.FlagTypeId);

        return new DayFlagResponse
        {
            Date = dayFlag.Date,
            FlagTypeKey = flagType!.FlagTypeKey,
            Created = dayFlag.Created,
            FlagId = dayFlag.FlagId
        };
    }

    public async Task<DayFlagResponse> CreateAsync(Realm realm, DayFlagPayload request)
    {
        var dayFlag = await ToDatabase(realm, request);
        try
        {
            var result = await dayFlagRepository.AddDayFlagAsync(dayFlag);
            return await ToApi(result);
        }
        catch (FlagGroupEntryExistException)
        {
            throw new ResultException(Results.BadRequest(new ProblemDetails
            {
                Title = "FlagGroup already have an entry on the date",
                Detail = "TODO" // TODO
            }));
        }
    }

    public async Task<DayFlagResponse?> GetSingleAsync(Realm realm, Guid dayFlagId)
    {
        var item = await db.DayFlags
            .SingleOrDefaultAsync(e => e.FlagId == dayFlagId);

        return item == null ? null : await ToApi(item);
    }

    public async Task<PagingResponse<DayFlagResponse>> ExecuteQueryAsync(
        Realm realm,
        DayFlagQuery apiQuery
    )
    {
        IQueryable<DayFlag> query;

        if (apiQuery.FlagTypeKeys is not null)
        {
            query = dayFlagRepository.GetDayFlagsQueryByFlagTypeKeys(
                realm, apiQuery.FlagTypeKeys, apiQuery.AsDateRange());
        }
        else if (apiQuery.FlagGroupKeys is not null)
        {
            query = dayFlagRepository.GetDayFlagsQueryByFlagGroupKeys(
                realm, apiQuery.FlagGroupKeys, apiQuery.AsDateRange());
        }
        else
        {
            query = dayFlagRepository.GetDayFlagsQuery(realm,
                apiQuery.AsDateRange());
        }

        var pagingItems = await query.AsPaginationResponseAsync(apiQuery);

        var flagTypes = pagingItems.Items
            .Select(e => e.FlagTypeId);

        var flagTypeNames = await db.FlagTypes
            .Where(e => flagTypes.Contains(e.FlagTypeId))
            .ToDictionaryAsync(k => k.FlagTypeId, v => v.FlagTypeKey);

        return new PagingResponse<DayFlagResponse>(
            Items: pagingItems.Items.Select(e => new DayFlagResponse
            {
                Date = e.Date,
                FlagTypeKey = flagTypeNames[e.FlagTypeId],
                Created = e.Created,
                FlagId = e.FlagId
            }),
            Page: pagingItems.Page,
            PageSize: pagingItems.PageSize,
            TotalPages: pagingItems.TotalPages
        );
    }
}