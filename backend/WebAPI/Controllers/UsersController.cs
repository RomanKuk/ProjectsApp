using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Services;
using Common.DTOs.FunctionalModels;
using Common.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<UserDTO>>> Get()
        {
            return Ok(await userService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            return Ok(await userService.GetUserById(id));
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Create([FromBody] UserCreateDTO dto)
        {
            var user = await userService.CreateUser(dto);
            return Created($"api/Users/{user.Id}", user);
        }

        [HttpPut]
        public async Task<ActionResult<UserDTO>> Put([FromBody] UserUpdateDTO team)
        {
            return Ok(await userService.UpdateUser(team));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            await userService.DeleteUserById(id);
            return NoContent();
        }
    }
}
