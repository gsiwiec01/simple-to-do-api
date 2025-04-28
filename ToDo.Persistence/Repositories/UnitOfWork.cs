using ToDoApp.Application.Interfaces.Persistence;
using ToDoApp.Persistence.Data;

namespace ToDoApp.Persistence.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly ToDoContext _context;

    public UnitOfWork(ToDoContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}