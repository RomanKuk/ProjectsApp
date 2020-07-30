using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Services.Abstract;
using Common.DTOs.FunctionalModels;
using Common.DTOs.Team;
using Common.DTOs.User;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IRepository<User> repository;
        private readonly ITeamService teamService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ITeamService teamService) : base(unitOfWork, mapper)
        {
            this.repository = unitOfWork.Set<User>();
            this.teamService = teamService;
        }

        public async Task<ICollection<UserDTO>> GetAllUsers()
        {
            var users = (await repository.GetAll())
                .Include(u => u.Team);

            return mapper.Map<ICollection<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var userEntity = await repository.GetById(id);
            if (userEntity == null)
            {
                throw new NotFoundException(nameof(User), id);
            }

            return (await GetAllUsers()).FirstOrDefault(u => u.Id == id);
        }

        public async Task<UserDTO> CreateUser(UserCreateDTO userDto)
        {
            var userEntity = mapper.Map<User>(userDto);
            userEntity.RegisteredAt = DateTime.Now;
            if (userEntity.TeamId != null && userEntity.TeamId != 0)
            {
                await teamService.GetTeamById((int)userEntity.TeamId);
            }
            else
            {
                userEntity.TeamId = null;
            }

            await repository.Create(userEntity);
            await unitOfWork.SaveChangesAsync();

            var createdUser = mapper.Map<UserDTO>(userEntity);

            return createdUser;
            //return (await GetAllUsers()).FirstOrDefault(u => u.Id == userEntity.Id);
        }

        public async Task<UserDTO> UpdateUser(UserUpdateDTO userDto)
        {
            var userEntity = await repository.GetById(userDto.Id);
            if (userEntity == null)
            {
                throw new NotFoundException(nameof(User), userDto.Id);
            }

            if (userEntity.TeamId != null && userEntity.TeamId != 0)
            {
                await teamService.GetTeamById((int)userEntity.TeamId);
            }
            else
            {
                userEntity.TeamId = null;
            }

            userEntity.FirstName = userDto.FirstName;
            userEntity.LastName = userDto.LastName;
            userEntity.Email = userDto.Email;
            userEntity.Birthday = userDto.Birthday;
            userEntity.TeamId = userDto.TeamId;

            await repository.Update(userEntity);
            await unitOfWork.SaveChangesAsync();

            return (await GetAllUsers()).FirstOrDefault(u => u.Id == userEntity.Id);
        }

        public async Task DeleteUserById(int id)
        {
            var userEntity = await repository.GetById(id);
            if (userEntity == null)
            {
                throw new NotFoundException(nameof(User), id);
            }

            await repository.DeleteById(id);
            await unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
            teamService.Dispose();
        }
    }
}
