using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Moq;
using Todos.Application.Interfaces;
using Todos.Domain.Entities;
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
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>(), CommandType.Text))
                .ReturnsAsync(expectedItems);

            // Act
            var actual = await _sut.GetAllAsync();
            
            // Assert
            Assert.Equal(expectedItems, actual);
        }
        
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoEntriesPresent()
        {
            // Arrange
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>(), CommandType.Text))
                .ReturnsAsync(new List<TodoItem>());
            
            // Act
            var actual = await _sut.GetAllAsync();
            
            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnDefault_WhenItemNotFound()
        {
            // Arrange
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>(), CommandType.Text, It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(new List<TodoItem>());
            
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
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>(), CommandType.Text, It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedItems);

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
                Title = "Todo Created",
                CreatedAt = DateTime.Parse("2022-02-02T19:05:24.417841Z"),
                UpdatedAt = DateTime.Parse("2022-02-02T19:05:24.417841Z"),
            };
            _mockDbContext.Setup(x => x.UpsertTimeStamps(It.IsAny<TodoItem>(), It.IsAny<bool>()))
                .Returns(expectedItem);

            // Act
            var actual = await _sut.CreateAsync(expectedItem);
            
            // Assert
            _mockDbContext.Verify(x => 
                    x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<SqlParameter[]>()),
                Times.Once);
            
            Assert.Equal(expectedItem, actual);
        }
        
        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingItem()
        {
            // Arrange
            var existingItem = new TodoItem()
            {
                Id = 1,
                Title = "Todo Item",
                CreatedAt = DateTime.Parse("2022-02-02T19:05:24.417841Z"),
                UpdatedAt = DateTime.Parse("2022-02-02T19:05:24.417841Z"),
            };
            var updatedItem = new TodoItem()
            {
                Id = 1,
                Title = "Todo Item Updated",
                CreatedAt = DateTime.Parse("2022-02-02T19:05:24.417841Z"),
                UpdatedAt = DateTime.Parse("2022-02-02T19:05:30.417841Z"),
            };
            _mockDbContext.Setup(x => x.UpsertTimeStamps(It.IsAny<TodoItem>(), It.IsAny<bool>()))
                .Returns(updatedItem);

            // Act
            var actual = await _sut.UpdateAsync(existingItem);
            
            // Assert
            _mockDbContext.Verify(x => 
                    x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<SqlParameter[]>()),
                Times.Once);
            
            Assert.Equal(updatedItem, actual);
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

            // Act
            await _sut.DeleteAsync(existingItem);
            
            // Assert
            _mockDbContext.Verify(x => 
                    x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<SqlParameter[]>()),
                Times.Once);
        }
    }
}