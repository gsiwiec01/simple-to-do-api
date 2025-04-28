namespace ToDoApp.Application.Commands;

public record CreateToDoCommand(Guid Id, string Title, string Description, DateTime Expiry);