using FluentValidation;
using Scalar.AspNetCore;
using ToDoApp.API.Configuration.Exceptions;
using ToDoApp.API.Endpoints;
using ToDoApp.API.Requests.Validators;
using ToDoApp.Application;
using ToDoApp.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDatabase(builder.Configuration, "ToDoDatabase"); // Adds the database context and connection.
builder.Services.AddServices(); // Registers application and infrastructure services.
builder.Services.AddProblemDetails(); // Adds RFC 7807 standardized error responses.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // Registers the global exception handler.
builder.Services.AddValidatorsFromAssemblyContaining<CreateToDoRequestValidator>(); // Adds FluentValidation validators.

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(opt => { opt.WithTitle("Simple ToDo Api"); });

app.UseExceptionHandler(); // Enables centralized exception handling.
app.UseHttpsRedirection();
app.UseDatabase(); // Applies any pending database migrations.

app.MapToDoEndpoints(); // Maps the ToDo endpoints.

app.Run();

/// <summary>
/// Dummy Program class required for integration testing.
/// </summary>
public partial class Program
{
}