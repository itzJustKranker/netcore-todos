using Microsoft.Extensions.Logging;
using Todos.Application.Interfaces;
using Todos.Domain.Entities;

namespace Todos.WebUI.Controllers
{
    // Route: "api/[controller]" (i.e. api/todolists)
    public class TodoListsController : ApiCrudControllerBase<TodoList>
    {
        public TodoListsController(
            ILogger<TodoListsController> logger,
            ITodoListRepository repository) : base(logger, repository)
        {
        }
    }
}