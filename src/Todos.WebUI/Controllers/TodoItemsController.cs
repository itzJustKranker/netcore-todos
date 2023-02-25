namespace Todos.WebUI.Controllers;

using Microsoft.Extensions.Logging;
using Todos.Application.Interfaces;
using Todos.Domain.Entities;

// Route: "api/[controller]" (i.e. api/todoitems)
public class TodoItemsController : ApiCrudControllerBase<TodoItem>
{
    public TodoItemsController(
        ILogger<TodoItemsController> logger,
        ITodoItemRepository repository) : base(logger, repository)
    {
    }
}