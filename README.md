# Global exception handling with ASP.NET Core

### Old way (pre .NET 8)
__Steps__:
1. Define a middleware class (in-place, a duck-typed middleware class, or a strongly-typed IMiddleware implementation).
2. In the body of InvokeAsync method, cover the call to "next(context)" with a try-catch block.
3. In the catch block, setup logic to handle the exception and probably, return a response back to the client.
4. If implementing IMiddleware, register the class with the DI container. 
   For other implementations of middleware, this can be skipped.
5. Configure the middleware pipeline by setting up the exception handling middleware as the first one.
   app.UseMiddleware<T>()


### New way (.NET 8 onwards)
1. Implement IExceptionhandler interface
2. Setup logic in the TryHandleAsync() method to handle exception and probably, return a response back to the client.
   Return true from the method after handling exception to complete middleware processing.
3. Register the global exception handler with DI container: builder.Services.AddExceptionhandler<T>()
4. Register the problem details service with DI container to let it act as a fallback to in cases where exception is not handled at all: builder.Services.AddProblemDetails<T>()
4. Configure the middleware pipeline by setting up the exception handling middleware: app.UseExceptionHandler();
5. This works for both - controllers and minimal APIs.

#### Chaining the global exception handlers (new way)
1. Register the IExceptionHandler interface implementations with the DI container.
   The order of the registeration is the order of execution of the handlers.
2. To pass control to next global exception handler in line, return false from the TryHandleAsync() method.
3. The middleware pipeline configuration remains untouched.