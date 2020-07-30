using AutoMapper;
using BLL.Interfaces;
using BLL.MappingProfiles;
using BLL.Services;
using Bogus;
using Common.DTOs.Team;
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
    public class TeamServiceTests
    {
        private readonly ITeamService teamService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ModelsContext context;

        public TeamServiceTests()
        {
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
        }

        [Fact]
        public async Task GetTeamsUsersSortedByRegisteredDateAsync_WhenUsersElderThan10AndLatestRegistered_ThenFirstUserInUsers()
        {
            var team = await teamService.CreateTeam(A.Fake<TeamCreateDTO>());

            var userId = 1;
            var userFake = new Faker<User>()
                .RuleFor(u => u.Id, f => userId++)
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(pi => pi.Birthday, f => f.Date.Between(new DateTime(1990, 1, 1), new DateTime(2010, 1, 1)))
                .RuleFor(pi => pi.RegisteredAt, f => DateTime.Now)
                .RuleFor(pi => pi.TeamId, f => f.PickRandom(team).Id);

            var users = userFake.Generate(5);
            context.Users.AddRange(users);
            await context.SaveChangesAsync();

            var check = context.Users
                .Where(u => (DateTime.Now.Year - u.Birthday.Year) > 10)
                .OrderByDescending(u => u.RegisteredAt)
                .FirstOrDefault();

            var result = (await teamService.GetTeamsUsersSortedByRegisteredDateAsync()).FirstOrDefault();
            Assert.Equal(check.Id, result.Users.FirstOrDefault().Id);
        }
    }
}
