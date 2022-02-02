using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Todos.Application.Interfaces;
using Todos.Domain.Entities;
using Todos.Infrastructure.Persistence;

namespace Todos.Infrastructure.Repositories
{
    public class TodoListRepository : ITodoListRepository
    {
        private readonly IDbContext<TodoList> _context;
        private const string TableName = "TodoLists";
        
        public TodoListRepository(IDbContext<TodoList> context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoList>> GetAllAsync()
        {
            var cmd = $"SELECT * FROM {TableName}";
            return await _context.ExecuteReaderQuery(cmd, CommandType.Text);
        }

        public async Task<TodoList> GetAsync(long id)
        {
            var cmd = $"SELECT * FROM {TableName} WHERE Id = @id";
            var result = await _context.ExecuteReaderQuery(cmd, CommandType.Text, new SqlParameter("@id", id));
            return result.FirstOrDefault();
        }

        public async Task<TodoList> CreateAsync(TodoList entity)
        {
            var cmd = $"INSERT INTO {TableName} (Title, CreatedAt, UpdatedAt) VALUES (@title, @createdAt, @updatedAt)";

            var audited = _context.UpsertTimeStamps(entity, true);
            
            var sqlParams = new[]
            {
                new SqlParameter("@title", entity.Title),
                new SqlParameter("@createdAt", audited.CreatedAt),
                new SqlParameter("@updatedAt", audited.UpdatedAt)
            };
            await _context.ExecuteNonQuery(cmd, CommandType.Text, sqlParams);
            return audited;
        }

        public async Task<TodoList> UpdateAsync(TodoList entity)
        {
            var cmd = $"UPDATE {TableName} SET Title = @title, UpdatedAt = @updatedAt WHERE Id = @id";

            var audited = _context.UpsertTimeStamps(entity);
            
            var sqlParams = new[]
            {
                new SqlParameter("@id", entity.Id),
                new SqlParameter("@title", entity.Title),
                new SqlParameter("@updatedAt", audited.UpdatedAt)
            };
            await _context.ExecuteNonQuery(cmd, CommandType.Text, sqlParams);
            return audited;
        }

        public async Task DeleteAsync(TodoList entity)
        {
            var cmd = $"DELETE FROM {TableName} WHERE Id = @id";
            await _context.ExecuteNonQuery(cmd, CommandType.Text, new SqlParameter("@id", entity.Id));
        }
    }
}