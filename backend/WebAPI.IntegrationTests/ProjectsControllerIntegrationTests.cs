using Common.DTOs.Project;
using Common.DTOs.Team;
using Common.DTOs.User;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebAPI.IntegrationTests
{
    public class ProjectsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly HttpClient client;

        public ProjectsControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            client = factory.CreateClient();  
        }


        [Fact]
        public async Task Create_ThanResponseWithCode201AndCorrespondedBody()
        {
            var httpResponseTeams = await client.GetAsync($"api/teams");
            var stringResponseTeams = await httpResponseTeams.Content.ReadAsStringAsync();
            var teams = JsonConvert.DeserializeObject<ICollection<TeamDTO>>(stringResponseTeams);

            var httpResponseUsers = await client.GetAsync($"api/users");
            var stringResponseUsers = await httpResponseUsers.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<ICollection<UserDTO>>(stringResponseUsers);

            var project = new ProjectCreateDTO
            {
                Name = "A",
                Description = "BBB",
                Deadline = DateTime.Now + new TimeSpan(1, 0, 0, 0),
                AuthorId = users.Max(u => u.Id),
                TeamId = teams.Max(t => t.Id)
            };

            string jsonInString = JsonConvert.SerializeObject(project);
            var httpResponse = await client.PostAsync("api/projects", 
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var createdProject = JsonConvert.DeserializeObject<ProjectDTO>(stringResponse);

            await client.DeleteAsync($"api/projects/{createdProject.Id}");

            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
            Assert.Equal(project.Name, createdProject.Name);
            Assert.Equal(project.Description, createdProject.Description);
            Assert.Equal(project.Deadline, createdProject.Deadline);
            Assert.Equal(project.AuthorId, createdProject.Author.Id);
            Assert.Equal(project.TeamId, createdProject.Team.Id);
        }


        [Fact]
        public async Task Create_WhenDeadlineEarliearOrEqualCreatedDate_ThenResponseCode400()
        {
            var httpResponseTeams = await client.GetAsync($"api/teams");
            var stringResponseTeams = await httpResponseTeams.Content.ReadAsStringAsync();
            var teams = JsonConvert.DeserializeObject<ICollection<TeamDTO>>(stringResponseTeams);

            var httpResponseUsers = await client.GetAsync($"api/users");
            var stringResponseUsers = await httpResponseUsers.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<ICollection<UserDTO>>(stringResponseUsers);

            var projectDeadlineEarlier = new ProjectCreateDTO
            {
                Name = "A",
                Description = "BBB",
                Deadline = DateTime.Now - new TimeSpan(1, 0, 0, 0),
                AuthorId = users.Max(u => u.Id),
                TeamId = teams.Max(t => t.Id)
            };

            string jsonInString = JsonConvert.SerializeObject(projectDeadlineEarlier);
            var httpResponseEarlier = await client.PostAsync("api/projects",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponseEarlier.Content.ReadAsStringAsync();
            var createdProject = JsonConvert.DeserializeObject<ProjectDTO>(stringResponse);

            await client.DeleteAsync($"api/projects/{createdProject.Id}");

            var projectDeadlineEqual = new ProjectCreateDTO
            {
                Name = "A",
                Description = "BBB",
                Deadline = DateTime.Now,
                AuthorId = users.Max(u => u.Id),
                TeamId = teams.Max(t => t.Id)
            };

            jsonInString = JsonConvert.SerializeObject(projectDeadlineEqual);
            var httpResponseEqual = await client.PostAsync("api/projects",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            stringResponse = await httpResponseEqual.Content.ReadAsStringAsync();
            createdProject = JsonConvert.DeserializeObject<ProjectDTO>(stringResponse);

            await client.DeleteAsync($"api/projects/{createdProject.Id}");

            Assert.Equal(HttpStatusCode.BadRequest, httpResponseEarlier.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, httpResponseEqual.StatusCode);
        }

        [Fact]
        public async Task Create_WhenAuthorOrTeamIsUnreal_ThenResponseCode404()
        {
            var httpResponseTeams = await client.GetAsync($"api/teams");
            var stringResponseTeams = await httpResponseTeams.Content.ReadAsStringAsync();
            var teams = JsonConvert.DeserializeObject<ICollection<TeamDTO>>(stringResponseTeams);

            var httpResponseUsers = await client.GetAsync($"api/users");
            var stringResponseUsers = await httpResponseUsers.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<ICollection<UserDTO>>(stringResponseUsers);

            var projectAuthorUnreal = new ProjectCreateDTO
            {
                Name = "A",
                Description = "BBB",
                Deadline = DateTime.Now - new TimeSpan(1, 0, 0, 0),
                AuthorId = 0,
                TeamId = teams.Max(t => t.Id)
            };

            string jsonInString = JsonConvert.SerializeObject(projectAuthorUnreal);
            var httpResponseAuthorUnreal = await client.PostAsync("api/projects",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponseAuthorUnreal.Content.ReadAsStringAsync();
            var createdProject = JsonConvert.DeserializeObject<ProjectDTO>(stringResponse);

            await client.DeleteAsync($"api/projects/{createdProject.Id}");

            var projectTeamUnreal = new ProjectCreateDTO
            {
                Name = "A",
                Description = "BBB",
                Deadline = DateTime.Now,
                AuthorId = users.Max(u => u.Id),
                TeamId = 0
            };

            jsonInString = JsonConvert.SerializeObject(projectTeamUnreal);
            var httpResponseTeamUnreal = await client.PostAsync("api/projects",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            stringResponse = await httpResponseTeamUnreal.Content.ReadAsStringAsync();
            createdProject = JsonConvert.DeserializeObject<ProjectDTO>(stringResponse);

            await client.DeleteAsync($"api/projects/{createdProject.Id}");

            Assert.Equal(HttpStatusCode.NotFound, httpResponseAuthorUnreal.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, httpResponseTeamUnreal.StatusCode);
        }
        public void Dispose()
        {
            
        }
    }
}
