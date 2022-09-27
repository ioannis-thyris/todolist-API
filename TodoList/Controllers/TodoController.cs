using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using TodoListAPI.Dtos;
using TodoListAPI.Models;
using TodoListAPI.Repositories;

namespace TodoListAPI.Controllers
{
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repo;
        private readonly IMapper _mapper;

        public TodoController(ITodoRepository todoRepository, IMapper mapper)
        {
            _repo = todoRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [Route("api/todos")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var todosDtos = await _repo.GetAll();

                return Ok(todosDtos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to retrieve Todos:\n {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/todos/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (id is null)
                return BadRequest("Id is missing from the request.");

            try
            {
                var todo = await _repo.GetById(id);

                return todo is null ? NotFound($"No Todo in the database with Id: {id}.") : Ok(todo);
            }
            catch (Exception)
            {
                return BadRequest($"Unable to retrieve Todo with Id: {id}.");
            }
        }

        [HttpPost]
        [Route("api/todos")]
        public async Task<IActionResult> Create(TodoDtoCreate todoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Todo object");

            try
            {
                // Assure that dto is a valid Todo
                _mapper.Map<Todo>(todoDto);

                var todoCreated = await _repo.Insert(todoDto);
                
                return CreatedAtAction("GetById", new { id = todoCreated.Id }, todoCreated);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is FormatException)
                    return BadRequest("Bad date/time format.");

                return BadRequest("Create was not successful.");
            }
        }

        [HttpDelete]
        [Route("api/todos/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id is null)
                return BadRequest("Id is missing from the request.");

            var todoForDelete = await _repo.GetById(id);

            if (todoForDelete == null)
                return BadRequest($"No Todo in the database with Id: {id}.");

            try
            {
                await _repo.Delete(id);

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("Delete was not successful.");
            }
        }

        [HttpPut]
        [Route("api/todos/{id}")]
        public async Task<IActionResult> Update(string id, TodoDtoUpdate todoDto)
        {
            if (id is null)
                return BadRequest("Id is missing from the request.");

            if (!ModelState.IsValid)
                return BadRequest("Invalid Todo object");

            var todoForUpate = await _repo.GetById(id);

            if (todoForUpate == null)
                return BadRequest($"No Todo in the database with Id: {id}.");

            try
            {
                // Assure that dto is a valid Todo
                _mapper.Map<Todo>(todoDto);

                var todo = await _repo.Update(id, todoDto);

                return Ok(todoDto);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is FormatException)
                    return BadRequest("Bad date/time format.");

                return BadRequest("Update was not successful.");
            }
        }

    }
}
