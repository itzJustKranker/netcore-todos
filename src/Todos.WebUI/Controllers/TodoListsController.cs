using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Todos.Application.Interfaces;
using Todos.Domain.Entities;

namespace Todos.WebUI.Controllers
{
    // Route: "api/[controller]" (i.e. api/todolists)
    public class TodoListsController : ApiCrudControllerBase<TodoList>
    {
        private readonly ITodoListRepository _repository;
        
        public TodoListsController(
            ILogger<TodoListsController> logger,
            ITodoListRepository repository) : base(logger, repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAll()
        {
            var allLists = await _repository.GetAllAsync();
            return new OkObjectResult(allLists);
        }
    }
}