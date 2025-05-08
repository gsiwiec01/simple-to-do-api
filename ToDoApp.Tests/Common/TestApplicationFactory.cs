using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using ToDoApp.Persistence.Data;

namespace ToDoApp.Tests.Integration;

public class TestApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithUsername("root")
        .WithPassword("root")
        .WithCleanUp(true)
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(x =>
        {
            var serviceDescriptor = typeof(ToDoContext);
            if (x.FirstOrDefault(t => t.ServiceType == serviceDescriptor) is {} descriptor)
            {
                x.Remove(descriptor);
            }

            x.AddDbContext<ToDoContext>(t =>
            {
                t.UseNpgsql(_dbContainer.GetConnectionString());
            });
        });
    }
    
    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}