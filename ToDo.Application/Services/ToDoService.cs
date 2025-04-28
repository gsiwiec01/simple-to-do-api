using ToDoApp.Application.Commands;
using ToDoApp.Application.Enums;
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

    public Task<List<ToDo>> GetIncomingToDosAsync(IncomingScope scope)
    {
        var today = DateTime.Today;
        
        DateTime start;
        DateTime end;

        switch (scope)
        {
            case IncomingScope.Today:
            {
                start = today;
                end = today.AddDays(1).AddTicks(-1);
                
                break;
            }

            case IncomingScope.Tomorrow:
            {
                start = today.AddDays(1);
                end = today.AddDays(2).AddTicks(-1);
                
                break;
            }

            case IncomingScope.Week:
            {
                var daysUntilEndOfWeek = DayOfWeek.Sunday - today.DayOfWeek;
                
                start = today;
                end = today.AddDays(daysUntilEndOfWeek + 1).AddTicks(-1);
                
                break;
            }

            default:
            {
                throw new ArgumentOutOfRangeException(nameof(scope), scope, "Invalid incoming scope value");
            }
        }

        return _toDoRepository.GetIncomingBetweenAsync(start, end);
    }

    public async Task CreateToDoAsync(CreateToDoCommand command)
    {
        var todo = ToDo.Create(command.Id, command.Title, command.Description, command.Expiry);
        
        await _toDoRepository.AddAsync(todo);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateToDoAsync(UpdateToDoCommand command)
    {
        var todo = await _toDoRepository.GetByIdAsync(command.Id);
        if (todo is null)
            throw new KeyNotFoundException($"ToDo with id {command.Id} not found");
        
        todo.Update(command.Title, command.Description, command.Expiry, command.PercentComplete);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SetPercentCompleteAsync(SetPercentCompleteCommand command)
    {
        var todo = await _toDoRepository.GetByIdAsync(command.Id);
        if (todo is null)
            throw new KeyNotFoundException($"ToDo with id {command.Id} not found");
        
        todo.SetPercentComplete(command.PercentComplete);
        await _unitOfWork.SaveChangesAsync();
    }
}