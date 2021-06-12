using System.Collections.Generic;
using Todos.Domain.Common;

namespace Todos.Domain.Entities
{
    public class TodoList : BaseEntity
    {
        public string Title { get; set; }
        public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
    }
}