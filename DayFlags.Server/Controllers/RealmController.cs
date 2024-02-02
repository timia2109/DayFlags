using DayFlags.Core.Models;
using DayFlags.Core.Repositories;
using DayFlags.Server.Rest;
using DayFlags.Server.Utils;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DayFlags.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RealmController(IRealmRepository realmRepository) : ControllerBase
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
        return realm is null ? NotFound() : Ok(realm.Adapt<RealmResponse>());
    }

    /// <summary>
    /// Creates an Realm
    /// </summary>
    /// <param name="realmPayload">The new realm</param>
    [HttpPost]
    public async ValueTask<IActionResult> CreateRealm(RealmPayload realmPayload)
    {
        var realm = realmPayload.Adapt<Realm>();
        realm = await realmRepository.AddRealmAsync(realm);

        return CreatedAtAction(
            nameof(GetRealm),
            realm
        );
    }

}