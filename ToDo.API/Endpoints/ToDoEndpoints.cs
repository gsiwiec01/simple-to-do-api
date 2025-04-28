using ToDoApp.API.Requests;
using ToDoApp.Application.Commands;
using ToDoApp.Application.Services.Interfaces;

namespace ToDoApp.API.Endpoints;

public static class ToDoEndpoints
{
    public static void MapToDoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/todos");

        group.MapGet("/", async (IToDoService service) =>
        {
            var todos = await service.GetAllToDo();
            return Results.Ok(todos);
        });

        group.MapPost("/", (CreateToDoRequest request, IToDoService service) =>
        {
            var id = Guid.NewGuid();
            var command = new CreateToDoCommand(id, request.Title, request.Description, request.Expiry);

            service.CreateToDo(command);

            return Results.Created($"/todos/{id}", id);
        });
    }
}