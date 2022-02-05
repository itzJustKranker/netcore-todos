using System.Diagnostics.CodeAnalysis;
using Todos.Domain.Common;

namespace Todos.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class TodoItem : AuditedEntity
    {
        public TodoItem()
        {
        }

        public TodoItem(string title, string description, bool completed, int priority, int listId)
        {
            this.Title = title;
            this.Description = description;
            this.Completed = completed;
            this.Priority = priority;
            this.ListId = listId;
        }
        
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public int Priority { get; private set; }

        public long ListId { get; set; }
        public virtual TodoList List { get; set; }
    }
}