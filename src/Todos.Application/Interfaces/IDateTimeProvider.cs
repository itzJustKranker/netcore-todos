namespace Todos.Application.Interfaces;

using System;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}