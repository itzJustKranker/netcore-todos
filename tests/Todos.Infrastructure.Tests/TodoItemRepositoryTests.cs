using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Todos.Application.Interfaces;
using Todos.Domain.Entities;
using Todos.Infrastructure.Repositories;
using Todos.Infrastructure.Tests.Helpers;
using Xunit;

namespace Todos.Infrastructure.Tests
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

        [Fact]
        public async Task CreateAsync_ShouldCreateItem()
        {
            // Arrange
            var expectedItem = new TodoItem()
            {
                Id = 1,
                Title = "Todo Created"
            };
            var expectedItems = new List<TodoItem>();
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(expectedItems);
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoItem>())
                .Returns(mockDbSet.Object);

            mockDbSet.Setup(_ => _.AddAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()))
                .Callback<TodoItem, CancellationToken>((item, token) => expectedItems.Add(item));

            var sut = new TodoItemRepository(dbContextMock.Object);
            
            // Act
            await sut.CreateAsync(expectedItem);
            
            // Assert
            Assert.Contains(expectedItem, expectedItems);
        }
        
        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingItem()
        {
            // Arrange
            var existingItem = new TodoItem()
            {
                Id = 1,
                Title = "Todo Item"
            };
            var expectedItems = new List<TodoItem>() { existingItem };
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(expectedItems);
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoItem>())
                .Returns(mockDbSet.Object);

            var updatedItem = new TodoItem()
            {
                Id = 1,
                Title = "Todo Item Updated"
            };
            
            mockDbSet.Setup(_ => _.Update(It.IsAny<TodoItem>()))
                .Callback<TodoItem>(item =>
                    expectedItems[expectedItems.FindIndex(x => x.Id == existingItem.Id)] = item);

            var sut = new TodoItemRepository(dbContextMock.Object);
            
            // Act
            await sut.UpdateAsync(updatedItem);
            
            // Assert
            Assert.Contains(updatedItem, expectedItems);
        }
        
        [Fact]
        public async Task DeleteAsync_ShouldRemoveExistingItem()
        {
            // Arrange
            var existingItem = new TodoItem()
            {
                Id = 1,
                Title = "Todo Item"
            };
            var expectedItems = new List<TodoItem>() { existingItem };
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(expectedItems);
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoItem>())
                .Returns(mockDbSet.Object);

            mockDbSet.Setup(_ => _.Remove(It.IsAny<TodoItem>()))
                .Callback<TodoItem>(item => expectedItems.Remove(item));

            var sut = new TodoItemRepository(dbContextMock.Object);
            
            // Act
            await sut.DeleteAsync(existingItem);
            
            // Assert
            Assert.DoesNotContain(existingItem, expectedItems);
        }
    }
}