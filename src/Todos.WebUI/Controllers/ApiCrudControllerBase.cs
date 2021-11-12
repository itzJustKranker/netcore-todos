using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Todos.Application.Interfaces;
using Todos.Domain.Common;

namespace Todos.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiCrudControllerBase<TEntity> : ControllerBase where TEntity : BaseEntity
    {
        private readonly ILogger _logger;
        private readonly IAsyncCrudRepository<TEntity> _repository;

        protected ApiCrudControllerBase(
            ILogger logger,
            IAsyncCrudRepository<TEntity> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            try
            {
                var allEntities = await _repository.GetAllAsync();
                return new OkObjectResult(allEntities);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[Todos] Unexpected error occurred within /getall");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpGet("{id:long}")]
        public virtual async Task<IActionResult> GetById([FromRoute] long id)
        {
            try
            {
                var entity = await _repository.GetAsync(id);
                return new OkObjectResult(entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[Todos] Unexpected error occurred within /getbyid");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPost("create")]
        public virtual async Task<IActionResult> Create([FromBody] TEntity entity)
        {
            try
            {
                var created = await _repository.CreateAsync(entity);
                return new OkObjectResult(created);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[Todos] Unexpected error occurred within /create");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPatch("{id:long}/update")]
        public virtual async Task<IActionResult> Update([FromRoute] long id, [FromBody] TEntity entity)
        {
            try
            {
                if (id != entity.Id)
                    return new BadRequestResult();

                var updated = await _repository.UpdateAsync(entity);
                return new OkObjectResult(updated);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[Todos] Unexpected error occurred within /update");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpDelete("{id:long}/delete")]
        public virtual async Task<IActionResult> Delete([FromRoute] long id, [FromBody] TEntity entity)
        {
            try
            {
                if (id != entity.Id)
                    return new BadRequestResult();
            
                await _repository.DeleteAsync(entity);
                return new NoContentResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[Todos] Unexpected error occurred within /delete");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}