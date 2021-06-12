using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todos.Application.Interfaces;
using Todos.Application.Repositories;
using Todos.Domain.Entities;
using Todos.Infrastructure.Persistence;

namespace Todos.Infrastructure
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            // Register DbContext
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            
            // Register Repositories
            services.AddScoped<IAsyncCrudRepository<TodoItem>, TodoItemRepository>();
            services.AddScoped<IAsyncCrudRepository<TodoList>, TodoListRepository>();

            return services;
        }

        public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnectionString");
                options.UseSqlServer(connectionString ?? string.Empty);
            });

            return services;
        }
    }
}