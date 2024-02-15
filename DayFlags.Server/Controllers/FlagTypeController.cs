using System.ComponentModel.DataAnnotations;
using DayFlags.Core.Models;
using DayFlags.Core.Repositories;
using DayFlags.Server.Rest;
using DayFlags.Server.Utils;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DayFlags.Server.Controllers;

[ApiController]
public class FlagTypeController(IRealmRepository realmRepository,
    IFlagTypeRepository flagTypeRepository,
    IFlagGroupRepository flagGroupRepository) : BaseRealmRelatedController(realmRepository)
{

    /// <summary>
    /// Gets all FlagTypes of this realm
    /// </summary>
    /// <param name="realmId">RealmId</param>
    /// <param name="pagingParameters">Paging</param>
    /// <response code="200">FlagTypes</response>
    [HttpGet]
    [ProducesResponseType<PagingResponse<FlagTypePayload>>(200)]
    public async ValueTask<IActionResult> GetFlagTypes(
        Guid realmId,
        [FromQuery] PagingParameters pagingParameters
    )
    {
        var query = flagTypeRepository
            .GetFlagTypesQuery(Realm)
            .Include(e => e.FlagGroup);

        var response = await query
            .AsConvertedPaginationResponseAsync<FlagTypePayload, FlagType>(
                ToApi,
                pagingParameters);

        return Ok(response);
    }

    /// <summary>
    /// Gets a single FlagType
    /// </summary>
    /// <param name="realmId">RealmId</param>
    /// <param name="flagTypeKey">Key of the FlagType</param>
    /// <response code="200">The searched FlagType</response>
    /// <response code="404">The FlagType was not found</response>
    [HttpGet("{flagTypeKey}")]
    [ProducesResponseType<FlagTypePayload>(200)]
    [ProducesResponseType<ProblemDetails>(404)]
    public async ValueTask<IActionResult> GetFlagType(
        Guid realmId,
        [StringLength(64)] string flagTypeKey
    )
    {
        var flagType = await FindFlagTypeAsync(flagTypeKey);
        return Ok(ToApi(flagType));
    }

    /// <summary>
    /// Creates FlagType
    /// </summary>
    /// <param name="realmId">RealmId</param>
    /// <response code="200">The FlagType was created</response>
    /// <response code="409">The FlagTypeKey is already used in this realm</response>
    [HttpPost]
    [ProducesResponseType<FlagTypePayload>(201)]
    public async ValueTask<IActionResult> CreateFlagType(
        Guid realmId,
        FlagTypePayload payload
    )
    {
        await CheckIfKeyExistsAsync(payload.FlagTypeKey);

        var flagType = await ToDatabase(payload);
        await flagTypeRepository.AddFlagTypeAsync(flagType);

        return CreatedAtAction(
            nameof(GetFlagType),
            new { realmId = Realm.RealmId, flagTypeKey = flagType.FlagTypeKey },
            ToApi(flagType)
        );
    }

    /// <summary>
    /// Updates a FlagType
    /// </summary>
    /// <param name="realmId">RealmId</param>
    /// <param name="flagTypeKey">The FlagTypeKey</param>
    /// <response code="200">The FlagType was updated</response>
    /// <response code="409">The new FlagTypeKey is already used in this realm</response>
    [HttpPut("{flagTypeKey}")]
    [ProducesResponseType<FlagTypePayload>(200)]
    [ProducesResponseType<ProblemDetails>(404)]
    public async ValueTask<IActionResult> UpdateFlagType(
        Guid realmId,
        [StringLength(64)] string flagTypeKey,
        FlagTypePayload payload
    )
    {
        var flagType = await FindFlagTypeAsync(flagTypeKey);

        if (flagType.FlagTypeKey != payload.FlagTypeKey)
            await CheckIfKeyExistsAsync(payload.FlagTypeKey);

        var target = await ToDatabase(payload, flagType);

        target.Adapt(flagType);
        flagType = await flagTypeRepository.UpdateFlagTypeAsync(flagType);

        return Ok(
            ToApi(flagType)
        );
    }

    /// <summary>
    /// Deletes a FlagType
    /// </summary>
    /// <param name="realmId">RealmId</param>
    /// <param name="flagTypeKey">Key of the FlagType</param>
    /// <response code="200">The FlagType was deleted</response>
    /// <response code="404">The FlagType was not found</response>
    [HttpDelete("{flagTypeKey}")]
    [ProducesResponseType<FlagTypePayload>(200)]
    [ProducesResponseType<ProblemDetails>(404)]
    public async ValueTask<IActionResult> DeleteFlagType(
        Guid realmId,
        [StringLength(64)] string flagTypeKey
    )
    {
        var flagType = await FindFlagTypeAsync(flagTypeKey);
        await flagTypeRepository.DeleteFlagTypeAsync(flagType);

        return Ok(
            ToApi(flagType)
        );
    }

    private async Task<FlagType> ToDatabase(FlagTypePayload payload,
        FlagType? loaded = null
    )
    {
        var flagType =
            loaded is null
            ? payload.Adapt<FlagType>()
            : payload.Adapt(loaded);

        flagType.Realm = Realm;

        if (payload.FlagGroupKey is not null)
        {
            var flagGroup = await flagGroupRepository.GetFlagGroupAsync(
                Realm,
                payload.FlagGroupKey
            ) ?? throw new ResultException(Results.NotFound(new ProblemDetails
            {
                Title = "FlagGroup not found",
                Detail = $"FlagGroup with key {payload.FlagGroupKey} does not exist in realm {Realm.RealmId}"
            }));
            flagType.FlagGroup = flagGroup;
        }

        return flagType;
    }

    private FlagTypePayload ToApi(FlagType flagType)
    {
        var payload = flagType.Adapt<FlagTypePayload>();
        payload.FlagGroupKey = flagType.FlagGroup?.FlagGroupKey;
        return payload;
    }

    private async ValueTask CheckIfKeyExistsAsync(string flagTypeKey)
    {
        var existingType = await flagTypeRepository.GetFlagTypeAsync(
            Realm,
            flagTypeKey
        );

        if (existingType is not null) throw new ResultException(
            Results.Conflict(new ProblemDetails
            {
                Title = "FlagTypeKey already exists",
                Detail = $"A FlagType with the key {flagTypeKey} already exist in this realm"
            })
        );
    }

    private async ValueTask<FlagType> FindFlagTypeAsync(string flagTypeKey)
    {
        return await flagTypeRepository
            .GetFlagTypeAsync(Realm, flagTypeKey)
            ?? throw new ResultException(
            Results.NotFound(new ProblemDetails
            {
                Title = "FlagTypeKey not found",
                Detail = $"The FlagTypeKey {flagTypeKey} does not exist in the Realm {Realm.RealmId}"
            })
        );
    }

}