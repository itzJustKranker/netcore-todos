using System.Collections.Generic;
using Todos.Domain.Common;

namespace Todos.Domain.Entities
{
    public class TodoList : BaseEntity
    {
        public string Title { get; set; }
        public IList<TodoItem> Items { get; set; } = new List<TodoItem>();
    }
}