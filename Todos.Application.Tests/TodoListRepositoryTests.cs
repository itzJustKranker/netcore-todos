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
    }
}