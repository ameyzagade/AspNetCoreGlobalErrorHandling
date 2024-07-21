namespace AspNetCoreWebApiErrorHandlingMiddleware.MinimalApiEndpoints;

public static class MinimalApiEndpoints
{
    public static void MapMinimalEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/minimal", () => {
            throw new InvalidOperationException("minimal");
        });
    }
}