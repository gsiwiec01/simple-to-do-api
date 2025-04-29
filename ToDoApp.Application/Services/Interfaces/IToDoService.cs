using ToDoApp.Application.Commands;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Enums;
using ToDoApp.Application.Models;

namespace ToDoApp.Application.Services.Interfaces;

public interface IToDoService
{
    Task<List<ToDoDto>> GetAllAsync();
    Task<ToDoDto?> GetByIdAsync(Guid id);
    Task<List<ToDoDto>> GetIncomingAsync(IncomingScope scope);
    Task CreateAsync(CreateToDoCommand command);
    Task UpdateAsync(UpdateToDoCommand command);
    Task SetPercentCompleteAsync(SetPercentCompleteCommand command);
    Task MarkAsDoneAsync(Guid id);
    Task DeleteAsync(Guid id);
}