namespace Todos.Infrastructure.Persistence;

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Todos.Domain.Common;
using Todos.Infrastructure.Settings;

public abstract class DbContext<TEntity> where TEntity : BaseEntity
{
    private readonly IMongoDatabase _mongoDatabase;
    private readonly DatabaseSettings _settings;

    protected DbContext(IOptions<DatabaseSettings> options)
    {
        _settings = options.Value;
        var client = new MongoClient(_settings.ConnectionString);
        _mongoDatabase = client.GetDatabase(_settings.DatabaseName);
    }

    protected abstract string CollectionName { get; }

    protected IMongoCollection<TEntity> Collection => _mongoDatabase.GetCollection<TEntity>(CollectionName);
}