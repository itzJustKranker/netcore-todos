using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todos.Application.Interfaces;
using Todos.Domain.Common;

namespace Todos.Application.Repositories
{
    public class AsyncCrudRepository<TEntity> : IAsyncCrudRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        protected AsyncCrudRepository(IApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        
        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .ToListAsync();
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var created = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync(default);
            return created?.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var updated = _dbSet.Update(entity);
            await _context.SaveChangesAsync(default);
            return updated?.Entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(default);
        }
    }
}