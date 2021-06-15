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
    public class TodoListsControllerTests
    {
        private readonly Mock<ITodoListRepository> _mockTodoItemRepository = new Mock<ITodoListRepository>();
        private readonly TodoListsController _sut;

        public TodoListsControllerTests()
        {
            _sut = new TodoListsController(_mockTodoItemRepository.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnTodoLists()
        {
            // Arrange
            var expectedItems = new List<TodoList>()
            {
                new TodoList()
                {
                    Id = 1,
                    Title = "Todo List"
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
        public async Task GetById_ShouldReturnTodoList()
        {
            // Arrange
            var expectedItem = new TodoList()
            {
                Id = 1,
                Title = "Todo List"
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
        public async Task Create_ShouldReturnCreatedList()
        {
            // Arrange
            var input = new TodoList() {Title = "Todo List"};
            var expectedItem = new TodoList()
            {
                Id = 1,
                Title = "Todo List"
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
        public async Task Update_ShouldReturnUpdatedList()
        {
            // Arrange
            var input = new TodoList() { Id = 1, Title = "Todo List Updated"};
            var expectedItem = new TodoList()
            {
                Id = 1,
                Title = "Todo List Updated"
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
            var input = new TodoList() { Id = 1, Title = "Todo List Updated"};

            // Act
            var result = await _sut.Update(2, input);

            // Assert
            _mockTodoItemRepository.VerifyAll();
            
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldRemoveList()
        {
            // Arrange
            var input = new TodoList() { Id = 1, Title = "Todo List To Delete"};
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
            var input = new TodoList() { Id = 1, Title = "Todo List To Delete"};

            // Act
            var result = await _sut.Delete(2, input);

            // Assert
            _mockTodoItemRepository.Verify(_ => _.DeleteAsync(It.IsAny<TodoList>()), Times.Never);
            
            Assert.IsType<BadRequestResult>(result);
        }
    }
}