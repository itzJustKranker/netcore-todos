using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todos.Application.Interfaces;
using Todos.Domain.Common;

namespace Todos.Infrastructure.Repositories
{
    public abstract class AsyncCrudRepository<TEntity> : IAsyncCrudRepository<TEntity> where TEntity : BaseEntity
    {
        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
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