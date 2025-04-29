using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Interfaces.Persistence;
using ToDoApp.Persistence.Data;

namespace ToDoApp.Persistence.Repositories;

internal class ToDoRepository : IToDoRepository
{
    private readonly ToDoContext _context;

    public ToDoRepository(ToDoContext context)
    {
        _context = context;
    }

    public Task<List<Application.Models.ToDo>> GetAllAsync()
    {
        return _context.Todos.ToListAsync();
    }

    public Task<Application.Models.ToDo?> GetByIdAsync(Guid id)
    {
        return _context.Todos.SingleOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<Application.Models.ToDo>> GetIncomingBetweenAsync(DateTime start, DateTime end)
    {
        return _context.Todos
            .Where(x => x.PercentComplete < 100)
            .Where(x => x.Expiry >= start.ToUniversalTime() && x.Expiry <= end.ToUniversalTime())
            .ToListAsync();
    }

    public async Task AddAsync(Application.Models.ToDo todo)
    {
        await _context.Todos.AddAsync(todo);
    }

    public void Update(Application.Models.ToDo todo)
    {
        _context.Todos.Update(todo);
    }

    public void Delete(Application.Models.ToDo todo)
    {
        _context.Todos.Remove(todo);
    }
}