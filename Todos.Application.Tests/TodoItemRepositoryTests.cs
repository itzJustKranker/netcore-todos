using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Moq;
using Todos.Application.Interfaces;
using Todos.Application.Repositories;
using Todos.Application.Tests.Helpers;
using Todos.Domain.Entities;
using Xunit;

namespace Todos.Application.Tests
{
    [ExcludeFromCodeCoverage]
    public class TodoItemRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_ShouldReturnItems_WhenEntitiesExist()
        {
            // Arrange
            var expectedItems = new List<TodoItem>()
            {
                new TodoItem()
                {
                    Id = 1,
                    Title = "Test Item"
                }
            };
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(expectedItems);
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoItem>())
                .Returns(mockDbSet.Object);
            var sut = new TodoItemRepository(dbContextMock.Object);

            // Act
            var actual = await sut.GetAllAsync();
            
            // Assert
            Assert.Equal(expectedItems, actual);
        }
        
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoEntriesPresent()
        {
            // Arrange
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(new List<TodoItem>());
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoItem>())
                .Returns(mockDbSet.Object);
            var sut = new TodoItemRepository(dbContextMock.Object);

            // Act
            var actual = await sut.GetAllAsync();
            
            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnDefault_WhenItemNotFound()
        {
            // Arrange
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(new List<TodoItem>());
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoItem>())
                .Returns(mockDbSet.Object);
            var sut = new TodoItemRepository(dbContextMock.Object);
            
            // Act
            var actual = await sut.GetAsync(1);
            
            // Assert
            Assert.Null(actual);
        }
        
        [Fact]
        public async Task GetAsync_ShouldReturnExpectedItem_WhenItemExists()
        {
            // Arrange
            var expectedItem = new TodoItem()
            {
                Id = 1,
                Title = "Test Item"
            };
            var expectedItems = new List<TodoItem>() { expectedItem };
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(expectedItems);
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoItem>())
                .Returns(mockDbSet.Object);
            var sut = new TodoItemRepository(dbContextMock.Object);
            
            // Act
            var actual = await sut.GetAsync(1);
            
            // Assert
            Assert.Equal(expectedItem, actual);
        }
    }
}