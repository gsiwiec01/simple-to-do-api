using ToDoApp.Application.Commands;
using ToDoApp.Application.Models;

namespace ToDoApp.Application.Services.Interfaces;

public interface IToDoService
{
    Task<List<ToDo>> GetAllToDoAsync();
    Task<ToDo?> GetToDoByIdAsync(Guid id);
    Task CreateToDoAsync(CreateToDoCommand command);
}