using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DayFlags.Server.Controllers;

public abstract class RestBaseController : ControllerBase
{
    protected NotFoundObjectResult ObjectNotFound()
    {
        return NotFound(new ProblemDetails
        {
            Title = "The searched object was not found"
        });
    }

    protected IActionResult AsGetResult<TResponse>(
        object? entity
    )
    {
        return entity is null ? ObjectNotFound() : Ok(entity.Adapt<TResponse>());
    }
}