namespace ToDoApp.Application.Exceptions;

/// <summary>
/// Represents the base exception type for application-specific errors.
/// </summary>
public class AppException : Exception
{
    public AppException(string message) : base(message)
    {
    }
}