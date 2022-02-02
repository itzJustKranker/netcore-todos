using System;
using System.Collections.Generic;
using System.Data;
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
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>(), CommandType.Text))
                .ReturnsAsync(expectedLists);

            // Act
            var actual = await _sut.GetAllAsync();
            
            // Assert
            Assert.Equal(expectedLists, actual);
        }
        
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoEntriesPresent()
        {
            // Arrange
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>(), CommandType.Text))
                .ReturnsAsync(new List<TodoList>());
            
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
                .ReturnsAsync(new List<TodoList>());
            
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
            _mockDbContext.Setup(x => x.ExecuteReaderQuery(It.IsAny<string>(), CommandType.Text, It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedLists);

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
                Title = "Todo List Created",
                CreatedAt = DateTime.Parse("2022-02-02T19:05:24.417841Z"),
                UpdatedAt = DateTime.Parse("2022-02-02T19:05:24.417841Z"),
            };
            _mockDbContext.Setup(x => x.UpsertTimeStamps(It.IsAny<TodoList>(), It.IsAny<bool>()))
                .Returns(expectedList);

            // Act
            var actual = await _sut.CreateAsync(expectedList);
            
            // Assert
            _mockDbContext.Verify(x => 
                    x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<SqlParameter[]>()),
                Times.Once);
            
            Assert.Equal(expectedList, actual);
        }
        
        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingItem()
        {
            // Arrange
            var existingList = new TodoList()
            {
                Id = 1,
                Title = "Todo List",
                CreatedAt = DateTime.Parse("2022-02-02T19:05:24.417841Z"),
                UpdatedAt = DateTime.Parse("2022-02-02T19:05:24.417841Z"),
            };
            var updatedList = new TodoList()
            {
                Id = 1,
                Title = "Todo List Updated",
                CreatedAt = DateTime.Parse("2022-02-02T19:05:24.417841Z"),
                UpdatedAt = DateTime.Parse("2022-02-02T19:05:30.417841Z"),
            };
            _mockDbContext.Setup(x => x.UpsertTimeStamps(It.IsAny<TodoList>(), It.IsAny<bool>()))
                .Returns(updatedList);

            // Act
            var actual = await _sut.UpdateAsync(existingList);
            
            // Assert
            _mockDbContext.Verify(x => 
                    x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<SqlParameter[]>()),
                Times.Once);
            
            Assert.Equal(updatedList, actual);
        }
        
        [Fact]
        public async Task DeleteAsync_ShouldRemoveExistingItem()
        {
            // Arrange
            var existingList = new TodoList()
            {
                Id = 1,
                Title = "Todo Item"
            };

            // Act
            await _sut.DeleteAsync(existingList);
            
            // Assert
            _mockDbContext.Verify(x => 
                    x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<SqlParameter[]>()),
                Times.Once);
        }
    }
}