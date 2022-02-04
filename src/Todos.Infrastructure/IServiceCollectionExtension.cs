using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Todos.Application.Interfaces;
using Todos.Application.Providers;
using Todos.Domain.Common;
using Todos.Domain.Entities;
using Todos.Infrastructure.Persistence;
using Todos.Infrastructure.Repositories;

namespace Todos.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            // Register Contexts
            services.RegisterDbContext<TodoItem>();
            services.RegisterDbContext<TodoList>();
            
            // Register Repositories
            services.AddScoped<ITodoItemRepository, TodoItemRepository>();
            services.AddScoped<ITodoListRepository, TodoListRepository>();
            
            // Register Services
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            return services;
        }

        private static IServiceCollection RegisterDbContext<TEntity>(this IServiceCollection services) where TEntity : BaseEntity
        {
            services.AddSingleton<IDbContext<TEntity>, DbContext<TEntity>>();
            return services;
        }
    }
}