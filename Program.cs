using AspNetCoreWebApiErrorHandlingMiddleware.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// For the old way of using global exception handler, 
// register the interface implementation of IMiddleware with DI container.
// builder.Services.AddTransient<OldGlobalExceptionHandler>();

// For the new way of using global exception handler, 
// register the interface implementation of IExceptionHandler with the DI container
builder.Services.AddExceptionHandler<NewGlobalExceptionHandler>();
// Exception handlers can be chained too, to allow different logic in exception handlers.
// To pass control to the next exception handler, simply return false before generating response 
// so that the next exception handler can take over
// builder.Services.AddExceptionHandler<NewGlobalExceptionHandler2>();
// Register problem details with the DI container as this is a dependency for the new global exception handler
builder.Services.AddProblemDetails();

var app = builder.Build();

//app.MinimalApiEndpoints();

// For the old way of using global exception handler, 
// configure the middleware pipeline to use the interface implementation of IMiddleware.
// It will be resolved from the DI container.
// app.UseMiddleware<OldGlobalExceptionHandler>();

// For the new way of using global exception handler,
// configure the middleware pipeline by adding UseExceptionhandler().
// The concrete implementation will be resolved from the DI container.
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
