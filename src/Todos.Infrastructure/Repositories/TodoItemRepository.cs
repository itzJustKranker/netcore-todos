using Todos.Application.Interfaces;
using Todos.Domain.Entities;
using Todos.Infrastructure.Persistence;

namespace Todos.Infrastructure.Repositories
{
    public class TodoItemRepository : AsyncCrudRepository<TodoItem>, ITodoItemRepository
    {
        public TodoItemRepository(IDbContext context) : base(context)
        {
        }
    }
}