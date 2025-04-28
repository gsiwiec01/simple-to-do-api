using ToDoApp.Application.Commands;
using ToDoApp.Application.Interfaces.Persistence;
using ToDoApp.Application.Models;
using ToDoApp.Application.Services.Interfaces;

namespace ToDoApp.Application.Services;

public class ToDoService : IToDoService
{
    private readonly IToDoRepository _toDoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ToDoService(IToDoRepository toDoRepository, IUnitOfWork unitOfWork)
    {
        _toDoRepository = toDoRepository;
        _unitOfWork = unitOfWork;
    }

    public Task<List<ToDo>> GetAllToDoAsync()
    {
        return _toDoRepository.GetAllAsync();
    }

    public Task<ToDo?> GetToDoByIdAsync(Guid id)
    {
        return _toDoRepository.GetByIdAsync(id);
    }

    public async Task CreateToDoAsync(CreateToDoCommand command)
    {
        var todo = ToDo.Create(command.Id, command.Title, command.Description, command.Expiry);
        
        await _toDoRepository.AddAsync(todo);
        await _unitOfWork.SaveChangesAsync();
    }
}