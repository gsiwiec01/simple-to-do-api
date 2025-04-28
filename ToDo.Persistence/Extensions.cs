using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ToDoApp.Application.Interfaces.Persistence;
using ToDoApp.Persistence.Data;
using ToDoApp.Persistence.Repositories;

namespace ToDoApp.Persistence;

public static class Extensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration,
        string connectionStringName)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentException("Connection string is missing");

        return services.AddDatabase(connectionString);
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ToDoContext>(ctxOptions =>
        {
            ctxOptions.UseNpgsql(connectionString,
                opt => { opt.MigrationsAssembly(typeof(Extensions).Assembly.FullName); });
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IToDoRepository, ToDoRepository>();

        return services;
    }


    public static IApplicationBuilder UseDatabase(this IApplicationBuilder app)
    {
        var services = app.ApplicationServices;
        using var scope = services.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<ToDoContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ToDoContext>>();

        var pendingMigrations = dbContext.Database.GetPendingMigrations().ToList();
        if (pendingMigrations.Count != 0)
        {
            logger.LogInformation($"Database is not up to date. Missing migrations: {pendingMigrations.Count}");

            dbContext.Database.Migrate();

            foreach (var migration in pendingMigrations)
            {
                logger.LogInformation($"Migration executed: {migration}");
            }

            logger.LogInformation("Database is up to date");
        }
        else
        {
            logger.LogInformation("Database already up to date.");
        }

        return app;
    }
}