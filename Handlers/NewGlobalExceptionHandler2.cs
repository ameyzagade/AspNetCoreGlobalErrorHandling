using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApiErrorHandlingMiddleware.Handlers;

public class NewGlobalExceptionHandler2 : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine("New Global Exception handler #2!");
        Console.WriteLine(exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "Internal Server Error",
            Detail = "Some internal server error occurred while processing the request. Handled by exception handler #2",
            Type = "http://localhost/help",
            Instance = nameof(ProblemDetails),
            Extensions = new Dictionary<string, object?>
                {
                    { "exceptionDetails", exception.Message }
                }

        };

        var responseJson = JsonSerializer.Serialize(problemDetails);

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(responseJson, cancellationToken: cancellationToken);

        return true;
    }
}
