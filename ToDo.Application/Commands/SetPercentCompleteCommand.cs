namespace ToDoApp.Application.Commands;

public record SetPercentCompleteCommand(Guid Id, int PercentComplete);