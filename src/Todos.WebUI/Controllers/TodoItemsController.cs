using Todos.Application.Interfaces;
using Todos.Domain.Entities;

namespace Todos.WebUI.Controllers
{
    public class TodoItemsController : ApiCrudControllerBase<TodoItem>
    {
        public TodoItemsController(ITodoItemRepository repository) : base(repository)
        {
        }
    }
}