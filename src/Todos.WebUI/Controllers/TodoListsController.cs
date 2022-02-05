using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Todos.Application.Interfaces;
using Todos.Domain.Entities;
using AutoMapper;

namespace Todos.WebUI.Controllers
{
    // Route: "api/[controller]" (i.e. api/todolists)
    public class TodoListsController : ApiCrudControllerBase<TodoList>
    {
        private readonly ITodoListRepository _repository;
        private readonly ITodoItemRepository _todoItemRepository;
        
        public TodoListsController(
            ILogger<TodoListsController> logger,
            ITodoListRepository repository, 
            ITodoItemRepository todoItemRepository) : base(logger, repository)
        {
            _repository = repository;
            _todoItemRepository = todoItemRepository;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAll()
        {
            var allLists = await _repository.GetAllAsync();
            return new OkObjectResult(allLists);
        }
        
        [HttpGet]
        [Route("getallwithitems")]
        public List<TodoList> getAllWithItems()
        {
            try
            {
                // GetAll() endpoint will fetch all lists
                var listsResult = (OkObjectResult)GetAll().Result;
                var lists = (List<TodoList>)listsResult.Value;

                foreach (var list in lists)
                {
                    var items = _todoItemRepository.GetItemsByList(list.Id).ToList();
                    // sort items by priority ASC
                    TodoItem temp;
                    for (int i = 0; i <= items.Count-1; i++)  
                    {
                        for (int j = i+1; j < items.Count; j++)  
                        {  
                            // if item i has larger priority move after j
                            // priority 0 is HIGHEST
                            if (items[i].Priority > items[j].Priority)  
                            {
                                temp = items[i];
                                items[i] = items[j];  
                                items[j] = temp;  
                            }
                        }  
                    }
                    list.Items = items.ToList();
                }

                return lists;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}