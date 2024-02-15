using DayFlags.Core.Models;
using DayFlags.Core.Repositories;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DayFlags.Server.Controllers;

[ApiController]
[Route("api/Realm/{realmId:guid}/[controller]")]
public abstract class BaseRealmRelatedController(IRealmRepository realmRepository)
    : RestBaseController, IAsyncActionFilter
{

    protected Realm Realm { get; private set; } = null!;

    [NonAction]
    public async Task OnActionExecutionAsync(ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var realmId = (Guid)context.ActionArguments["realmId"];
        var realm = await realmRepository.FindRealmAsync(realmId);

        if (realm is null)
        {
            var notFound = new NotFoundObjectResult(new ProblemDetails
            {
                Title = "Realm not found",
                Detail = $"The realm {realmId} does not exist"
            });
            await notFound.ExecuteResultAsync(context);
            return;
        }

        Realm = realm;
        await next();
    }
}