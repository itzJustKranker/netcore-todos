using System;
using Todos.Application.Interfaces;

namespace Todos.Application.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow() => DateTime.UtcNow;
    }
}