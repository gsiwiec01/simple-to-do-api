namespace ToDoApp.Application.Commands;

public record UpdateToDoCommand(
    Guid Id,
    string Title,
    string Description,
    DateTime Expiry,
    int PercentComplete
);