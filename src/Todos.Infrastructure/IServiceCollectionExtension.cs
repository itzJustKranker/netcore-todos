namespace Todos.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Todos.Application.Interfaces;
using Todos.Application.Providers;
using Todos.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public static class IServiceCollectionExtension
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services)
    {
        // Register Repositories
        services.AddSingleton<ITodoListRepository, TodoListRepository>();
        services.AddSingleton<ITodoItemRepository, TodoItemRepository>();

        // Register Services
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}