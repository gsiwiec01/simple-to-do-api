namespace ToDoApp.Application.Exceptions;

public class ToDoExpiryDateCannotBeInThePastException : AppException
{
    public ToDoExpiryDateCannotBeInThePastException() : base("Expiry date cannot be in the past.")
    {
    }
}