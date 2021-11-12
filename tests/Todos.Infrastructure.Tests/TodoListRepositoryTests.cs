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
    public class TodoListRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_ShouldReturnItems_WhenEntitiesExist()
        {
            // Arrange
            var expectedLists = new List<TodoList>()
            {
                new TodoList()
                {
                    Id = 1,
                    Title = "Test List"
                }
            };
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(expectedLists);
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoList>())
                .Returns(mockDbSet.Object);
            var sut = new TodoListRepository(dbContextMock.Object);

            // Act
            var actual = await sut.GetAllAsync();
            
            // Assert
            Assert.Equal(expectedLists, actual);
        }
        
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoEntriesPresent()
        {
            // Arrange
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(new List<TodoList>());
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoList>())
                .Returns(mockDbSet.Object);
            var sut = new TodoListRepository(dbContextMock.Object);

            // Act
            var actual = await sut.GetAllAsync();
            
            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnDefault_WhenItemNotFound()
        {
            // Arrange
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(new List<TodoList>());
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoList>())
                .Returns(mockDbSet.Object);
            var sut = new TodoListRepository(dbContextMock.Object);

            // Act
            var actual = await sut.GetAsync(1);
            
            // Assert
            Assert.Null(actual);
        }
        
        [Fact]
        public async Task GetAsync_ShouldReturnExpectedItem_WhenItemExists()
        {
            // Arrange
            var expectedList = new TodoList()
            {
                Id = 1,
                Title = "Test List"
            };
            var expectedItems = new List<TodoList>() { expectedList };
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(expectedItems);
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoList>())
                .Returns(mockDbSet.Object);
            var sut = new TodoListRepository(dbContextMock.Object);
            
            // Act
            var actual = await sut.GetAsync(1);
            
            // Assert
            Assert.Equal(expectedList, actual);
        }
        
        [Fact]
        public async Task CreateAsync_ShouldCreateItem()
        {
            // Arrange
            var expectedList = new TodoList()
            {
                Id = 1,
                Title = "Todo List Created"
            };
            var expectedItems = new List<TodoList>();
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(expectedItems);
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoList>())
                .Returns(mockDbSet.Object);

            mockDbSet.Setup(_ => _.AddAsync(It.IsAny<TodoList>(), It.IsAny<CancellationToken>()))
                .Callback<TodoList, CancellationToken>((item, token) => expectedItems.Add(item));

            var sut = new TodoListRepository(dbContextMock.Object);
            
            // Act
            await sut.CreateAsync(expectedList);
            
            // Assert
            Assert.Contains(expectedList, expectedItems);
        }
        
        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingItem()
        {
            // Arrange
            var existingList = new TodoList()
            {
                Id = 1,
                Title = "Todo List"
            };
            var expectedItems = new List<TodoList>() { existingList };
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(expectedItems);
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoList>())
                .Returns(mockDbSet.Object);

            var updatedItem = new TodoList()
            {
                Id = 1,
                Title = "Todo List Updated"
            };
            
            mockDbSet.Setup(_ => _.Update(It.IsAny<TodoList>()))
                .Callback<TodoList>(item =>
                    expectedItems[expectedItems.FindIndex(x => x.Id == existingList.Id)] = item);

            var sut = new TodoListRepository(dbContextMock.Object);
            
            // Act
            await sut.UpdateAsync(updatedItem);
            
            // Assert
            Assert.Contains(updatedItem, expectedItems);
        }
        
        [Fact]
        public async Task DeleteAsync_ShouldRemoveExistingItem()
        {
            // Arrange
            var existingList = new TodoList()
            {
                Id = 1,
                Title = "Todo List"
            };
            var expectedItems = new List<TodoList>() { existingList };
            var mockDbSet = DbContextMockHelper.GetQueryableMockDbSet(expectedItems);
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(_ => _.Set<TodoList>())
                .Returns(mockDbSet.Object);

            mockDbSet.Setup(_ => _.Remove(It.IsAny<TodoList>()))
                .Callback<TodoList>(item => expectedItems.Remove(item));

            var sut = new TodoListRepository(dbContextMock.Object);
            
            // Act
            await sut.DeleteAsync(existingList);
            
            // Assert
            Assert.DoesNotContain(existingList, expectedItems);
        }
    }
}