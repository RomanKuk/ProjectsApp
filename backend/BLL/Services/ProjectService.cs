using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Services.Abstract;
using Common.DTOs.FunctionalModels;
using Common.DTOs.Project;
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
    public class ProjectService : BaseService, IProjectService
    {
        private readonly IRepository<Project> repository;
        private readonly ITeamService teamService;
        private readonly IUserService userService;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper,
            ITeamService teamService, IUserService userService) : base(unitOfWork, mapper)
        {
            this.repository = unitOfWork.Set<Project>();
            this.teamService = teamService;
            this.userService = userService;
        }

        public async Task<ICollection<ProjectDTO>> GetAllProjects()
        {
            var projects = (await repository.GetAll())
                    .Include(p => p.Author)
                        .ThenInclude(a => a.Team)
                    .Include(p => p.Team)
                    .Include(p => p.Tasks)
                        .ThenInclude(t => t.Performer)
                        .ThenInclude(p => p.Team);

            return mapper.Map<ICollection<ProjectDTO>>(projects);
        }

        public async Task<ProjectDTO> GetProjectById(int id)
        {
            var projectCheck = await repository.GetById(id);
            if (projectCheck == null)
            {
                throw new NotFoundException(nameof(Project), id);
            }

            return (await GetAllProjects()).FirstOrDefault(p => p.Id == id);
        }

        public async Task<ProjectDTO> CreateProject(ProjectCreateDTO projectDto)
        {
            var projectEntity = mapper.Map<Project>(projectDto);
            projectEntity.CreatedAt = DateTime.Now;

            await userService.GetUserById(projectEntity.AuthorId);
            await teamService.GetTeamById(projectEntity.TeamId);

            if (projectEntity.Deadline <= projectEntity.CreatedAt)
                throw new ArgumentException("Deadline of a created project must be later then created date");
            
            await repository.Create(projectEntity);
            await unitOfWork.SaveChangesAsync();

            return (await GetAllProjects()).FirstOrDefault(p => p.Id == projectEntity.Id);
        }

        public async Task<ProjectDTO> UpdateProject(ProjectUpdateDTO projectDto)
        {
            var projectEntity = await repository.GetById(projectDto.Id);

            if (projectEntity == null)
            {
                throw new NotFoundException(nameof(Project), projectDto.Id);
            }

            if (projectEntity.Deadline <= projectEntity.CreatedAt)
                throw new ArgumentException("Deadline of a created project must be later then created date");

            await teamService.GetTeamById(projectDto.TeamId);
            await userService.GetUserById(projectDto.AuthorId);

            projectEntity.Name = projectDto.Name;
            projectEntity.Description = projectDto.Description;
            projectEntity.Deadline = projectDto.Deadline;
            projectEntity.AuthorId = projectDto.AuthorId;
            projectEntity.TeamId = projectDto.TeamId;

            await repository.Update(projectEntity);
            await unitOfWork.SaveChangesAsync();

            return (await GetAllProjects()).FirstOrDefault(p => p.Id == projectEntity.Id);
        }

        public async Task DeleteProjectById(int id)
        {
            var projectEntity = await repository.GetById(id);
            if (projectEntity == null)
            {
                throw new NotFoundException(nameof(Project), id);
            }

            await repository.DeleteById(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<Dictionary<ProjectDTO, int>> GetUserProjectTasksCountByUserId(int userId)
        {
            await userService.GetUserById(userId);
            var projects = (await GetAllProjects())
                .Where(pr => pr.Author.Id == userId)
                .GroupBy(pr => pr)
                .ToDictionary(item => item.Key, item => item.Key.Tasks.Count());

            return projects;
        }

        public async Task<IEnumerable<Project_TasksDTO>> CreateNewProjectStructureAsync()
        {
            var structure = (await GetAllProjects()).Select(project => new Project_TasksDTO
            {
                Project = project,
                LongestTaskByDescription = project.Tasks.OrderByDescending(t => t.Description.Length).FirstOrDefault(),
                ShortestTaskByName = project.Tasks.OrderBy(t => t.Name.Length).FirstOrDefault(),
                TotalUsersCount = (project.Description.Length > 20 || project.Tasks.Count() < 3) ?
                    project.Team.Participants.Count() : 0
            });

            return structure;
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
            userService.Dispose();
            teamService.Dispose();
        }
    }
}
