using Todos.Domain.Entities;

namespace Todos.Application.Interfaces
{
    public interface ITodoItemRepository : IAsyncCrudRepository<TodoItem>
    {
    }
}