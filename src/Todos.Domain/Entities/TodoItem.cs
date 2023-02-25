namespace Todos.Domain.Entities;

using System;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;
using Todos.Domain.Common;

[ExcludeFromCodeCoverage]
public class TodoItem : AuditedEntity
{
    [BsonElement("title")] [BsonRequired] public string Title { get; set; }

    [BsonElement("description")]
    [BsonIgnoreIfNull]
    public string Description { get; set; }

    [BsonElement("completed")] public bool Completed { get; set; } = false;

    [BsonElement("priority")]
    [BsonIgnoreIfNull]
    public int Priority { get; set; }

    [BsonElement("list_id")]
    [BsonRequired]
    public Guid ListId { get; set; }
}