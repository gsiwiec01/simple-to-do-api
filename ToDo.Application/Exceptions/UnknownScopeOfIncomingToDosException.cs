namespace ToDoApp.Application.Exceptions;

public class UnknownScopeOfIncomingToDosException : AppException
{
    public UnknownScopeOfIncomingToDosException(string scope) : base($"Scope {scope} is unknown for incoming todos")
    {
    }
}