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
    public class UsersControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly HttpClient client;

        public UsersControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            client = factory.CreateClient();
        }

        [Fact]
        public async Task DeleteById_ThanResponseWithCode204AndGetByIdResponseCode404()
        {
            var userCreate = new UserCreateDTO
            {
                Birthday = new DateTime(2000, 10, 10)
            };

            string jsonInString = JsonConvert.SerializeObject(userCreate);
            await client.PostAsync("api/users",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            var httpResponseUsers = await client.GetAsync($"api/users");
            var stringResponseUsers = await httpResponseUsers.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<ICollection<UserDTO>>(stringResponseUsers);

            var httpResponse = await client.DeleteAsync($"api/users/{users.Max(u => u.Id)}");

            Assert.Equal(HttpStatusCode.NoContent, httpResponse.StatusCode);

            var httpResponseGetById = await client.GetAsync($"api/users/{users.Max(u => u.Id)}");

            Assert.Equal(HttpStatusCode.NotFound, httpResponseGetById.StatusCode);
        }

        [Fact]
        public async Task DeleteById_WhenUserNotExist_ThenResponseCode404()
        {
            var httpResponse = await client.DeleteAsync($"api/users/{0}");
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Create_ThanResponseWithCode201AndCorrespondedBody()
        {
            var userCreate = new UserCreateDTO
            {
                FirstName = "A",
                LastName = "B",
                Birthday = new DateTime(2000, 10, 10),
                TeamId = null
            };

            string jsonInString = JsonConvert.SerializeObject(userCreate);
            var httpResponse = await client.PostAsync("api/users",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var createdUser = JsonConvert.DeserializeObject<UserDTO>(stringResponse);

            await client.DeleteAsync($"api/users/{createdUser.Id}");

            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
            Assert.Equal(userCreate.FirstName, createdUser.FirstName);
            Assert.Equal(userCreate.LastName, createdUser.LastName);
            Assert.Equal(userCreate.Email, createdUser.Email);
            Assert.Equal(userCreate.Birthday, createdUser.Birthday);
            Assert.Equal(userCreate.TeamId, createdUser.TeamId);
        }

        [Fact]
        public async Task Create_WhenTeamIsUnreal_ThanResponseWithCode404()
        {

            var userCreate = new UserCreateDTO
            {
                FirstName = "A",
                LastName = "B",
                Birthday = new DateTime(2000, 10, 10),
                TeamId = -1
            };

            string jsonInString = JsonConvert.SerializeObject(userCreate);
            var httpResponse = await client.PostAsync("api/users",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        public void Dispose()
        {

        }
    }
}
