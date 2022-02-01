using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Moq;
using Todos.Domain.Entities;
using Todos.Infrastructure.Persistence;
using Todos.Infrastructure.Repositories;
using Xunit;

namespace Todos.Infrastructure.Tests
{
    [ExcludeFromCodeCoverage]
    public class TodoListRepositoryTests
    {
        private readonly Mock<IDbContext<TodoList>> _mockDbContext = new Mock<IDbContext<TodoList>>();
        private readonly TodoListRepository _sut;
        
        public TodoListRepositoryTests()
        {
            _sut = new TodoListRepository(_mockDbContext.Object);
        }
        
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
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>()))
                .Returns(expectedLists);

            // Act
            var actual = await _sut.GetAllAsync();
            
            // Assert
            Assert.Equal(expectedLists, actual);
        }
        
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoEntriesPresent()
        {
            // Arrange
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>()))
                .Returns(new List<TodoList>());
            
            // Act
            var actual = await _sut.GetAllAsync();
            
            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnDefault_WhenItemNotFound()
        {
            // Arrange
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>()))
                .Returns(new List<TodoList>());
            
            // Act
            var actual = await _sut.GetAsync(1);
            
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
            var expectedLists = new List<TodoList>() { expectedList };
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>()))
                .Returns(expectedLists);

            // Act
            var actual = await _sut.GetAsync(1);
            
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

            // Act
            await _sut.CreateAsync(expectedList);
            
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
            var updatedItem = new TodoList()
            {
                Id = 1,
                Title = "Todo List Updated"
            };

            // Act
            await _sut.UpdateAsync(updatedItem);
            
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

            // Act
            await _sut.DeleteAsync(existingList);
            
            // Assert
            Assert.DoesNotContain(existingList, expectedItems);
        }
    }
}