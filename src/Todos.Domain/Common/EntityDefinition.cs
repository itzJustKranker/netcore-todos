namespace Todos.Domain.Common;

using System;

public interface IHasKey<TKey>
{
    TKey Id { get; }
}

public interface IHasKey : IHasKey<Guid>
{
}

public interface IHasTimestamps
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}