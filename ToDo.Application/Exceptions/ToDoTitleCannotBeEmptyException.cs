namespace ToDoApp.Application.Exceptions;

public class ToDoTitleCannotBeEmptyException : AppException
{
    public ToDoTitleCannotBeEmptyException() : base("Title cannot be empty.")
    {
    }
}