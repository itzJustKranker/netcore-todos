using System.Data;
using System.Data.SqlClient;

namespace Todos.Infrastructure.Persistence
{
    public interface IDbContext
    {
        void ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] parameters);
        void ExecuteScalar(string commandText, CommandType commandType, params SqlParameter[] parameters);
        SqlDataReader ExecuteReader(string commandText, CommandType commandType, params SqlParameter[] parameters);
    }
}