namespace ToDoApp.Application.Exceptions;

public class ToDoDoesNotExistException : AppException
{
    public ToDoDoesNotExistException(Guid id) : base($"ToDo with id {id} does not exist")
    {
    }
}