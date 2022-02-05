using System.Collections.Generic;
using Todos.Domain.Entities;

namespace Todos.Application.Interfaces
{
    public interface ITodoItemRepository : IAsyncCrudRepository<TodoItem>
    {
        ICollection<TodoItem> GetItemsByList(long listId);
    }
}