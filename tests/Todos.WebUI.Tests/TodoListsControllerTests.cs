namespace Todos.WebUI.Tests;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Todos.Application.Interfaces;
using Todos.Domain.Entities;
using Todos.WebUI.Controllers;
using Xunit;

[ExcludeFromCodeCoverage]
public class TodoListsControllerTests
{
    private readonly Mock<ITodoListRepository> _mockTodoListRepository = new();
    private readonly TodoListsController _sut;

    public TodoListsControllerTests()
    {
        _sut = new TodoListsController(new NullLogger<TodoListsController>(), _mockTodoListRepository.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnTodoLists()
    {
        // Arrange
        var expectedItems = new List<TodoList>
        {
            new()
            {
                Id = Guid.Empty,
                Title = "Todo List"
            }
        };
        _mockTodoListRepository.Setup(_ => _.GetAllAsync())
            .ReturnsAsync(expectedItems);

        // Act
        var result = await _sut.GetAll();
        var actual = result as OkObjectResult;

        // Assert
        _mockTodoListRepository.VerifyAll();

        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(actual);
        Assert.Equal(expectedItems, actual.Value);
    }

    [Fact]
    public async Task GetAll_ShouldReturn500_WhenRepoThrows()
    {
        // Arrange
        _mockTodoListRepository.Setup(_ => _.GetAllAsync())
            .ThrowsAsync(new Exception("Unexpected Error"));

        // Act
        var result = await _sut.GetAll();
        var actual = result as StatusCodeResult;

        // Assert
        _mockTodoListRepository.VerifyAll();

        Assert.NotNull(actual);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    public async Task GetById_ShouldReturnTodoList()
    {
        // Arrange
        var expectedItem = new TodoList
        {
            Id = Guid.Empty,
            Title = "Todo List"
        };
        _mockTodoListRepository.Setup(_ => _.GetAsync(Guid.Empty))
            .ReturnsAsync(expectedItem);

        // Act
        var result = await _sut.GetById(Guid.Empty);
        var actual = result as OkObjectResult;

        // Assert
        _mockTodoListRepository.VerifyAll();

        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(actual);
        Assert.Equal(expectedItem, actual.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturn500_WhenRepoThrows()
    {
        // Arrange
        _mockTodoListRepository.Setup(_ => _.GetAsync(Guid.Empty))
            .ThrowsAsync(new Exception("Unexpected Error"));

        // Act
        var result = await _sut.GetById(Guid.Empty);
        var actual = result as StatusCodeResult;

        // Assert
        _mockTodoListRepository.VerifyAll();

        Assert.NotNull(actual);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedList()
    {
        // Arrange
        var input = new TodoList { Title = "Todo List" };
        var expectedItem = new TodoList
        {
            Id = Guid.Empty,
            Title = "Todo List"
        };
        _mockTodoListRepository.Setup(_ => _.CreateAsync(input))
            .ReturnsAsync(expectedItem);

        // Act
        var result = await _sut.Create(input);
        var actual = result as OkObjectResult;

        // Assert
        _mockTodoListRepository.VerifyAll();

        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(actual);
        Assert.Equal(expectedItem, actual.Value);
    }

    [Fact]
    public async Task Create_ShouldReturn500_WhenRepoThrows()
    {
        // Arrange
        var input = new TodoList { Title = "Todo List" };
        _mockTodoListRepository.Setup(_ => _.CreateAsync(input))
            .ThrowsAsync(new Exception("Unexpected Error"));

        // Act
        var result = await _sut.Create(input);
        var actual = result as StatusCodeResult;

        // Assert
        _mockTodoListRepository.VerifyAll();

        Assert.NotNull(actual);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    public async Task Update_ShouldReturnUpdatedList()
    {
        // Arrange
        var input = new TodoList { Id = Guid.Empty, Title = "Todo List Updated" };
        var expectedItem = new TodoList
        {
            Id = Guid.Empty,
            Title = "Todo List Updated"
        };
        _mockTodoListRepository.Setup(_ => _.UpdateAsync(input))
            .ReturnsAsync(expectedItem);

        // Act
        var result = await _sut.Update(input.Id, input);
        var actual = result as OkObjectResult;

        // Assert
        _mockTodoListRepository.VerifyAll();

        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(actual);
        Assert.Equal(expectedItem, actual.Value);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var input = new TodoList { Id = Guid.Empty, Title = "Todo List Updated" };

        // Act
        var result = await _sut.Update(Guid.NewGuid(), input);

        // Assert
        _mockTodoListRepository.VerifyAll();

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturn500_WhenRepoThrows()
    {
        // Arrange
        var input = new TodoList { Id = Guid.Empty, Title = "Todo List Updated" };
        _mockTodoListRepository.Setup(_ => _.UpdateAsync(input))
            .ThrowsAsync(new Exception("Unexpected Error"));

        // Act
        var result = await _sut.Update(input.Id, input);
        var actual = result as StatusCodeResult;

        // Assert
        _mockTodoListRepository.VerifyAll();

        Assert.NotNull(actual);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldRemoveList()
    {
        // Arrange
        var input = new TodoList { Id = Guid.Empty, Title = "Todo List To Delete" };
        _mockTodoListRepository.Setup(_ => _.DeleteAsync(input))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.Delete(Guid.Empty, input);

        // Assert
        _mockTodoListRepository.Verify(_ => _.DeleteAsync(input), Times.Once);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var input = new TodoList { Id = Guid.Empty, Title = "Todo List To Delete" };

        // Act
        var result = await _sut.Delete(Guid.NewGuid(), input);

        // Assert
        _mockTodoListRepository.Verify(_ => _.DeleteAsync(It.IsAny<TodoList>()), Times.Never);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturn500_WhenRepoThrows()
    {
        // Arrange
        var input = new TodoList { Id = Guid.Empty, Title = "Todo List To Delete" };
        _mockTodoListRepository.Setup(_ => _.DeleteAsync(input))
            .ThrowsAsync(new Exception("Unexpected Error"));

        // Act
        var result = await _sut.Delete(Guid.Empty, input);
        var actual = result as StatusCodeResult;

        // Assert
        _mockTodoListRepository.VerifyAll();

        Assert.NotNull(actual);
        Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
    }
}