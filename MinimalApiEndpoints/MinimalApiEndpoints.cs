namespace AspNetCoreWebApiErrorHandlingMiddleware.MinimalApiEndpoints;

public static class MinimalApiEndpoints
{
    public static void MapMinimalEndpoints(this IEndpointRouteBuilder routes)
    {
        var minimalApiRoutesGroup = routes.MapGroup("/minimal");

        minimalApiRoutesGroup.MapGet("", () =>
        {
            throw new NotImplementedException();
        });

        minimalApiRoutesGroup.MapGet("invalid", () =>
        {
            throw new InvalidOperationException();
        });
    }
}