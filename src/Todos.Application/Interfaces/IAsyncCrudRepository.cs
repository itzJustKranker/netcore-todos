namespace Todos.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAsyncCrudRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetAsync(Guid id);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}