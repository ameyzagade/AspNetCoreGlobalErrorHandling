using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApiErrorHandlingMiddleware.Handlers;

public class NewGlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine("New Global Exception handler!");
        Console.WriteLine(exception.Message);

        // Before generating response, the control flow is handed over to the next handler in the chain.
        // If the next exception handler is not setup, a default problem details response is sent to the client.
        if (exception is NotImplementedException)
        {
            return false;
        }

        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "Internal Server Error",
            Detail = "Some internal server error occurred while processing the request",
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

        return false;
    }
}
