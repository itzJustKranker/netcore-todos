using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Todos.Domain.Entities;
using Todos.Infrastructure.Repositories;
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
            var sut = new TodoListRepository();

            // Act
            var actual = await sut.GetAllAsync();
            
            // Assert
            Assert.Equal(expectedLists, actual);
        }
        
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoEntriesPresent()
        {
            // Arrange
            var sut = new TodoListRepository();

            // Act
            var actual = await sut.GetAllAsync();
            
            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnDefault_WhenItemNotFound()
        {
            // Arrange
            var sut = new TodoListRepository();

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
            var sut = new TodoListRepository();
            
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
            var sut = new TodoListRepository();
            
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
            var updatedItem = new TodoList()
            {
                Id = 1,
                Title = "Todo List Updated"
            };
            var sut = new TodoListRepository();
            
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
            var sut = new TodoListRepository();
            
            // Act
            await sut.DeleteAsync(existingList);
            
            // Assert
            Assert.DoesNotContain(existingList, expectedItems);
        }
    }
}