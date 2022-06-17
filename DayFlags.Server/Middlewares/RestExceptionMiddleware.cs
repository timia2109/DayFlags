using DayFlags.Core.Exceptions;

namespace DayFlags.Server.Middlewares;

/// <summary>
/// Writes a <see cref="ARestException"/> to the client
/// /// </summary>
public class RestExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public RestExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ARestException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(
                ex.ProblemDetails
            );
        }
    }

}