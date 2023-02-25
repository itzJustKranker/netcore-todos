namespace Todos.Application.Interfaces;

using Todos.Domain.Entities;

public interface ITodoItemRepository : IAsyncCrudRepository<TodoItem>
{
}