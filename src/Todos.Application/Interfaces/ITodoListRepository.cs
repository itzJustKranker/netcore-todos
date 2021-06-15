using Todos.Domain.Entities;

namespace Todos.Application.Interfaces
{
    public interface ITodoListRepository : IAsyncCrudRepository<TodoList>
    {
    }
}