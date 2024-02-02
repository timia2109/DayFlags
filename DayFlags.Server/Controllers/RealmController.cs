using DayFlags.Core.Models;
using DayFlags.Core.Repositories;
using DayFlags.Server.Rest;
using DayFlags.Server.Utils;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DayFlags.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RealmController(IRealmRepository realmRepository) : RestBaseController
{

    /// <summary>
    /// Fetches all Realms
    /// </summary>
    /// <param name="pagingParameters">Pagination</param>
    /// <response code="200">All created realms</response>
    [HttpGet]
    [ProducesResponseType<PagingResponse<RealmResponse>>(200)]
    public async ValueTask<IActionResult> GetAllRealms(
        [FromQuery] PagingParameters pagingParameters
    )
    {
        return Ok(
            await realmRepository.GetRealmsQuery()
                .AsConvertedPaginationResponseAsync<RealmResponse, Realm>(pagingParameters)
        );
    }

    /// <summary>
    /// Gets a single Realm
    /// </summary>
    /// <param name="realmId">Id of the realm</param>
    /// <response code="200">Realm received</response>
    /// <response code="404">Realm not found</response>
    [HttpGet("{realmId}")]
    [ProducesResponseType<RealmResponse>(200)]
    [ProducesResponseType<ProblemDetails>(404)]
    public async ValueTask<IActionResult> GetRealm(Guid realmId)
    {
        var realm = await realmRepository.FindRealmAsync(realmId);
        return AsGetResult<RealmResponse>(realm);
    }

    /// <summary>
    /// Creates an Realm
    /// </summary>
    /// <param name="realmPayload">The new realm</param>
    [HttpPost]
    [ProducesResponseType<RealmResponse>(201)]
    public async ValueTask<IActionResult> CreateRealm(RealmPayload realmPayload)
    {
        var realm = realmPayload.Adapt<Realm>();
        realm = await realmRepository.AddRealmAsync(realm);

        return CreatedAtAction(
            nameof(GetRealm),
            new { realmId = realm.RealmId },
            realm.Adapt<RealmResponse>()
        );
    }

    /// <summary>
    /// Updates an Realm
    /// </summary>
    /// <param name="realmId">Affected Realm</param>
    /// <param name="payload">Updated Realm</param>
    /// <response code="200">Realm updated</response>
    /// <response code="404">Realm not found</response>
    [HttpPut("{realmId}")]
    [ProducesResponseType<RealmResponse>(200)]
    [ProducesResponseType<ProblemDetails>(404)]
    public async ValueTask<IActionResult> UpdateRealm(Guid realmId, RealmPayload payload)
    {
        var entity = await realmRepository.FindRealmAsync(realmId);
        if (entity is null) return ObjectNotFound();

        payload.Adapt(entity);
        entity = await realmRepository.UpdateRealmAsync(entity);
        return Ok(entity.Adapt<RealmResponse>());
    }

    /// <summary>
    /// Deletes a Realm
    /// </summary>
    /// <param name="realmId">Affected realm</param>
    /// <response code="200">Realm deleted</response>
    /// <response code="404">Realm not found</response>
    [HttpDelete("{realmId}")]
    [ProducesResponseType<RealmResponse>(200)]
    [ProducesResponseType<ProblemDetails>(404)]
    public async ValueTask<IActionResult> DeleteRealm(Guid realmId)
    {
        var entity = await realmRepository.FindRealmAsync(realmId);
        if (entity is null) return ObjectNotFound();

        await realmRepository.DeleteRealmAsync(entity);
        return Ok(entity.Adapt<RealmResponse>());
    }

}