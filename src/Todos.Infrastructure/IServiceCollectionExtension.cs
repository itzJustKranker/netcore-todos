using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Todos.Application.Interfaces;
using Todos.Infrastructure.Repositories;

namespace Todos.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            // Register Repositories
            services.AddScoped<ITodoItemRepository, TodoItemRepository>();
            services.AddScoped<ITodoListRepository, TodoListRepository>();

            return services;
        }
    }
}