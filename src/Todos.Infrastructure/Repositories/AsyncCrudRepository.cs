using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todos.Application.Interfaces;
using Todos.Domain.Common;

namespace Todos.Infrastructure.Repositories
{
    public abstract class AsyncCrudRepository<TEntity> : IAsyncCrudRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IApplicationDbContext _context;
        protected readonly DbSet<TEntity> DbSet;

        protected AsyncCrudRepository(IApplicationDbContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }
        
        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            return await DbSet.AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<TEntity> GetAsync(long id)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            var created = await DbSet.AddAsync(entity);
            await _context.SaveChangesAsync(default);
            return created?.Entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var updated = DbSet.Update(entity);
            await _context.SaveChangesAsync(default);
            return updated?.Entity;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await _context.SaveChangesAsync(default);
        }
    }
}