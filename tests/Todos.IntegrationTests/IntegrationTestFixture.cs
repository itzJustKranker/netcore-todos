namespace Todos.IntegrationTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using DotNet.Testcontainers.Containers;
    using Todos.IntegrationTests.Containers;

    [ExcludeFromCodeCoverage]
    public class IntegrationTestFixture : IAsyncDisposable
    {
        public MongoDbTestcontainer MongoDbContainer { get; private set; }

        public IntegrationTestFixture()
        {
            MongoDbContainer = MongoContainer.Create();            
            Task.Run(StartContainer).Wait();
        }
        
        private async Task StartContainer()
        {
            await MongoDbContainer.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await MongoDbContainer.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}
