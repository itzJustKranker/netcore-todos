using Microsoft.Extensions.Logging;
using Todos.Application.Interfaces;
using Todos.Domain.Entities;

namespace Todos.WebUI.Controllers
{
    // Route: "api/[controller]" (i.e. api/todoitems)
    public class TodoItemsController : ApiCrudControllerBase<TodoItem>
    {
        public TodoItemsController(
            ILogger<TodoItemsController> logger, 
            ITodoItemRepository repository) : base(logger, repository)
        {
        }
    }
}