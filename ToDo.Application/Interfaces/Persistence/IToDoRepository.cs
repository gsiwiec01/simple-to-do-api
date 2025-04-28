using ToDoApp.Application.Models;

namespace ToDoApp.Application.Interfaces.Persistence;

public interface IToDoRepository
{
    Task<List<ToDo>> GetAllAsync();
    Task<ToDo?> GetByIdAsync(Guid id);
    Task<List<ToDo>> GetIncomingBetweenAsync(DateTime start, DateTime end);
    Task AddAsync(ToDo todo);
    void Update(ToDo todo);
    void Delete(ToDo todo);
}