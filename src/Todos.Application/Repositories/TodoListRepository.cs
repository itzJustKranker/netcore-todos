using Todos.Application.Interfaces;
using Todos.Domain.Entities;

namespace Todos.Application.Repositories
{
    public class TodoListRepository : AsyncCrudRepository<TodoList>, ITodoListRepository
    {
        public TodoListRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}