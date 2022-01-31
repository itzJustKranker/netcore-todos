using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Todos.Application.Interfaces;
using Todos.Application.Extensions;
using Todos.Domain.Common;
using Todos.Infrastructure.Persistence;

namespace Todos.Infrastructure.Repositories
{
    public abstract class AsyncCrudRepository<TEntity> : IAsyncCrudRepository<TEntity> where TEntity : BaseEntity
    {
        protected IDbContext Context;
        protected string TableName;

        protected AsyncCrudRepository(IDbContext context)
        {
            Context = context;
            TableName = $"{typeof(TEntity).Name}s";
        }
        
        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            var cmd = $"SELECT * FROM {TableName}";
            await using var reader = Context.ExecuteReader(cmd, CommandType.Text);
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

        public virtual async Task<TEntity> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}