using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Services;
using Common.DTOs.FunctionalModels;
using Common.DTOs.TaskModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService taskService;

        public TasksController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<TaskModelDTO>>> Get()
        {
            return Ok(await taskService.GetAllTasks());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModelDTO>> GetById(int id)
        {
            return Ok(await taskService.GetTaskById(id));
        }

        [HttpPost]
        public async Task<ActionResult<TaskModelDTO>> Create([FromBody] TaskModelCreateDTO dto)
        {
            var task = await taskService.CreateTask(dto);
            return Created($"api/Tasks/{task.Id}", task);
        }

        [HttpPut]
        public async Task<ActionResult<TaskModelDTO>> Put([FromBody] TaskModelUpdateDTO team)
        {
            return Ok(await taskService.UpdateTask(team));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            await taskService.DeleteTaskById(id);
            return NoContent();
        }

        [HttpGet("by-user-with-name-condition/{id}")]
        public async Task<ActionResult<IEnumerable<TaskModelDTO>>> GetTasksByUserIdWithNameCondition(int id)
        {
            return Ok(await taskService.GetTasksByUserIdWithNameConditionAsync(id));
        }

        [HttpGet("by-user-finished-this-year/{id}")]
        public async Task<ActionResult<IEnumerable<TaskFinishedThisYearDTO>>> GetTasksByUserFinishedThisYear(int id)
        {
            return Ok(await taskService.GetTasksByUserFinishedThisYear(id));
        }

        [HttpGet("users-sorted-by-first-name-sorted-tasks")]
        public async Task<ActionResult<IEnumerable<User_TasksDTO>>> GetUsersSortedByFirstNameAndSortedTasks()
        {
            return Ok(await taskService.GetUsersSortedByFirstNameAndSortedTasksAsync());
        }

        [HttpGet("structure-with-tasks-by-user/{id}")]
        public async Task<ActionResult<IEnumerable<UserLastProjectTasksDTO>>> GetNewUserStructure(int id)
        {
            return Ok(await taskService.CreateNewUserStructureAsync(id));
        }

        [HttpGet("unfinished-tasks-by-user/{id}")]
        public async Task<ActionResult<IEnumerable<TaskModelDTO>>> GetUnfinishedTasksByUser(int id)
        {
            return Ok(await taskService.GetUnfinishedTasksByUser(id));
        }

        [HttpGet("unfinished-tasks")]
        public async Task<ActionResult<IEnumerable<TaskModelDTO>>> GetUnfinishedTasks()
        {
            return Ok(await taskService.GetUnfinishedTasks());
        }
    }
}
