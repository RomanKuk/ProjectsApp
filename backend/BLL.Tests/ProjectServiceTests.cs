using AutoMapper;
using BLL.Interfaces;
using BLL.MappingProfiles;
using BLL.Services;
using Bogus;
using Common.DTOs.Project;
using Common.DTOs.TaskModel;
using Common.DTOs.Team;
using Common.DTOs.User;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Services;
using FakeItEasy;
using FakeItEasy.Sdk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BLL.Tests
{
    public class ProjectServiceTests: IDisposable
    {
        private readonly IProjectService projectService;
        private readonly ITeamService teamService;
        private readonly IUserService userService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ModelsContext context;

        public ProjectServiceTests()
        {
            teamService = A.Fake<ITeamService>();
            userService = A.Fake<IUserService>();

            var profiles = new List<Profile>() 
            { 
                new ProjectProfile(),
                new UserProfile(),
                new TaskProfile(),
                new TeamProfile()

            };
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(profiles));
            mapper = new Mapper(configuration);

            var dbContextOptions = 
                new DbContextOptionsBuilder<ModelsContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            context = new ModelsContext(dbContextOptions.Options);
            
            unitOfWork = new UnitOfWork(context);

            projectService = new ProjectService(unitOfWork, mapper, teamService, userService);
        }

        [Fact]
        public async Task GetUserProjectTasksCountByUserId_WhenUserProjectHas5Tasks_ThenTasksCountIs5()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now, TeamId = team.Id };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var projectCreate = new ProjectCreateDTO
            {
                Name = "A",
                Description = "BBB",
                Deadline = DateTime.Now + new TimeSpan(1, 0, 0, 0),
                AuthorId = user.Id,
                TeamId = team.Id
            };

            var project = await projectService.CreateProject(projectCreate);

            var taskId = 201;

            var taskFake = new Faker<TaskModel>()
                .RuleFor(c => c.Id, f => taskId++)
                .RuleFor(c => c.Name, f => f.Lorem.Sentence())
                .RuleFor(c => c.Description, f => f.Lorem.Sentences())
                .RuleFor(c => c.ProjectId, f => f.PickRandom(project).Id)
                .RuleFor(c => c.PerformerId, f => f.PickRandom(user).Id)
                .RuleFor(c => c.State, f => f.Random.Enum<TaskState>())
                .RuleFor(pi => pi.CreatedAt, f => f.Date.Between(new DateTime(2015, 1, 1), new DateTime(2019, 1, 1)))
                .RuleFor(pi => pi.FinishedAt, f => f.Date.Future(5, new DateTime(2019, 1, 1)));

            var tasks = taskFake.Generate(5);
            context.Tasks.AddRange(tasks);
            await context.SaveChangesAsync();

            var result = await projectService.GetUserProjectTasksCountByUserId(user.Id);
            Assert.Equal(5, result.Values.ElementAt(0));
        }

        [Fact]
        public async Task GetUserProjectTasksCountByUserId_WhenUserisNotAProjectAuthor_ThenEmpty()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now, TeamId = team.Id };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var result = await projectService.GetUserProjectTasksCountByUserId(user.Id);
            Assert.Empty(result);
        }

        [Fact]
        public async Task CreateNewProjectStructureAsync_WhenOneProjectHasDescriptionLengthGreaterThan20AndUsersInTeam3_ThenUsersCount3()
        {

            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();
            for (int i = 0; i < 3; i++)
            {
                var user = new User { Id = i + 1, Birthday = DateTime.Now, TeamId = team.Id };
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            StringBuilder desc = new StringBuilder();
            for (int i = 0; i < 21; i++)
            {
                desc.Append("o");
            }

            var projectCreate = new ProjectCreateDTO
            {
                Name = "A",
                Description = "BBB",
                Deadline = DateTime.Now + new TimeSpan(1, 0, 0, 0),
                AuthorId = context.Users.FirstOrDefault().Id,
                TeamId = team.Id
            };

            var project = await projectService.CreateProject(projectCreate);

            var result = (await projectService.CreateNewProjectStructureAsync()).FirstOrDefault();
            Assert.Equal(3, result.TotalUsersCount);
        }

        [Fact]
        public async Task CreateNewProjectStructureAsync_WhenOneProjectHasLongestTaskByDescriptionId3_ThenLongesttaskId3()
        {

            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now, TeamId = team.Id };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var projectCreate = new ProjectCreateDTO
            {
                Name = "A",
                Description = "BBB",
                Deadline = DateTime.Now + new TimeSpan(1, 0, 0, 0),
                AuthorId = user.Id,
                TeamId = team.Id
            };

            var project = await projectService.CreateProject(projectCreate);

            var taskId = 1;

            var taskFake = new Faker<TaskModel>()
                .RuleFor(c => c.Id, f => taskId++)
                .RuleFor(c => c.Name, f => f.Lorem.Sentence())
                .RuleFor(c => c.Description, f => f.Lorem.Word())
                .RuleFor(c => c.ProjectId, f => f.PickRandom(project).Id)
                .RuleFor(c => c.PerformerId, f => f.PickRandom(user).Id)
                .RuleFor(c => c.State, f => f.Random.Enum<TaskState>())
                .RuleFor(pi => pi.CreatedAt, f => f.Date.Between(new DateTime(2015, 1, 1), new DateTime(2019, 1, 1)))
                .RuleFor(pi => pi.FinishedAt, f => f.Date.Future(5, new DateTime(2019, 1, 1)));

            var tasks = taskFake.Generate(5);
            context.Tasks.AddRange(tasks);
            await context.SaveChangesAsync();

            var taskId3 = context.Tasks.Find(3);
            taskId3.Description = "00000000000000000000000000000000000000000";
            context.Tasks.Update(taskId3);

            var result = (await projectService.CreateNewProjectStructureAsync()).FirstOrDefault();
            Assert.Equal(taskId3.Id, result.LongestTaskByDescription.Id);
        }

        [Fact]
        public async Task CreateNewProjectStructureAsync_WhenOneProjectHasShortestTaskByNameId4_ThenShortestTaskId4()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = new User { Id = 1, Birthday = DateTime.Now, TeamId = team.Id };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var projectCreate = new ProjectCreateDTO
            {
                Name = "A",
                Description = "BBB",
                Deadline = DateTime.Now + new TimeSpan(1, 0, 0, 0),
                AuthorId = user.Id,
                TeamId = team.Id
            };

            var project = await projectService.CreateProject(projectCreate);

            var taskId = 1;

            var taskFake = new Faker<TaskModel>()
                .RuleFor(c => c.Id, f => taskId++)
                .RuleFor(c => c.Name, f => f.Lorem.Sentence())
                .RuleFor(c => c.Description, f => f.Lorem.Word())
                .RuleFor(c => c.ProjectId, f => f.PickRandom(project).Id)
                .RuleFor(c => c.PerformerId, f => f.PickRandom(user).Id)
                .RuleFor(c => c.State, f => f.Random.Enum<TaskState>())
                .RuleFor(pi => pi.CreatedAt, f => f.Date.Between(new DateTime(2015, 1, 1), new DateTime(2019, 1, 1)))
                .RuleFor(pi => pi.FinishedAt, f => f.Date.Future(5, new DateTime(2019, 1, 1)));

            var tasks = taskFake.Generate(5);
            context.Tasks.AddRange(tasks);
            await context.SaveChangesAsync();

            var taskId4 = context.Tasks.Find(4);
            taskId4.Name = "0";
            context.Tasks.Update(taskId4);

            var result = (await projectService.CreateNewProjectStructureAsync()).FirstOrDefault();
            Assert.Equal(taskId4.Id, result.ShortestTaskByName.Id);
        }


        public void Dispose()
        {
            projectService.Dispose();
            unitOfWork.Dispose();
            context.Dispose();
        }
    }
}
