using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Todos.Application.Extensions;
using Todos.Application.Interfaces;
using Todos.Domain.Common;
using Todos.Infrastructure.Settings;

namespace Todos.Infrastructure.Persistence
{
    public class DbContext<TEntity> : IDbContext<TEntity> where TEntity : BaseEntity
    {
        private readonly DatabaseSettings _settings;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DbContext(IOptions<DatabaseSettings> options, IDateTimeProvider dateTimeProvider)
        {
            _settings = options.Value;
            _dateTimeProvider = dateTimeProvider;
        }
        
        public async Task<IEnumerable<TEntity>> ExecuteReaderQuery(string cmd, CommandType commandType, params SqlParameter[] parameters)
        {
            using var reader = await ExecuteReader(cmd, commandType, parameters);
            var result = new List<TEntity>();
            
            while (reader.Read()) {
                var obj = Activator.CreateInstance<TEntity>();
                foreach (var prop in obj.GetType().GetProperties())
                {
                    if (!reader.HasColumn(prop.Name)) continue;
                    if (!Equals(reader.GetValue(prop.Name), DBNull.Value)) {
                        prop.SetValue(obj, reader.GetValue(prop.Name), null);
                    }
                }
                result.Add(obj);
            }

            return result;
        }

        public async Task ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            await using var conn = CreateSqlConnection();
            await using var cmd = new SqlCommand(commandText, conn);
            cmd.CommandType = commandType;
            cmd.Parameters.AddRange(parameters);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        
        public TEntity UpsertTimeStamps(TEntity entity, bool creating = false)
        {
            var auditedEntity = entity as AuditedEntity ?? new AuditedEntity();
            var dateTimeUtcNow = _dateTimeProvider.UtcNow();
            if (creating)
                auditedEntity.CreatedAt = dateTimeUtcNow;
            auditedEntity.UpdatedAt = dateTimeUtcNow;
            return auditedEntity as TEntity;
        }
        
        private async Task<IDataReaderWrapper> ExecuteReader(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var conn = CreateSqlConnection();
            await using var cmd = new SqlCommand(commandText, conn);
            cmd.CommandType = commandType;
            cmd.Parameters.AddRange(parameters);

            conn.Open();
            var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            return new DataReaderWrapper(reader);
        }
        
        private SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(_settings.ConnectionString);
        }
    }
}