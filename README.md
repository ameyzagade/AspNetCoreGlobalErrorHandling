# Global exception handling with ASP.NET Core

### Old way (pre .NET 8)
__Steps__:
1. Define a middleware class (in-place, duck-typed middleware class, or strongly-typed IMiddleware implementation class).
2. In the body of InvokeAsync method, cover the call to next(context) with a try-catch block.
3. In the catch block, setup logic to handle the exception and probably, return a response back to the client.
4. If implementing IMiddleware, register the class with the DI container. For other implementations of middleware, this can be skipped.
5. Configure the middleware pipeline by setting up the exception handling middleware as the first one.
app.UseMiddleware<T>()


### New way (.NET 8 onwards)
1. Implement IExceptionhandler interface
2. Setup logic in the TryHandleAsync() method to handle exception and probably, return a response back to the client.
   Return true from the method.
3. Register the global exception handler with DI container: builder.Services.AddExceptionhandler<T>()
4. Configure the middleware pipeline by setting up the exception handling middleware: app.UseExceptionHandler();


#### Chaining the global exception handlers (new way)
1. Register the IExceptionHandler interface implementations with the DI container.
2. The order of the registering is the order of execution of the handlers.
3. To pass control to next handler in line, return false from the TryHandleAsync()
4. The middleware pipeline configuration remains untouched.

__Note__: If there is no exception handler setup next in line, a default problem details response is sent to the client.