namespace Todos.Domain.Common;

public interface IEntity<T> : IEntity where T : IEntity<T>
{
}

public interface IEntity : IHasKey
{
}