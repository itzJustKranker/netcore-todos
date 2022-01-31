using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todos.Application.Interfaces;
using Todos.Domain.Common;
using Todos.Infrastructure.Persistence;

namespace Todos.Infrastructure.Repositories
{
    public abstract class AsyncCrudRepository<TEntity> : IAsyncCrudRepository<TEntity> where TEntity : BaseEntity
    {
        protected IDbContext<TEntity> Context;
        protected string TableName;

        protected AsyncCrudRepository(IDbContext<TEntity> context)
        {
            Context = context;
            TableName = $"{typeof(TEntity).Name}s";
        }
        
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var cmd = $"SELECT * FROM {TableName}";
            return Context.ExecuteReaderQuery(cmd);
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