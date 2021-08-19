using System;
using System.ComponentModel.DataAnnotations;

namespace Todos.Domain.Common
{
    public class BaseEntity : IHasKey
    {
        [Key]
        public virtual long Id { get; set; }
    }

    public abstract class BaseEntity<T> : BaseEntity, IEntity<T> where T : IEntity<T>
    {
    }

    public class AuditedEntity : BaseEntity, IHasTimestamps
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public abstract class AuditedEntity<T> : BaseEntity<T>, IHasTimestamps where T : IEntity<T>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}