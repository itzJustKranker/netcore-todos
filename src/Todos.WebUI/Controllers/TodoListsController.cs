using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Todos.Application.Interfaces;
using Todos.Domain.Entities;

namespace Todos.WebUI.Controllers
{
    public class TodoListsController : ApiCrudControllerBase<TodoList>
    {
        private readonly ITodoListRepository _repository;
        
        public TodoListsController(ITodoListRepository repository) : base(repository)
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