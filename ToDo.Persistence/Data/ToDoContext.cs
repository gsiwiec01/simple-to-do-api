using Microsoft.EntityFrameworkCore;

namespace ToDo.Persistence.Data;

internal class ToDoContext : DbContext
{
    public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
    {
    }
}