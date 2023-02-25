namespace Todos.Application.Interfaces;

using Todos.Domain.Entities;

public interface ITodoListRepository : IAsyncCrudRepository<TodoList>
{
}