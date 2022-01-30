using Todos.Application.Interfaces;
using Todos.Domain.Entities;

namespace Todos.Infrastructure.Repositories
{
    public class TodoListRepository : AsyncCrudRepository<TodoList>, ITodoListRepository
    {
    }
}