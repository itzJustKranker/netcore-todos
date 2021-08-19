using Todos.Domain.Common;

namespace Todos.Domain.Entities
{
    public class TodoItem : AuditedEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public int Priority { get; set; }

        public long ListId { get; set; }
        public virtual TodoList List { get; set; }
    }
}