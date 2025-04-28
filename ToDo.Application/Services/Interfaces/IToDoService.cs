using ToDoApp.Application.Commands;
using ToDoApp.Application.Models;

namespace ToDoApp.Application.Services.Interfaces;

public interface IToDoService
{
    Task<List<ToDo>> GetAllToDo();
    Task CreateToDo(CreateToDoCommand command);
}