using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Todos.Application.Interfaces;
using Todos.Domain.Entities;
using Todos.WebUI.Controllers;
using Xunit;

namespace Todos.WebUI.Tests
{
    [ExcludeFromCodeCoverage]
    public class TodoItemsControllerTests
    {
        private readonly Mock<ITodoItemRepository> _mockTodoItemRepository = new Mock<ITodoItemRepository>();
        private readonly TodoItemsController _sut;

        public TodoItemsControllerTests()
        {
            _sut = new TodoItemsController(_mockTodoItemRepository.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnTodoItems()
        {
            // Arrange
            var expectedItems = new List<TodoItem>()
            {
                new TodoItem()
                {
                    Id = 1,
                    Title = "Todo Item"
                }
            };
            _mockTodoItemRepository.Setup(_ => _.GetAllAsync())
                .ReturnsAsync(expectedItems);
            
            // Act
            var result = await _sut.GetAll();
            var actual = result as OkObjectResult;

            // Assert
            _mockTodoItemRepository.VerifyAll();
            
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(actual);
            Assert.Equal(expectedItems, actual.Value);
        }
        
        [Fact]
        public async Task GetById_ShouldReturnTodoItem()
        {
            // Arrange
            var expectedItem = new TodoItem()
            {
                Id = 1,
                Title = "Todo Item"
            };
            _mockTodoItemRepository.Setup(_ => _.GetAsync(1))
                .ReturnsAsync(expectedItem);
            
            // Act
            var result = await _sut.GetById(1);
            var actual = result as OkObjectResult;

            // Assert
            _mockTodoItemRepository.VerifyAll();
            
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(actual);
            Assert.Equal(expectedItem, actual.Value);
        }
        
        [Fact]
        public async Task Create_ShouldReturnCreatedItem()
        {
            // Arrange
            var input = new TodoItem() {Title = "Todo Item"};
            var expectedItem = new TodoItem()
            {
                Id = 1,
                Title = "Todo Item"
            };
            _mockTodoItemRepository.Setup(_ => _.CreateAsync(input))
                .ReturnsAsync(expectedItem);
            
            // Act
            var result = await _sut.Create(input);
            var actual = result as OkObjectResult;

            // Assert
            _mockTodoItemRepository.VerifyAll();
            
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(actual);
            Assert.Equal(expectedItem, actual.Value);
        }
        
        [Fact]
        public async Task Update_ShouldReturnUpdatedEntity()
        {
            // Arrange
            var input = new TodoItem() { Id = 1, Title = "Todo Item Updated"};
            var expectedItem = new TodoItem()
            {
                Id = 1,
                Title = "Todo Item Updated"
            };
            _mockTodoItemRepository.Setup(_ => _.UpdateAsync(input))
                .ReturnsAsync(expectedItem);
            
            // Act
            var result = await _sut.Update(input.Id, input);
            var actual = result as OkObjectResult;

            // Assert
            _mockTodoItemRepository.VerifyAll();
            
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(actual);
            Assert.Equal(expectedItem, actual.Value);
        }
        
        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var input = new TodoItem() { Id = 1, Title = "Todo Item Updated"};

            // Act
            var result = await _sut.Update(2, input);

            // Assert
            _mockTodoItemRepository.VerifyAll();
            
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldRemoveItem()
        {
            // Arrange
            var input = new TodoItem() { Id = 1, Title = "Todo Item To Delete"};
            _mockTodoItemRepository.Setup(_ => _.DeleteAsync(input))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.Delete(1, input);

            // Assert
            _mockTodoItemRepository.Verify(_ => _.DeleteAsync(input), Times.Once);
            
            Assert.IsType<NoContentResult>(result);
        }
        
        [Fact]
        public async Task Delete_ShouldReturnBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var input = new TodoItem() { Id = 1, Title = "Todo Item To Delete"};

            // Act
            var result = await _sut.Delete(2, input);

            // Assert
            _mockTodoItemRepository.Verify(_ => _.DeleteAsync(It.IsAny<TodoItem>()), Times.Never);
            
            Assert.IsType<BadRequestResult>(result);
        }
    }
}