using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Services.Abstract;
using Common.DTOs.FunctionalModels;
using Common.DTOs.TaskModel;
using Common.DTOs.TaskState;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class TaskService : BaseService, ITaskService
    {
        private readonly IRepository<TaskModel> repository;
        private readonly IProjectService projectService;
        private readonly IUserService userService;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper,
            IProjectService projectService, IUserService userService) : base(unitOfWork, mapper)
        {
            this.repository = unitOfWork.Set<TaskModel>();
            this.projectService = projectService;
            this.userService = userService;
        }

        public async Task<ICollection<TaskModelDTO>> GetAllTasks()
        {
            var tasks = (await repository.GetAll())
                    .Include(t => t.Performer)
                        .ThenInclude(p => p.Team);

            return mapper.Map<ICollection<TaskModelDTO>>(tasks);
        }

        public async Task<TaskModelDTO> GetTaskById(int id)
        {
            var taskCheck = await repository.GetById(id);
            if (taskCheck == null)
            {
                throw new NotFoundException(nameof(TaskModel), id);
            }

            return (await GetAllTasks()).FirstOrDefault(t => t.Id == id);
        }

        public async Task<TaskModelDTO> CreateTask(TaskModelCreateDTO taskDto)
        {
            var taskEntity = mapper.Map<TaskModel>(taskDto);
            taskEntity.CreatedAt = DateTime.Now;
            taskEntity.State = TaskState.Created;

            if (taskEntity.FinishedAt <= taskEntity.CreatedAt)
                throw new ArgumentException("Deadline of a created task must be later then created date");

            await projectService.GetProjectById(taskEntity.ProjectId);
            await userService.GetUserById(taskEntity.PerformerId);

            await repository.Create(taskEntity);
            await unitOfWork.SaveChangesAsync();

            return (await GetAllTasks()).FirstOrDefault(t => t.Id == taskEntity.Id);
        }

        public async Task<TaskModelDTO> UpdateTask(TaskModelUpdateDTO taskDto)
        {
            var taskEntity = await repository.GetById(taskDto.Id);
            if (taskEntity == null)
            {
                throw new NotFoundException(nameof(TaskModel), taskDto.Id);
            }

            if (taskEntity.FinishedAt <= taskEntity.CreatedAt)
                throw new ArgumentException("Deadline of a created task must be later then created date");

            await projectService.GetProjectById(taskDto.ProjectId);
            await userService.GetUserById(taskDto.PerformerId);
            if (!Enum.IsDefined(typeof(TaskStateDTO), taskDto.State))
                throw new ArgumentException("Task state is incorrect");

            taskEntity.Name = taskDto.Name;
            taskEntity.Description = taskDto.Description;
            taskEntity.FinishedAt = taskDto.FinishedAt;
            taskEntity.State = (TaskState)taskDto.State;
            taskEntity.PerformerId = taskDto.PerformerId;
            taskEntity.ProjectId = taskDto.ProjectId;

            await repository.Update(taskEntity);
            await unitOfWork.SaveChangesAsync();

            return (await GetAllTasks()).FirstOrDefault(t => t.Id == taskEntity.Id);
        }

        public async Task DeleteTaskById(int id)
        {
            var taskEntity = await repository.GetById(id);
            if (taskEntity == null)
            {
                throw new NotFoundException(nameof(TaskModel), id);
            }

            await repository.DeleteById(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskModelDTO>> GetTasksByUserIdWithNameConditionAsync(int userId)
        {
            await userService.GetUserById(userId);
            var tasks = (await GetAllTasks())
                    .Where(task => task.Performer.Id == userId && task.Name.Length < 45);

            return tasks;
        }

        public async Task<IEnumerable<TaskFinishedThisYearDTO>> GetTasksByUserFinishedThisYear(int userId)
        {
            await userService.GetUserById(userId);
            var tasks = (await GetAllTasks())
                    .Where(task => task.Performer.Id == userId &&
                        task.FinishedAt.Year == DateTime.Now.Year/*2020*/ &&
                        task.State == TaskStateDTO.Finished)
                    .Select(task => new TaskFinishedThisYearDTO { Id = task.Id, Name = task.Name });

            return tasks;
        }

        public async Task<IEnumerable<User_TasksDTO>> GetUsersSortedByFirstNameAndSortedTasksAsync()
        {
            return (await userService.GetAllUsers())
                .GroupJoin(await GetAllTasks(),
                    user => user.Id,
                    task => task.Performer.Id,
                    (u, t) => new User_TasksDTO
                    {
                        User = u,
                        Tasks = t.OrderByDescending(task => task.Name.Length).ToList()
                    })
                .OrderBy(id => id.User.FirstName);
        }

        public async Task<IEnumerable<UserLastProjectTasksDTO>> CreateNewUserStructureAsync(int userId)
        {
            await userService.GetUserById(userId);
            var users = await userService.GetAllUsers();
            var tasks = await GetAllTasks();
            var structure = users
                .Where(u => u.Id == userId)
                .GroupJoin(await projectService.GetAllProjects(),
                    user => user.Id,
                    project => project.Author.Id,
                    (us, pr) => new UserLastProjectTasksDTO
                    {
                        User = us,
                        LastProject = pr
                            .OrderByDescending(p => p.CreatedAt)
                            .FirstOrDefault(),
                        LastProjectTasksCount = pr
                            .OrderByDescending(p => p.CreatedAt)
                            .FirstOrDefault()?
                            .Tasks.Count,
                        UndoneOrCanceledTasksCount = users
                            .Where(user => user.Id == userId)
                            .GroupJoin(tasks,
                                user => user.Id,
                                task => task.Performer.Id,
                                (_, t) => t.Count(task => task.State != TaskStateDTO.Finished))
                            .FirstOrDefault(),
                        LongestTask = users
                            .Where(user => user.Id == userId)
                            .GroupJoin(tasks,
                                user => user.Id,
                                task => task.Performer.Id,
                                (_, t) => t.OrderByDescending(task => task.FinishedAt - task.CreatedAt).FirstOrDefault())
                            .FirstOrDefault(),
                    });

            return structure;
        }

        public async Task<IEnumerable<TaskModelDTO>> GetUnfinishedTasksByUser(int userId)
        {
            await userService.GetUserById(userId);
            var tasks = (await GetAllTasks())
                .Where(task => task.Performer.Id == userId && task.State != TaskStateDTO.Finished);

            return tasks;
        }

        public async Task<IEnumerable<TaskModelDTO>> GetUnfinishedTasks()
        {
            var tasks = (await GetAllTasks())
                .Where(task => task.State != TaskStateDTO.Finished);

            return tasks;
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
            projectService.Dispose();
            userService.Dispose();
        }
    }
}
