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

public class TodoListRepository : DbContext<TodoList>, ITodoListRepository
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public TodoListRepository(
        IOptions<DatabaseSettings> options,
        IDateTimeProvider dateTimeProvider) : base(options)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    protected override string CollectionName => "lists";

    public async Task<IEnumerable<TodoList>> GetAllAsync()
    {
        var results = await Collection.FindAsync(FilterDefinition<TodoList>.Empty);
        return await results.ToListAsync();
    }

    public async Task<TodoList> GetAsync(Guid id)
    {
        var filter = Builders<TodoList>.Filter.Eq(x => x.Id, id);
        var result = await Collection.FindAsync(filter);
        return await result.FirstOrDefaultAsync();
    }

    public async Task<TodoList> CreateAsync(TodoList entity)
    {
        entity.CreatedAt = _dateTimeProvider.UtcNow;
        entity.UpdatedAt = _dateTimeProvider.UtcNow;

        await Collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<TodoList> UpdateAsync(TodoList entity)
    {
        var filter = Builders<TodoList>.Filter.Eq(x => x.Id, entity.Id);

        entity.UpdatedAt = _dateTimeProvider.UtcNow;

        var builder = Builders<TodoList>.Update.Combine(
            Builders<TodoList>.Update.Set(x => x.Title, entity.Title),
            Builders<TodoList>.Update.Set(x => x.UpdatedAt, entity.UpdatedAt));

        await Collection.UpdateOneAsync(filter, builder);

        return entity;
    }

    public async Task DeleteAsync(TodoList entity)
    {
        var filter = Builders<TodoList>.Filter.Eq(x => x.Id, entity.Id);
        await Collection.DeleteOneAsync(filter);
    }
}