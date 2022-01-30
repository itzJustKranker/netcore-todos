using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Todos.Domain.Entities;
using Todos.Infrastructure.Repositories;
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
            var sut = new TodoItemRepository();

            // Act
            var actual = await sut.GetAllAsync();
            
            // Assert
            Assert.Equal(expectedItems, actual);
        }
        
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoEntriesPresent()
        {
            // Arrange
            var sut = new TodoItemRepository();

            // Act
            var actual = await sut.GetAllAsync();
            
            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnDefault_WhenItemNotFound()
        {
            // Arrange
            var sut = new TodoItemRepository();
            
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
            var sut = new TodoItemRepository();
            
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
            var sut = new TodoItemRepository();
            
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
            var updatedItem = new TodoItem()
            {
                Id = 1,
                Title = "Todo Item Updated"
            };
            var sut = new TodoItemRepository();
            
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
            var sut = new TodoItemRepository();
            
            // Act
            await sut.DeleteAsync(existingItem);
            
            // Assert
            Assert.DoesNotContain(existingItem, expectedItems);
        }
    }
}