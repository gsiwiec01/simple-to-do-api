using FluentValidation;
using ToDoApp.API.Requests;
using ToDoApp.API.Requests.Validators;
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
            var todos = await service.GetAllAsync();
            return Results.Ok(todos);
        });

        group.MapGet("/{id:guid}", async (Guid id, IToDoService service) =>
        {
            var todo = await service.GetByIdAsync(id);
            return todo is not null ? Results.Ok(todo) : Results.NotFound();
        });

        group.MapGet("/incoming", async (IncomingScope scope, IToDoService service) =>
        {
            var todos = await service.GetIncomingAsync(scope);
            return Results.Ok(todos);
        });

        group.MapPost("/", async (CreateToDoRequest request, IValidator<CreateToDoRequest> validator, IToDoService service) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors);
            
            var id = Guid.NewGuid();
            var command = new CreateToDoCommand(id, request.Title, request.Description, request.Expiry);

            await service.CreateAsync(command);

            return Results.Created($"/todos/{id}", id);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateToDoRequest request, IValidator<UpdateToDoRequest> validator, IToDoService service) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors);
            
            var command = new UpdateToDoCommand(
                id,
                request.Title,
                request.Description,
                request.Expiry,
                request.PercentComplete
            );

            await service.UpdateAsync(command);
            return Results.NoContent();
        });

        group.MapPatch("/{id:guid}/percent", async (Guid id, SetPercentCompleteRequest request, IValidator<SetPercentCompleteRequest> validator, IToDoService service) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors);
            
            var command = new SetPercentCompleteCommand(id, request.PercentComplete);

            await service.SetPercentCompleteAsync(command);
            return Results.NoContent();
        });
        
        group.MapPatch("/{id:guid}/done", async (Guid id, IToDoService service) =>
        {
            await service.MarkAsDoneAsync(id);
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IToDoService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}