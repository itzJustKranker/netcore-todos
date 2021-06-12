using Todos.Application.Interfaces;
using Todos.Domain.Entities;

namespace Todos.WebUI.Controllers
{
    public class TodoListsController : ApiCrudControllerBase<TodoList>
    {
        public TodoListsController(IAsyncCrudRepository<TodoList> repository) : base(repository)
        {
        }
    }
}