using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Services;
using Common.DTOs.FunctionalModels;
using Common.DTOs.Team;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService teamService;

        public TeamsController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<TeamDTO>>> Get()
        {
            return Ok(await teamService.GetAllTeams());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDTO>> GetById(int id)
        {
            return Ok(await teamService.GetTeamById(id));
        }

        [HttpPost]
        public async Task<ActionResult<TeamDTO>> Create([FromBody] TeamCreateDTO dto)
        {
            var team = await teamService.CreateTeam(dto);
            return Created($"api/Teams/{team.Id}", team);
        }

        [HttpPut]
        public async Task<ActionResult<TeamDTO>> Put([FromBody] TeamUpdateDTO team)
        {
            return Ok(await teamService.UpdateTeam(team));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            await teamService.DeleteTeamById(id);
            return NoContent();
        }

        [HttpGet("team-users-sorted-by-registered-date")]
        public async Task<ActionResult<IEnumerable<Team_UsersDTO>>> GetTeamsUsersSortedByRegisteredDate()
        {
            return Ok(await teamService.GetTeamsUsersSortedByRegisteredDateAsync());
        }
    }
}
