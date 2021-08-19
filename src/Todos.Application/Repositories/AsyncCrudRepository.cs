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
        protected readonly DbSet<TEntity> DbSet;

        protected AsyncCrudRepository(IApplicationDbContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }
        
        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await DbSet.AsNoTracking()
                .ToListAsync();
        }

        public async Task<TEntity> GetAsync(long id)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var created = await DbSet.AddAsync(entity);
            await _context.SaveChangesAsync(default);
            return created?.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var updated = DbSet.Update(entity);
            await _context.SaveChangesAsync(default);
            return updated?.Entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await _context.SaveChangesAsync(default);
        }
    }
}