using System.ComponentModel.DataAnnotations;
using DayFlags.Core.Models;
using DayFlags.Core.Repositories;
using DayFlags.Server.Rest;
using DayFlags.Server.Utils;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DayFlags.Server.Controllers;

[ApiController]
[Route("api/Realm/{realmId}/[controller]")]
public class FlagTypeController(IRealmRepository realmRepository,
    IFlagTypeRepository flagTypeRepository) : BaseRealmRelatedController(realmRepository)
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
        return Ok(
            await flagTypeRepository.GetFlagTypesQuery(Realm)
                .AsConvertedPaginationResponseAsync<FlagTypePayload, FlagType>(pagingParameters)
        );
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
        var flagType = await flagTypeRepository
            .GetFlagTypeAsync(Realm, flagTypeKey);

        return AsGetResult<FlagTypePayload>(flagType);
    }

    /// <summary>
    /// Creates FlagType
    /// </summary>
    /// <param name="realmId">RealmId</param>
    /// <response code="200">The FlagType was created</response>
    /// <response code="409">The FlagTypeKey is already used in this realm</response>
    [HttpPost]
    public async ValueTask<IActionResult> CreateFlagType(
        Guid realmId,
        FlagTypePayload payload
    )
    {
        await CheckIfKeyExistsAsync(payload.FlagTypeKey);

        var flagType = payload.Adapt<FlagType>();
        flagType.Realm = Realm;
        await flagTypeRepository.AddFlagTypeAsync(flagType);

        return CreatedAtAction(
            nameof(GetFlagType),
            new { realmId = Realm.RealmId, flagTypeKey = flagType.FlagTypeKey },
            flagType.Adapt<FlagTypePayload>()
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
    public async ValueTask<IActionResult> UpdateFlagType(
        Guid realmId,
        [StringLength(64)] string flagTypeKey,
        FlagTypePayload payload
    )
    {
        var flagType = await FindFlagTypeAsync(flagTypeKey);

        if (flagType.FlagTypeKey != payload.FlagTypeKey)
            await CheckIfKeyExistsAsync(payload.FlagTypeKey);

        payload.Adapt(flagType);
        flagType = await flagTypeRepository.UpdateFlagTypeAsync(flagType);

        return Ok(
            flagType.Adapt<FlagTypePayload>()
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
    public async ValueTask<IActionResult> DeleteFlagType(
        Guid realmId,
        [StringLength(64)] string flagTypeKey
    )
    {
        var flagType = await FindFlagTypeAsync(flagTypeKey);
        await flagTypeRepository.DeleteFlagTypeAsync(flagType);

        return Ok(
            flagType.Adapt<FlagTypePayload>()
        );
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