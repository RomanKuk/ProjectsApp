using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.MappingProfiles;
using BLL.Services;
using Bogus;
using Common.DTOs.Project;
using Common.DTOs.TaskModel;
using Common.DTOs.TaskState;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Services;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BLL.Tests
{
    public class TaskServiceTests
    {
        private readonly ITaskService taskService;
        private readonly IProjectService projectService;
        private readonly ITeamService teamService;
        private readonly IUserService userService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ModelsContext context;

        public TaskServiceTests()
        {
            teamService = A.Fake<ITeamService>();
            var profiles = new List<Profile>()
            {
                new ProjectProfile(),
                new UserProfile(),
                new TeamProfile(),
                new TaskProfile()
            };
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(profiles));
            mapper = new Mapper(configuration);

            var dbContextOptions =
                new DbContextOptionsBuilder<ModelsContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            context = new ModelsContext(dbContextOptions.Options);

            unitOfWork = new UnitOfWork(context);

            userService = new UserService(unitOfWork, mapper, teamService);
            projectService = new ProjectService(unitOfWork, mapper, teamService, userService);
            taskService = new TaskService(unitOfWork, mapper, projectService, userService);
        }

        [Fact]
        public async Task GetTasksByUserIdWithNameConditionAsync_WhenTaskCountInCollection6_ThenTasksCount6()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var project = new Project
            {
                AuthorId = user.Id,
                TeamId = team.Id
            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var taskFake = new Faker<TaskModelCreateDTO>()
                .RuleFor(c => c.Name, f => f.Lorem.Word())
                .RuleFor(c => c.Description, f => f.Lorem.Word())
                .RuleFor(c => c.ProjectId, f => f.PickRandom(project).Id)
                .RuleFor(c => c.PerformerId, f => f.PickRandom(user).Id)
                .RuleFor(pi => pi.FinishedAt, _ => DateTime.Now + new TimeSpan(1, 0, 0, 0));

            var tasks = taskFake.Generate(10);
            StringBuilder name = new StringBuilder();
            for (int i = 0; i < 46; i++)
            {
                name.Append("o");
            }
            tasks[0].Name = name.ToString();
            tasks[4].Name = name.ToString();
            tasks[6].Name = name.ToString();
            tasks[9].Name = name.ToString();

            foreach (var task in tasks)
                await taskService.CreateTask(task);

            var result = await taskService.GetTasksByUserIdWithNameConditionAsync(user.Id);
            Assert.Equal(6, result.Count());
        }

        [Fact]
        public async Task GetTasksByUserFinishedThisYear_WhenTaskCountInCollection1_ThenTasksCount1()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var project = new Project
            {
                AuthorId = user.Id,
                TeamId = team.Id
            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var taskFake = new Faker<TaskModelCreateDTO>()
                .RuleFor(c => c.Name, f => f.Lorem.Word())
                .RuleFor(c => c.Description, f => f.Lorem.Word())
                .RuleFor(c => c.ProjectId, f => f.PickRandom(project).Id)
                .RuleFor(c => c.PerformerId, f => f.PickRandom(user).Id)
                .RuleFor(pi => pi.FinishedAt, _ => DateTime.Now + new TimeSpan(1, 0, 0, 0));

            var tasks = taskFake.Generate(5);

            foreach (var task in tasks)
                await taskService.CreateTask(task);

            var createdTask = await taskService.GetTaskById(1);
            var updatedTask = new TaskModelUpdateDTO
            {
                Id = createdTask.Id,
                FinishedAt = DateTime.Now,
                State = TaskStateDTO.Finished,
                PerformerId = createdTask.Performer.Id,
                ProjectId = createdTask.ProjectId
            };
            await taskService.UpdateTask(updatedTask);

            var result = await taskService.GetTasksByUserFinishedThisYear(user.Id);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetTasksByUserFinishedThisYear_WhenUserisNotATaskPerformer_ThenEmpty()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now, TeamId = team.Id };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var result = await taskService.GetTasksByUserFinishedThisYear(user.Id);
            Assert.Empty(result);
        }

        [Fact]
        public async Task UpdateTask_WhenUpdateTaskStateToFinished_ThenStateFinished()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var project = new Project
            {
                AuthorId = user.Id,
                TeamId = team.Id
            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var taskFake = new Faker<TaskModelCreateDTO>()
                .RuleFor(c => c.Name, f => f.Lorem.Word())
                .RuleFor(c => c.Description, f => f.Lorem.Word())
                .RuleFor(c => c.ProjectId, f => f.PickRandom(project).Id)
                .RuleFor(c => c.PerformerId, f => f.PickRandom(user).Id)
                .RuleFor(pi => pi.FinishedAt, _ => DateTime.Now + new TimeSpan(1, 0, 0, 0));

           await taskService.CreateTask(taskFake);

            var createdTask = await taskService.GetTaskById(1);
            var updatedTask = new TaskModelUpdateDTO
            {
                Id = createdTask.Id,
                FinishedAt = DateTime.Now,
                State = TaskStateDTO.Finished,
                PerformerId = createdTask.Performer.Id,
                ProjectId = createdTask.ProjectId
            };
            await taskService.UpdateTask(updatedTask);

            var result = await taskService.GetTaskById(1);
            Assert.Equal(TaskStateDTO.Finished, result.State);
        }

        [Fact]
        public async Task UpdateTask_WhenUpdateTaskStateToUnrealState_ThrowArgumentException()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var project = new Project
            {
                AuthorId = user.Id,
                TeamId = team.Id
            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var taskFake = new Faker<TaskModelCreateDTO>()
                .RuleFor(c => c.Name, f => f.Lorem.Word())
                .RuleFor(c => c.Description, f => f.Lorem.Word())
                .RuleFor(c => c.ProjectId, f => f.PickRandom(project).Id)
                .RuleFor(c => c.PerformerId, f => f.PickRandom(user).Id)
                .RuleFor(pi => pi.FinishedAt, _ => DateTime.Now + new TimeSpan(1, 0, 0, 0));

            await taskService.CreateTask(taskFake);

            var createdTask = await taskService.GetTaskById(1);
            var updatedTask = new TaskModelUpdateDTO
            {
                Id = createdTask.Id,
                FinishedAt = DateTime.Now,
                State = TaskStateDTO.Canceled+1,
                PerformerId = createdTask.Performer.Id,
                ProjectId = createdTask.ProjectId
            };

            await Assert.ThrowsAsync<ArgumentException>(() => taskService.UpdateTask(updatedTask));
        }

        [Fact]
        public async Task GetUsersSortedByFirstNameAndSortedTasksAsync_WhenLongestTaskOfUserId_ThenFirstTaskInTasks()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var project = new Project
            {
                AuthorId = user.Id,
                TeamId = team.Id
            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var taskFake = new Faker<TaskModelCreateDTO>()
                .RuleFor(c => c.Name, f => f.Lorem.Sentences())
                .RuleFor(c => c.Description, f => f.Lorem.Word())
                .RuleFor(c => c.ProjectId, f => f.PickRandom(project).Id)
                .RuleFor(c => c.PerformerId, f => f.PickRandom(user).Id)
                .RuleFor(pi => pi.FinishedAt, _ => DateTime.Now + new TimeSpan(1, 0, 0, 0));

            var tasks = taskFake.Generate(10);

            foreach (var task in tasks)
                await taskService.CreateTask(task);

            var check = (await taskService.GetAllTasks())
                .Where(task => task.Performer.Id == user.Id)
                .OrderByDescending(task => task.Name.Length).FirstOrDefault();

            var result = (await taskService.GetUsersSortedByFirstNameAndSortedTasksAsync()).FirstOrDefault();
            Assert.Equal(check.Id, result.Tasks.FirstOrDefault()?.Id);
        }

        [Fact]
        public async Task CreateNewUserStructureAsync_WhenTwoProject_ThenReturnLast()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var firstProjectCreate = A.Fake<ProjectCreateDTO>();
            firstProjectCreate.AuthorId = user.Id;
            firstProjectCreate.TeamId = team.Id;
            firstProjectCreate.Deadline = DateTime.Now + new TimeSpan(1, 0, 0, 0);
            _ = await projectService.CreateProject(firstProjectCreate);

            var lastProjectCreate = A.Fake<ProjectCreateDTO>();
            lastProjectCreate.AuthorId = user.Id;
            lastProjectCreate.TeamId = team.Id;
            lastProjectCreate.Deadline = DateTime.Now + new TimeSpan(1, 0, 0, 0);

            var lastProject = await projectService.CreateProject(firstProjectCreate);

            var result = (await taskService.CreateNewUserStructureAsync(user.Id)).FirstOrDefault();
            Assert.Equal(lastProject.Id, result.LastProject.Id);
        }

        [Fact]
        public async Task CreateNewUserStructureAsync_WhenTwoProject_ThenReturnLastProjectTasksCount()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var firstProjectCreate = A.Fake<ProjectCreateDTO>();
            firstProjectCreate.AuthorId = user.Id;
            firstProjectCreate.TeamId = team.Id;
            firstProjectCreate.Deadline = DateTime.Now + new TimeSpan(1, 0, 0, 0);

            var firstProject = await projectService.CreateProject(firstProjectCreate);

            var lastProjectCreate = A.Fake<ProjectCreateDTO>();
            lastProjectCreate.AuthorId = user.Id;
            lastProjectCreate.TeamId = team.Id;
            lastProjectCreate.Deadline = DateTime.Now + new TimeSpan(1, 0, 0, 0);

            var lastProject = await projectService.CreateProject(firstProjectCreate);

            var projects = await projectService.GetAllProjects();

            var taskFake = new Faker<TaskModelCreateDTO>()
                .RuleFor(c => c.Name, f => f.Lorem.Sentences())
                .RuleFor(c => c.Description, f => f.Lorem.Word())
                .RuleFor(c => c.ProjectId, f => f.PickRandom(projects).Id)
                .RuleFor(c => c.PerformerId, f => f.PickRandom(user).Id)
                .RuleFor(pi => pi.FinishedAt, _ => DateTime.Now + new TimeSpan(1, 0, 0, 0));

            var tasks = taskFake.Generate(10);

            foreach (var task in tasks)
                await taskService.CreateTask(task);

            var check = (await projectService.GetAllProjects())
                .OrderByDescending(p => p.CreatedAt).FirstOrDefault()?.Tasks.Count;

            var result = (await taskService.CreateNewUserStructureAsync(user.Id)).FirstOrDefault();
            Assert.Equal(check, result.LastProject.Tasks.Count);
        }

        [Fact]
        public async Task CreateNewUserStructureAsync_WhenOneUserHas10Tasks_ThenLongestTask()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var firstProjectCreate = A.Fake<ProjectCreateDTO>();
            firstProjectCreate.AuthorId = user.Id;
            firstProjectCreate.TeamId = team.Id;
            firstProjectCreate.Deadline = DateTime.Now + new TimeSpan(1, 0, 0, 0);

            var firstProject = await projectService.CreateProject(firstProjectCreate);

            var taskFake = new Faker<TaskModelCreateDTO>()
                .RuleFor(c => c.Name, f => f.Lorem.Sentences())
                .RuleFor(c => c.Description, f => f.Lorem.Word())
                .RuleFor(c => c.ProjectId, f => f.PickRandom(firstProject).Id)
                .RuleFor(c => c.PerformerId, f => f.PickRandom(user).Id)
                .RuleFor(pi => pi.FinishedAt, f => f.Date.Future(5));

            var tasks = taskFake.Generate(10);

            foreach (var task in tasks)
                await taskService.CreateTask(task);

            var check = (await taskService.GetAllTasks())
                .Where(t => t.Performer.Id == user.Id)
                .OrderByDescending(task => task.FinishedAt - task.CreatedAt)
                .FirstOrDefault();

            var result = (await taskService.CreateNewUserStructureAsync(user.Id)).FirstOrDefault();
            Assert.Equal(check.Id, result.LongestTask.Id);
        }

        [Fact]
        public async Task CreateNewUserStructureAsync_WhenUserisNotATaskPerformer_ThenEmpty()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now, TeamId = team.Id };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var result = await taskService.GetTasksByUserFinishedThisYear(user.Id);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUnfinishedTasksByUser_WhenUserisNotExist_ThenThrowNotFoundException()
        {
            await Assert.ThrowsAsync<NotFoundException>(() => taskService.GetUnfinishedTasksByUser(0));
        }

        [Fact]
        public async Task GetUnfinishedTasksByUser_WhenUserHas5UnfinishedTasks_ThenTasksCount5()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var firstProjectCreate = A.Fake<ProjectCreateDTO>();
            firstProjectCreate.AuthorId = user.Id;
            firstProjectCreate.TeamId = team.Id;
            firstProjectCreate.Deadline = DateTime.Now + new TimeSpan(1, 0, 0, 0);

            var firstProject = await projectService.CreateProject(firstProjectCreate);

            var taskFake = new Faker<TaskModelCreateDTO>()
                .RuleFor(c => c.Name, f => f.Lorem.Sentences())
                .RuleFor(c => c.Description, f => f.Lorem.Word())
                .RuleFor(c => c.ProjectId, f => f.PickRandom(firstProject).Id)
                .RuleFor(c => c.PerformerId, f => f.PickRandom(user).Id)
                .RuleFor(pi => pi.FinishedAt, f => f.Date.Future(5));

            var tasks = taskFake.Generate(5);

            foreach (var task in tasks)
                await taskService.CreateTask(task);

            var result = (await taskService.GetUnfinishedTasksByUser(user.Id));
            Assert.Equal(5, result.Count());
        }
    }
}
