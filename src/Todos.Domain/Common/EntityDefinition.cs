using System;

namespace Todos.Domain.Common
{
    public interface IHasKey<TKey>
    {
        TKey Id { get; }
    }
        
    public interface IHasKey : IHasKey<long>
    {
    }
            
    public interface IHasTimestamps
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}