using ToDoApp.API.Requests;
using ToDoApp.Application.Commands;
using ToDoApp.Application.Enums;
using ToDoApp.Application.Services.Interfaces;

namespace ToDoApp.API.Endpoints;

public static class ToDoEndpoints
{
    public static void MapToDoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/todos");

        group.MapGet("/", async (IToDoService service) =>
        {
            var todos = await service.GetAllToDoAsync();
            return Results.Ok(todos);
        });

        group.MapGet("/{id:guid}", async (Guid id, IToDoService service) =>
        {
            var todo = await service.GetToDoByIdAsync(id);
            return todo is not null ? Results.Ok(todo) : Results.NotFound();
        });

        group.MapGet("/incoming", async (IncomingScope scope, IToDoService service) =>
        {
            var todos = await service.GetIncomingToDosAsync(scope);
            return Results.Ok(todos);
        });

        group.MapPost("/", async (CreateToDoRequest request, IToDoService service) =>
        {
            var id = Guid.NewGuid();
            var command = new CreateToDoCommand(id, request.Title, request.Description, request.Expiry);

            await service.CreateToDoAsync(command);

            return Results.Created($"/todos/{id}", id);
        });
    }
}