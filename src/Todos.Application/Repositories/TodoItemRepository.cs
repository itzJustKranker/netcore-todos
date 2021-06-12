using Todos.Application.Interfaces;
using Todos.Domain.Entities;

namespace Todos.Application.Repositories
{
    public class TodoItemRepository : AsyncCrudRepository<TodoItem>
    {
        public TodoItemRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}