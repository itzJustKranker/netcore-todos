using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class TodoItemRepositoryTests
    {
        private readonly Mock<IDbContext<TodoItem>> _mockDbContext = new Mock<IDbContext<TodoItem>>();
        private readonly TodoItemRepository _sut;
        
        public TodoItemRepositoryTests()
        {
            _sut = new TodoItemRepository(_mockDbContext.Object);
        }
        
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
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>()))
                .Returns(expectedItems);

            // Act
            var actual = await _sut.GetAllAsync();
            
            // Assert
            Assert.Equal(expectedItems, actual);
        }
        
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoEntriesPresent()
        {
            // Arrange
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>()))
                .Returns(new List<TodoItem>());
            
            // Act
            var actual = await _sut.GetAllAsync();
            
            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnDefault_WhenItemNotFound()
        {
            // Arrange
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .Returns(new List<TodoItem>());
            
            // Act
            var actual = await _sut.GetAsync(1);
            
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
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .Returns(expectedItems);

            // Act
            var actual = await _sut.GetAsync(1);
            
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

            // Act
            await _sut.CreateAsync(expectedItem);
            
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
            var updatedItem = new TodoItem()
            {
                Id = 1,
                Title = "Todo Item Updated"
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
            var existingItem = new TodoItem()
            {
                Id = 1,
                Title = "Todo Item"
            };
            var expectedItems = new List<TodoItem>() { existingItem };

            // Act
            await _sut.DeleteAsync(existingItem);
            
            // Assert
            Assert.DoesNotContain(existingItem, expectedItems);
        }
    }
}