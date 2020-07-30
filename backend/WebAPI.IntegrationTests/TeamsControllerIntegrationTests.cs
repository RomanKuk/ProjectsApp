using Common.DTOs.Team;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebAPI.IntegrationTests
{
    public class TeamsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly HttpClient client;

        public TeamsControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            client = factory.CreateClient();
        }

        [Fact]
        public async Task Create_ThanResponseWithCode201AndCorrespondedBody()
        {
            var team = new TeamCreateDTO
            {
                Name = "A"
            };

            string jsonInString = JsonConvert.SerializeObject(team);
            var httpResponse = await client.PostAsync("api/teams",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var createdTeam = JsonConvert.DeserializeObject<TeamDTO>(stringResponse);

            await client.DeleteAsync($"api/teams/{createdTeam.Id}");

            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
            Assert.Equal(team.Name, createdTeam.Name);
        }

        public void Dispose()
        {
            
        }
    }
}
