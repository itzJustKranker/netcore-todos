using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using Todos.Application.Extensions;
using Todos.Domain.Common;
using Todos.Infrastructure.Settings;

namespace Todos.Infrastructure.Persistence
{
    public class DbContext<TEntity> : IDbContext<TEntity> where TEntity : BaseEntity
    {
        private readonly DatabaseSettings _settings;

        public DbContext(IOptions<DatabaseSettings> options)
        {
            _settings = options.Value;
        }
        
        public IEnumerable<TEntity> ExecuteReaderQuery(string cmd, params SqlParameter[] parameters)
        {
            using var reader = ExecuteReader(cmd, CommandType.Text, parameters);
            var result = new List<TEntity>();
            
            while (reader.Read()) {
                var obj = Activator.CreateInstance<TEntity>();
                foreach (var prop in obj.GetType().GetProperties())
                {
                    if (!reader.HasColumn(prop.Name)) continue;
                    if (!Equals(reader[prop.Name], DBNull.Value)) {
                        prop.SetValue(obj, reader[prop.Name], null);
                    }
                }
                result.Add(obj);
            }

            return result;
        }
        
        private void ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            using var conn = CreateSqlConnection();
            using var cmd = new SqlCommand(commandText, conn);
            cmd.CommandType = commandType;
            cmd.Parameters.AddRange(parameters);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        
        private void ExecuteScalar(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            using var conn = CreateSqlConnection();
            using var cmd = new SqlCommand(commandText, conn);
            cmd.CommandType = commandType;
            cmd.Parameters.AddRange(parameters);

            conn.Open();
            cmd.ExecuteScalar();
        }
        
        private IDataReader ExecuteReader(string commandText, CommandType commandType, params SqlParameter[] parameters)
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