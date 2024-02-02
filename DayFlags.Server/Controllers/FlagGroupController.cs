using System.ComponentModel.DataAnnotations;
using DayFlags.Core.Models;
using DayFlags.Core.Repositories;
using DayFlags.Server.Rest;
using DayFlags.Server.Utils;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DayFlags.Server.Controllers;

[ApiController]
public class FlagGroupController(IRealmRepository realmRepository,
    IFlagGroupRepository flagGroupRepository) : BaseRealmRelatedController(realmRepository)
{

    /// <summary>
    /// Get all FlagGroups in this realm
    /// </summary>
    /// <param name="realmId">Realm ID</param>
    /// <param name="pagingParameters">Pagination</param>
    /// <response code="200">The FlagGroups</response>
    [HttpGet]
    [ProducesResponseType<PagingResponse<FlagGroupPayload>>(200)]
    public async ValueTask<IActionResult> GetFlagGroups(
        Guid realmId,
        [FromQuery] PagingParameters pagingParameters
    )
    {
        var query = flagGroupRepository.GetFlagGroupsQuery(Realm);

        return Ok(await query
            .AsConvertedPaginationResponseAsync<FlagGroupPayload, FlagGroup>(pagingParameters)
        );
    }

    /// <summary>
    /// Gets a single FlagGroup
    /// </summary>
    /// <param name="realmId">Realm ID</param>
    /// <param name="flagGroupKey">Key for the group</param>
    /// <response code="200">FlagGroup</response>
    /// <response code="404">FlagGroup not found</response>
    [HttpGet("{flagGroupKey}")]
    [ProducesResponseType<FlagGroupPayload>(200)]
    [ProducesResponseType<ProblemDetails>(404)]
    public async ValueTask<IActionResult> GetFlagGroup(
        Guid realmId,
        [StringLength(64)] string flagGroupKey
    )
    {
        var flagGroup = await FindFlagGroupAsync(flagGroupKey);
        return AsGetResult<FlagGroupPayload>(flagGroup);
    }

    /// <summary>
    /// Creates a FlagGroup
    /// </summary>
    /// <param name="realmId">Realm Id</param>
    /// <param name="payload">Payload</param>
    /// <response code="201">The FlagGroup was created</response>
    /// <response code="409">The FlagGroupKey is already used</response>
    [HttpPost]
    [ProducesResponseType<FlagGroupPayload>(201)]
    [ProducesResponseType<ProblemDetails>(409)]
    public async ValueTask<IActionResult> CreateFlagGroup(
        Guid realmId,
        FlagGroupPayload payload
    )
    {
        await CheckIfKeyExistsAsync(payload.FlagGroupKey);

        var flagGroup = payload.Adapt<FlagGroup>();
        flagGroup.Realm = Realm;
        await flagGroupRepository.AddFlagGroupAsync(flagGroup);

        return CreatedAtAction(
            nameof(GetFlagGroup),
            new { realmId = Realm.RealmId, flagGroupKey = flagGroup.FlagGroupKey },
            flagGroup.Adapt<FlagGroupPayload>()
        );
    }

    /// <summary>
    /// Updates a flag gruop
    /// </summary>
    /// <param name="realmId">Realm ID</param>
    /// <param name="flagGroupKey">Key of the FlagGroup</param>
    /// <param name="payload">Payload</param>
    /// <response code="200">The FlagGroup was updated</response>
    /// <response code="404">FlagGroup not found</response>
    [HttpPut("{flagGroupKey}")]
    [ProducesResponseType<FlagGroupPayload>(200)]
    [ProducesResponseType<ProblemDetails>(404)]
    public async ValueTask<IActionResult> UpdateFlagGroup(
        Guid realmId,
        [StringLength(64)] string flagGroupKey,
        FlagGroupPayload payload
    )
    {
        var flagGroup = await FindFlagGroupAsync(flagGroupKey);

        if (flagGroup.FlagGroupKey != payload.FlagGroupKey)
            await CheckIfKeyExistsAsync(payload.FlagGroupKey);

        payload.Adapt(flagGroup);
        flagGroup = await flagGroupRepository.UpdateFlagGroupAsync(flagGroup);

        return Ok(
            flagGroup.Adapt<FlagGroupPayload>()
        );
    }

    /// <summary>
    /// Deletes a FlagGroup
    /// </summary>
    /// <param name="realmId">Realm Id</param>
    /// <param name="flagGroupKey">Key of the FlagGroup</param>
    /// <response code="200">The FlagGroup was deleted</response>
    /// <response code="404">FlagGroup not found</response>
    [HttpDelete("{flagGroupKey}")]
    [ProducesResponseType<FlagGroupPayload>(200)]
    [ProducesResponseType<ProblemDetails>(404)]
    public async ValueTask<IActionResult> DeleteFlagType(
        Guid realmId,
        [StringLength(64)] string flagGroupKey
    )
    {
        var flagGroup = await FindFlagGroupAsync(flagGroupKey);
        await flagGroupRepository.DeleteFlagGroupAsync(flagGroup);

        return Ok(
            flagGroup.Adapt<FlagGroupPayload>()
        );
    }

    /// <summary>
    /// Get all children FlagType of this FlagGroup
    /// </summary>
    /// <param name="realmId">Realm ID</param>
    /// <param name="flagGroupKey">FlagGroupKey</param>
    /// <param name="pagingParameters">Paging parameters</param>
    /// <response code="200">Children</response>
    /// <response code="404">FlagGroup not found</response>
    [HttpGet("{flagGroupKey}/FlagType")]
    [ProducesResponseType<PagingResponse<FlagTypePayload>>(200)]
    public async ValueTask<IActionResult> GetChildrenAsync(
        Guid realmId,
        [StringLength(64)] string flagGroupKey,
        [FromQuery] PagingParameters pagingParameters
    )
    {
        var flagGroup = await FindFlagGroupAsync(flagGroupKey);
        var children = flagGroupRepository
            .GetChildrenFlagTypesQuery(flagGroup);

        var pageResult = await children
            .AsPaginationResponseAsync(pagingParameters);

        return Ok(new PagingResponse<FlagTypePayload>(
            pageResult.Page,
            pageResult.PageSize,
            pageResult.TotalPages,
            pageResult.Items.Select(e =>
            {
                var api = e.Adapt<FlagTypePayload>();
                api.FlagGroupKey = flagGroupKey;
                return api;
            })
        ));
    }

    private async ValueTask CheckIfKeyExistsAsync(string flagGroupKey)
    {
        var existingType = await flagGroupRepository.GetFlagGroupAsync(
            Realm,
            flagGroupKey
        );

        if (existingType is not null) throw new ResultException(
            Results.Conflict(new ProblemDetails
            {
                Title = "FlagGroupKey already exists",
                Detail = $"A FlagGroup with the key {flagGroupKey} already exist in the Realm {Realm.RealmId}"
            })
        );
    }

    private async ValueTask<FlagGroup> FindFlagGroupAsync(string flagGroupKey)
    {
        return await flagGroupRepository
            .GetFlagGroupAsync(Realm, flagGroupKey)
            ?? throw new ResultException(
            Results.NotFound(new ProblemDetails
            {
                Title = "FlagGroupKey not found",
                Detail = $"The FlagGroupKey {flagGroupKey} does not exist in the Realm {Realm.RealmId}"
            })
        );
    }

}