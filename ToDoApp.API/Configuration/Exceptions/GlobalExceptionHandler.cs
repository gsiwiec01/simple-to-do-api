using Microsoft.AspNetCore.Diagnostics;
using ToDoApp.Application.Exceptions;

namespace ToDoApp.API.Configuration.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            ToDoDoesNotExistException => StatusCodes.Status404NotFound,
            UnknownScopeOfIncomingToDosException => StatusCodes.Status400BadRequest,
            ToDoTitleCannotBeEmptyException => StatusCodes.Status400BadRequest,
            ToDoPercentCompleteMustBeBetweenException => StatusCodes.Status400BadRequest,
            ToDoExpiryDateCannotBeInThePastException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var errorResponse = new
        {
            Error = exception.Message,
        };
        
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
        
        return new ValueTask<bool>(true);
    }
}