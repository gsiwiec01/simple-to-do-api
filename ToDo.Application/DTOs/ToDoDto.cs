namespace ToDoApp.Application.DTOs;

public record ToDoDto(
    Guid Id,
    string Title,
    string Description,
    DateTime Expiry,
    int PercentComplete,
    bool IsDone
);