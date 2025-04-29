using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Persistence.Data;

internal class ToDoContext : DbContext
{
    public DbSet<Application.Models.ToDo> Todos { get; set; }

    public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Application.Models.ToDo>()
            .Property(x => x.Expiry)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
    }
}