namespace ToDoApp.API.Requests;

public record CreateToDoRequest(string Title, string Description, DateTime Expiry);