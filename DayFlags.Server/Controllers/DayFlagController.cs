using DayFlags.Core.Repositories;
using DayFlags.Server.Rest;
using DayFlags.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace DayFlags.Server.Controllers;

public class DayFlagController(IRealmRepository realmRepository,
    DayFlagApiService apiService) : BaseRealmRelatedController(realmRepository)
{
    [HttpGet]
    [ProducesResponseType<PagingResponse<DayFlagResponse>>(200)]
    public async Task<IActionResult> GetDayFlags(Guid realmId, [FromQuery] DayFlagQuery query)
    {
        return Ok(await apiService.ExecuteQueryAsync(Realm, query));
    }

    [HttpGet("{dayFlagId}")]
    [ProducesResponseType<PagingResponse<DayFlagResponse>>(200)]
    public async Task<IActionResult> GetDayFlag(Guid realmId, Guid dayFlagId)
    {
        var result = await apiService.GetSingleAsync(Realm, dayFlagId);

        return result == null ? NotFound(new ProblemDetails
        {
            Detail = $"DayFlag with Id {dayFlagId} not found",
            Title = "DayFlag not found"
        }) : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDayFlag(Guid realmId, [FromBody] DayFlagPayload request)
    {
        var response = await apiService.CreateAsync(Realm, request);
        return CreatedAtAction(nameof(GetDayFlag), new
        {
            realmId = Realm.RealmId,
            dayFlagId = response.FlagId
        }, response);
    }

}