namespace Todos.Application.Providers;

using System;
using Todos.Application.Interfaces;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}