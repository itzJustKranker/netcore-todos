using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Todos.Application.Interfaces;
using Todos.Domain.Common;

namespace Todos.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiCrudControllerBase<TEntity> : ControllerBase where TEntity : BaseEntity
    {
        private readonly IAsyncCrudRepository<TEntity> _repository;

        public ApiCrudControllerBase(IAsyncCrudRepository<TEntity> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var allEntities = await _repository.GetAllAsync();
            return new OkObjectResult(allEntities);
        }
        
        [HttpGet("{id:long}")]
        public virtual async Task<IActionResult> GetById([FromRoute] long id)
        {
            var entity = await _repository.GetAsync(id);
            return new OkObjectResult(entity);
        }
        
        [HttpPost("create")]
        public virtual async Task<IActionResult> Create([FromBody] TEntity entity)
        {
            var created = await _repository.CreateAsync(entity);
            return new OkObjectResult(created);
        }
        
        [HttpPatch("{id:long}/update")]
        public virtual async Task<IActionResult> Update([FromRoute] long id, [FromBody] TEntity entity)
        {
            if (id != entity.Id)
                return new BadRequestResult();

            var updated = await _repository.UpdateAsync(entity);
            return new OkObjectResult(updated);
        }
        
        [HttpDelete("{id:long}/delete")]
        public virtual async Task<IActionResult> Delete([FromRoute] long id, [FromBody] TEntity entity)
        {
            if (id != entity.Id)
                return new BadRequestResult();
            
            await _repository.DeleteAsync(entity);
            return new NoContentResult();
        }
    }
}