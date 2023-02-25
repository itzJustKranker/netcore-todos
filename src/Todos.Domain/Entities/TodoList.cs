namespace Todos.Domain.Entities;

using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;
using Todos.Domain.Common;

[ExcludeFromCodeCoverage]
public class TodoList : AuditedEntity
{
    [BsonElement("title")]
    [BsonIgnoreIfNull]
    public string Title { get; set; }
}