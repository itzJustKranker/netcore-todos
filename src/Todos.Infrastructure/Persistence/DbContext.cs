using Microsoft.Extensions.Options;
using Todos.Domain.Common;
using Todos.Infrastructure.Settings;
using MongoDB.Driver;

namespace Todos.Infrastructure.Persistence
{
    public abstract class DbContext<TEntity> where TEntity : BaseEntity
    {
        private readonly DatabaseSettings _settings;
        private readonly IMongoDatabase _mongoDatabase;

        protected DbContext(IOptions<DatabaseSettings> options)
        {
            _settings = options.Value;
            var client = new MongoClient(_settings.ConnectionString);
            _mongoDatabase = client.GetDatabase(_settings.DatabaseName);
        }
        
        protected abstract string CollectionName { get; }

        protected IMongoCollection<TEntity> Collection => _mongoDatabase.GetCollection<TEntity>(CollectionName);
    }
}