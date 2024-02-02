using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DayFlags.Server.Utils;

/// <summary>
/// An exception which contains a <see cref="IResult"/>, which get responded to the
/// request
/// </summary>
public class ResultException : Exception
{
    private readonly IResult _result;

    public ResultException(IResult result)
    {
        _result = result;
    }

    public async Task HandleAsync(HttpContext httpContent)
    {
        await _result.ExecuteAsync(httpContent);
    }

}

public class ResultExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ResultException res) return false;
        await res.HandleAsync(httpContext);
        return true;
    }
}
