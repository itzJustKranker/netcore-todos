using System.Collections.Generic;
using Todos.Domain.Common;

namespace Todos.Infrastructure.Persistence
{
    public interface IDbContext<out TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> ExecuteReaderQuery(string cmd);
    }
}