using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;

namespace Todos.Domain.Common
{
    [ExcludeFromCodeCoverage]
    public class BaseEntity : IHasKey
    {
        [Key] 
        [BsonId, BsonElement("id")]
        public virtual Guid Id { get; set; } = Guid.NewGuid();
    }

    [ExcludeFromCodeCoverage]
    public abstract class BaseEntity<T> : BaseEntity, IEntity<T> where T : IEntity<T>
    {
    }

    [ExcludeFromCodeCoverage]
    public class AuditedEntity : BaseEntity, IHasTimestamps
    {
        [BsonElement("created_at"), BsonIgnoreIfNull]
        public DateTime CreatedAt { get; set; }
        
        [BsonElement("updated_at"), BsonIgnoreIfNull]
        public DateTime UpdatedAt { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public abstract class AuditedEntity<T> : BaseEntity<T>, IHasTimestamps where T : IEntity<T>
    {
        [BsonElement("created_at"), BsonIgnoreIfNull]
        public DateTime CreatedAt { get; set; }
        
        [BsonElement("updated_at"), BsonIgnoreIfNull]
        public DateTime UpdatedAt { get; set; }
    }
}