namespace ToDoApp.Application.Exceptions;

public class ToDoPercentCompleteMustBeBetweenException : AppException
{
    public ToDoPercentCompleteMustBeBetweenException() : base("Percent complete must be between 0 and 100.")
    {
    }
}