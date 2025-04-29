using ToDoApp.Application.Commands;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Enums;
using ToDoApp.Application.Exceptions;
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

    public async Task<List<ToDoDto>> GetAllAsync()
    {
        var todos = await _toDoRepository.GetAllAsync();
        return todos.Select(x =>
            new ToDoDto(x.Id, x.Title, x.Description, x.Expiry, x.PercentComplete)
        ).ToList();
    }

    public async Task<ToDoDto?> GetByIdAsync(Guid id)
    {
        var todo = await _toDoRepository.GetByIdAsync(id);
        if (todo is null)
            throw new ToDoDoesNotExistException(id);

        var dto = new ToDoDto(todo.Id, todo.Title, todo.Description, todo.Expiry, todo.PercentComplete);
        return dto;
    }

    public async Task<List<ToDoDto>> GetIncomingAsync(IncomingScope scope)
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
                var daysUntilEndOfWeek = DayOfWeek.Saturday - today.DayOfWeek;
                
                start = today;
                end = today.AddDays(daysUntilEndOfWeek + 1).AddTicks(-1);
                
                break;
            }

            default:
            {
                throw new UnknownScopeOfIncomingToDosException(scope.ToString());
            }
        }
        
        var todos = await _toDoRepository.GetIncomingBetweenAsync(start, end);
        return todos.Select(x =>
            new ToDoDto(x.Id, x.Title, x.Description, x.Expiry, x.PercentComplete)
        ).ToList();
    }

    public async Task CreateAsync(CreateToDoCommand command)
    {
        var todo = ToDo.Create(command.Id, command.Title, command.Description, command.Expiry);
        
        await _toDoRepository.AddAsync(todo);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateToDoCommand command)
    {
        var todo = await _toDoRepository.GetByIdAsync(command.Id);
        if (todo is null)
            throw new ToDoDoesNotExistException(command.Id);
        
        todo.Update(command.Title, command.Description, command.Expiry, command.PercentComplete);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SetPercentCompleteAsync(SetPercentCompleteCommand command)
    {
        var todo = await _toDoRepository.GetByIdAsync(command.Id);
        if (todo is null)
            throw new ToDoDoesNotExistException(command.Id);
        
        todo.SetPercentComplete(command.PercentComplete);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task MarkAsDoneAsync(Guid id)
    {
        var todo = await _toDoRepository.GetByIdAsync(id);
        if (todo is null)
            throw new ToDoDoesNotExistException(id);
        
        todo.MarkAsDone();
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var todo = await _toDoRepository.GetByIdAsync(id);
        if (todo is null)
            throw new ToDoDoesNotExistException(id);
        
        _toDoRepository.Delete(todo);
        await _unitOfWork.SaveChangesAsync();
    }
}