using Todos.Application.Interfaces;
using Todos.Domain.Entities;
using Todos.Infrastructure.Persistence;

namespace Todos.Infrastructure.Repositories
{
    public class TodoListRepository : AsyncCrudRepository<TodoList>, ITodoListRepository
    {
        public TodoListRepository(IDbContext<TodoList> context) : base(context)
        {
        }
    }
}