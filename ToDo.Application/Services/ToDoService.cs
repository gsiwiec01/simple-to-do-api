using ToDoApp.Application.Interfaces.Persistence;
using ToDoApp.Application.Models;
using ToDoApp.Application.Services.Interfaces;

namespace ToDoApp.Application.Services;

public class ToDoService : IToDoService
{
    private readonly IToDoRepository _toDoRepository;

    public ToDoService(IToDoRepository toDoRepository)
    {
        _toDoRepository = toDoRepository;
    }

    public Task<List<ToDo>> GetAllToDo()
    {
        return _toDoRepository.GetAllAsync();
    }
}