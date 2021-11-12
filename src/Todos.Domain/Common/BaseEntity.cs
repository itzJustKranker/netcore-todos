using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Todos.Domain.Common
{
    [ExcludeFromCodeCoverage]
    public class BaseEntity : IHasKey
    {
        [Key]
        public virtual long Id { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public abstract class BaseEntity<T> : BaseEntity, IEntity<T> where T : IEntity<T>
    {
    }

    [ExcludeFromCodeCoverage]
    public class AuditedEntity : BaseEntity, IHasTimestamps
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public abstract class AuditedEntity<T> : BaseEntity<T>, IHasTimestamps where T : IEntity<T>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}