using Scalar.AspNetCore;
using ToDoApp.API.Configuration.Exceptions;
using ToDoApp.API.Endpoints;
using ToDoApp.Application;
using ToDoApp.Persistence;
using ToDoApp.Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDatabase(builder.Configuration, "ToDoDatabase");
builder.Services.AddServices();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.WithTitle("Simple ToDo Api");
    });
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseDatabase();

app.MapToDoEndpoints();

app.Run();
