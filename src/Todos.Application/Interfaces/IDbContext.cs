using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Todos.Domain.Common;

namespace Todos.Application.Interfaces
{
    public interface IDbContext<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> ExecuteReaderQuery(string cmd, CommandType commandType, params SqlParameter[] parameters);
        Task ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] parameters);
        TEntity UpsertTimeStamps(TEntity entity, bool creating = false);
    }
}