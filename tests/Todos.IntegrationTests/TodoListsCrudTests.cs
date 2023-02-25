namespace Todos.IntegrationTests;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

[ExcludeFromCodeCoverage]
[Collection("IntegrationTests")]
public class TodoListsCrudTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly IntegrationTestWebApplicationFactory _factory;        

    public TodoListsCrudTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task CreateList_WhenRequestSentToController()
    {
        // Arrange
        var client = _factory.CreateClient();

        var json = """
        {
            "id": "4c754889-6010-42a6-b70c-f37565bbb54d",
            "title": "new"
        }
        """;
        var url = "/api/todolists";
        var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(url, content);
        var actual = await _factory.TodoListRepository.GetAsync(Guid.Parse("4c754889-6010-42a6-b70c-f37565bbb54d"));
            
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(actual);
    }
}