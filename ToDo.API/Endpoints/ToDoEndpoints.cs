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
    }
}