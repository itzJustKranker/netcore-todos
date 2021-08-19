using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todos.Application.Interfaces
{
    public interface IAsyncCrudRepository<TEntity>
    {
        Task<IList<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(long id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}