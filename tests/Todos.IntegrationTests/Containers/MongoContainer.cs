namespace Todos.IntegrationTests.Containers
{
    using System.Diagnostics.CodeAnalysis;
    using DotNet.Testcontainers.Builders;
    using DotNet.Testcontainers.Configurations;
    using DotNet.Testcontainers.Containers;

    [ExcludeFromCodeCoverage]
    public class MongoContainer
    {
        /// <summary>
        /// Creates MongoDB test container with defaults. This would be 
        /// used with the connection string: "mongodb://root:example@localhost:27017"
        /// </summary>
        /// <returns>A MongoDB TestContainer instance.</returns>
        public static MongoDbTestcontainer Create()
        {
            return Create("db", "root", "example", 27017);
        }
        
        /// <summary>
        /// Creates a MongoDB test container.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <param name="port">Port.</param>
        /// <returns>A MongoDB TestContainer instance.</returns>
        public static MongoDbTestcontainer Create(
            string databaseName, 
            string username, 
            string password, 
            int port)
        {
            return new TestcontainersBuilder<MongoDbTestcontainer>()
                .WithDatabase(new MongoDbTestcontainerConfiguration()
                {
                    Database = databaseName,
                    Username = username,
                    Password = password,
                    Port = port
                })
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(port))
                .WithCleanUp(true)
                .WithAutoRemove(true)
                .Build();
        }
    }
}
