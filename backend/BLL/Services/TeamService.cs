using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Services.Abstract;
using Common.DTOs.FunctionalModels;
using Common.DTOs.Team;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class TeamService : BaseService, ITeamService
    {
        private readonly IRepository<Team> repository;

        public TeamService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            this.repository = unitOfWork.Set<Team>();
        }

        public async Task<ICollection<TeamDTO>> GetAllTeams()
        {
            var teams = (await repository.GetAll())
                .Include(t => t.Participants);

            return mapper.Map<ICollection<TeamDTO>>(teams);
        }

        public async Task<TeamDTO> GetTeamById(int id)
        {
            var teamEntity = await repository.GetById(id);
            if (teamEntity == null)
            {
                throw new NotFoundException(nameof(Team), id);
            }

            return (await GetAllTeams()).FirstOrDefault(t => t.Id == id);
        }

        public async Task<TeamDTO> CreateTeam(TeamCreateDTO teamDto)
        {
            var teamEntity = mapper.Map<Team>(teamDto);
            teamEntity.CreatedAt = DateTime.Now;

            await repository.Create(teamEntity);
            await unitOfWork.SaveChangesAsync();

            return (await GetAllTeams()).FirstOrDefault(p => p.Id == teamEntity.Id);
        }

        public async Task<TeamDTO> UpdateTeam(TeamUpdateDTO teamDto)
        {
            var teamEntity = await repository.GetById(teamDto.Id);
            if (teamEntity == null)
            {
                throw new NotFoundException(nameof(Team), teamDto.Id);
            }

            teamEntity.Name = teamDto.Name;

            await repository.Update(teamEntity);
            await unitOfWork.SaveChangesAsync();

            return (await GetAllTeams()).FirstOrDefault(p => p.Id == teamEntity.Id);
        }

        public async Task DeleteTeamById(int id)
        {
            var teamEntity = await repository.GetById(id);
            if (teamEntity == null)
            {
                throw new NotFoundException(nameof(Team), id);
            }

            await repository.DeleteById(id);
            await unitOfWork.SaveChangesAsync();
        }


        public async Task<IEnumerable<Team_UsersDTO>> GetTeamsUsersSortedByRegisteredDateAsync()
        {
            var teams = (await GetAllTeams()).Select(team => new Team_UsersDTO()
            {
                Id = team.Id,
                Name = team.Name,
                Users = team.Participants
                    .OrderByDescending(us => us.RegisteredAt)
                    .ToList()
            })
                .Where(id => id.Users
                .All(user => DateTime.Now.Year - user.Birthday.Year > 10) &&
                                        id.Users.Count() != 0);
            return teams;
        }
        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
