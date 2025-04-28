using ToDoApp.Application.Commands;
using ToDoApp.Application.Enums;
using ToDoApp.Application.Models;

namespace ToDoApp.Application.Services.Interfaces;

public interface IToDoService
{
    Task<List<ToDo>> GetAllToDoAsync();
    Task<ToDo?> GetToDoByIdAsync(Guid id);
    Task<List<ToDo>> GetIncomingToDosAsync(IncomingScope scope);
    Task CreateToDoAsync(CreateToDoCommand command);
    Task UpdateToDoAsync(UpdateToDoCommand command);
    Task SetPercentCompleteAsync(SetPercentCompleteCommand command);
}