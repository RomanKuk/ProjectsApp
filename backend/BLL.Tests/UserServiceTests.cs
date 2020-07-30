using AutoMapper;
using BLL.Interfaces;
using BLL.MappingProfiles;
using BLL.Services;
using Common.DTOs.User;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Services;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BLL.Tests
{
    public class UserServiceTests
    {
        private readonly IUserService userService;
        private readonly ITeamService teamService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ModelsContext context;

        public UserServiceTests()
        {
            userService = A.Fake<IUserService>();
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

            teamService = new TeamService(unitOfWork, mapper);
            userService = new UserService(unitOfWork, mapper, teamService);
        }

        [Fact]
        public async Task CreateUser_WhenNewUser_ThenUsersPlusOne()
        {
            var user = A.Fake<UserCreateDTO>();

            await userService.CreateUser(user);

            Assert.Single(await userService.GetAllUsers());
        }

        [Fact]
        public async Task CreateUser_WhenNewUser_ThenCalled()
        {
            var dbContextOptions =
                new DbContextOptionsBuilder<ModelsContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakeTeamService = A.Fake<ITeamService>();
            var userService = new UserService(fakeUnitOfWork, mapper, fakeTeamService);

            var repository = (IRepository<User>)Array.Find(typeof(UserService).GetFields(BindingFlags.NonPublic | BindingFlags.Instance), f => f.Name == "repository")
                    .GetValue(userService);

            await userService.CreateUser(A.Fake<UserCreateDTO>());

            A.CallTo(() => fakeUnitOfWork.SaveChangesAsync()).MustHaveHappened(1, Times.Exactly);
        }

        [Fact]
        public async Task CreateUser_WhenNewUserToTeam_ThenUsersInTeamPlusOne()
        {
            var team = new Team { Id = 1, Name = "new team", CreatedAt = DateTime.Now };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var user = A.Fake<UserCreateDTO>();
            user.TeamId = team.Id;
            await userService.CreateUser(user);

            Assert.Single((await teamService.GetAllTeams()).FirstOrDefault()?.Participants);
        }
    }
}
