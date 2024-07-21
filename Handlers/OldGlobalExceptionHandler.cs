using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApiErrorHandlingMiddleware.Handlers;

// This global exception handling middleware can be written using duck typed syntax of the middleware
public class OldGlobalExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Old Global Exception handler!");
            Console.WriteLine(exception.Message);

            var problemDetails = new ProblemDetails
            {
                Status = (int) HttpStatusCode.InternalServerError,
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

            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(responseJson);

            return;
        }
    }
}