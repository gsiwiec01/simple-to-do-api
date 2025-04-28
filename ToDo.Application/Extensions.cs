using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Application.Services;
using ToDoApp.Application.Services.Interfaces;

namespace ToDoApp.Application;

public static class Extensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IToDoService, ToDoService>();
        
        return services;
    }
}