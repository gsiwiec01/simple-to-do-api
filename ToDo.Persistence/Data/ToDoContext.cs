using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Persistence.Data;

internal class ToDoContext : DbContext
{
    public DbSet<Application.Models.ToDo> Todos { get; set; }
    
    public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
    {
    }
}