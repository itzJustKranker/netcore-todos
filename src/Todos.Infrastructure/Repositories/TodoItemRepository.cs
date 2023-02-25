namespace Todos.Infrastructure.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Todos.Application.Interfaces;
using Todos.Domain.Entities;
using Todos.Infrastructure.Persistence;
using Todos.Infrastructure.Settings;

public class TodoItemRepository : DbContext<TodoItem>, ITodoItemRepository
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public TodoItemRepository(
        IOptions<DatabaseSettings> options,
        IDateTimeProvider dateTimeProvider) : base(options)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    protected override string CollectionName => "items";

    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        var results = await Collection.FindAsync(FilterDefinition<TodoItem>.Empty);
        return await results.ToListAsync();
    }

    public async Task<TodoItem> GetAsync(Guid id)
    {
        var filter = Builders<TodoItem>.Filter.Eq(x => x.Id, id);
        var result = await Collection.FindAsync(filter);
        return await result.FirstOrDefaultAsync();
    }

    public async Task<TodoItem> CreateAsync(TodoItem entity)
    {
        entity.CreatedAt = _dateTimeProvider.UtcNow;
        entity.UpdatedAt = _dateTimeProvider.UtcNow;

        await Collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<TodoItem> UpdateAsync(TodoItem entity)
    {
        var filter = Builders<TodoItem>.Filter.Eq(x => x.Id, entity.Id);

        entity.UpdatedAt = _dateTimeProvider.UtcNow;

        var builder = Builders<TodoItem>.Update.Combine(
            Builders<TodoItem>.Update.Set(x => x.Title, entity.Title),
            Builders<TodoItem>.Update.Set(x => x.Description, entity.Description),
            Builders<TodoItem>.Update.Set(x => x.Completed, entity.Completed),
            Builders<TodoItem>.Update.Set(x => x.Priority, entity.Priority),
            Builders<TodoItem>.Update.Set(x => x.ListId, entity.ListId),
            Builders<TodoItem>.Update.Set(x => x.UpdatedAt, entity.UpdatedAt));

        await Collection.UpdateOneAsync(filter, builder);

        return entity;
    }

    public async Task DeleteAsync(TodoItem entity)
    {
        var filter = Builders<TodoItem>.Filter.Eq(x => x.Id, entity.Id);
        await Collection.DeleteOneAsync(filter);
    }
}