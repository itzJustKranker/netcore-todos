using System.Diagnostics.CodeAnalysis;
using Todos.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace Todos.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class TodoList : AuditedEntity
    {
        [BsonElement("title"), BsonIgnoreIfNull]
        public string Title { get; set; }
    }
}