using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Todos.Domain.Common;

namespace Todos.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class TodoList : AuditedEntity
    {
        public string Title { get; set; }
        public IList<TodoItem> Items { get; set; } = new List<TodoItem>();
    }
}