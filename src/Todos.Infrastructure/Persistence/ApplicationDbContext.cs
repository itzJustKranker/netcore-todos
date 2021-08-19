using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todos.Application.Interfaces;
using Todos.Domain.Common;
using Todos.Domain.Entities;

namespace Todos.Infrastructure.Persistence
{
    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            return SaveWithTracking();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await SaveWithTrackingAsync(true, cancellationToken);
        }

        protected virtual int SaveWithTracking()
        {
            AddTimeStamps();
            return base.SaveChanges();
        }

        protected virtual async Task<int> SaveWithTrackingAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken)
        {
            AddTimeStamps();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddTimeStamps()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditedEntity && (e.State == EntityState.Added
                                                          || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((AuditedEntity) entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditedEntity) entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}