using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Services;
using Common.Converters;
using Common.DTOs;
using Common.DTOs.FunctionalModels;
using Common.DTOs.Project;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService projectService;

        public ProjectsController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<ProjectDTO>>> Get()
        {
            return Ok(await projectService.GetAllProjects());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDTO>> GetById(int id)
        {
            return Ok(await projectService.GetProjectById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> Create([FromBody] ProjectCreateDTO dto)
        {
            var project = await projectService.CreateProject(dto);
            return Created($"api/Projects/{project.Id}", project);
        }

        [HttpPut]
        public async Task<ActionResult<ProjectDTO>> Put([FromBody] ProjectUpdateDTO team)
        {
            return Ok(await projectService.UpdateProject(team));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            await projectService.DeleteProjectById(id);
            return NoContent();
        }

        [HttpGet("project-tasks-count-by-user/{id}")]
        public async Task<ActionResult<Dictionary<ProjectDTO, int>>> GetUserProjectTasksCountByUserId(int id)
        {
            var result = await projectService.GetUserProjectTasksCountByUserId(id);

            var json = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                ContractResolver = new DictionaryArrayConverter()
            });
            return Ok(json);
        }

        [HttpGet("new-project-structure")]
        public async Task<ActionResult<IEnumerable<Project_TasksDTO>>> GetNewProjectStructure()
        {
            return Ok(await projectService.CreateNewProjectStructureAsync());
        }
    }
}
