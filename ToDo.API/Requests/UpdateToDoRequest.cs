namespace ToDoApp.API.Requests;

public record UpdateToDoRequest(
    string Title,
    string Description,
    DateTime Expiry,
    int PercentComplete
);