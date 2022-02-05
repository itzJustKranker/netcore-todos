using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Todos.Application.Interfaces;
using Todos.Domain.Entities;

namespace Todos.Infrastructure.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly IDbContext<TodoItem> _context;
        private const string TableName = "TodoItems";

        public TodoItemRepository(IDbContext<TodoItem> context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            var cmd = $"SELECT * FROM {TableName}";
            return await _context.ExecuteReaderQuery(cmd, CommandType.Text);
        }
        
        public ICollection<TodoItem> GetItemsByList(long listId)
        {
            var cmd = $"SELECT * FROM {TableName}";
            var items = _context.ExecuteReaderQuery(cmd, CommandType.Text).Result.ToList();
            return items.Where(x => x.ListId == listId).ToList();
        }

        public async Task<TodoItem> GetAsync(long id)
        {
            var cmd = $"SELECT * FROM {TableName} WHERE Id = @id";
            var result = await _context.ExecuteReaderQuery(cmd, CommandType.Text, new SqlParameter("@id", id));
            return result.FirstOrDefault();
        }

        public async Task<TodoItem> CreateAsync(TodoItem entity)
        {
            var cmd = $"INSERT INTO {TableName} (Title, Description, Completed, Priority, ListId, CreatedAt, UpdatedAt) VALUES (@title, @description, @completed, @priority, @listId, @createdAt, @updatedAt)";

            var audited = _context.UpsertTimeStamps(entity, true);
            
            var sqlParams = new[]
            {
                new SqlParameter("@title", entity.Title),
                new SqlParameter("@description", entity.Description),
                new SqlParameter("@completed", entity.Completed),
                new SqlParameter("@priority", entity.Priority),
                new SqlParameter("@listId", entity.ListId),
                new SqlParameter("@createdAt", audited.CreatedAt),
                new SqlParameter("@updatedAt", audited.UpdatedAt)
            };
            await _context.ExecuteNonQuery(cmd, CommandType.Text, sqlParams);
            return audited;
        }

        public async Task<TodoItem> UpdateAsync(TodoItem entity)
        {
            var cmd = $"UPDATE {TableName} SET Title = @title, Description = @description, Completed = @completed, Priority = @priority, ListId = @listId, UpdatedAt = @updatedAt WHERE Id = @id";

            var audited = _context.UpsertTimeStamps(entity);
            
            var sqlParams = new[]
            {
                new SqlParameter("@id", entity.Id),
                new SqlParameter("@title", entity.Title),
                new SqlParameter("@description", entity.Description),
                new SqlParameter("@completed", entity.Completed),
                new SqlParameter("@priority", entity.Priority),
                new SqlParameter("@listId", entity.ListId),
                new SqlParameter("@updatedAt", audited.UpdatedAt)
            };
            await _context.ExecuteNonQuery(cmd, CommandType.Text, sqlParams);
            return audited;
        }

        public async Task DeleteAsync(TodoItem entity)
        {
            var cmd = $"DELETE FROM {TableName} WHERE Id = @id";
            await _context.ExecuteNonQuery(cmd, CommandType.Text, new SqlParameter("@id", entity.Id));
        }
    }
}