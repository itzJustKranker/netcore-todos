using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using Todos.Infrastructure.Settings;

namespace Todos.Infrastructure.Persistence
{
    public class DbContext : IDbContext
    {
        private readonly DatabaseSettings _settings;

        public DbContext(IOptions<DatabaseSettings> options)
        {
            _settings = options.Value;
        }
        
        public void ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            using var conn = CreateSqlConnection();
            using var cmd = new SqlCommand(commandText, conn);
            cmd.CommandType = commandType;
            cmd.Parameters.AddRange(parameters);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        
        public void ExecuteScalar(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            using var conn = CreateSqlConnection();
            using var cmd = new SqlCommand(commandText, conn);
            cmd.CommandType = commandType;
            cmd.Parameters.AddRange(parameters);

            conn.Open();
            cmd.ExecuteScalar();
        }
        
        public SqlDataReader ExecuteReader(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var conn = CreateSqlConnection();
            using var cmd = new SqlCommand(commandText, conn);
            cmd.CommandType = commandType;
            cmd.Parameters.AddRange(parameters);

            conn.Open();
            var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        private SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(_settings.ConnectionString);
        }
    }
}