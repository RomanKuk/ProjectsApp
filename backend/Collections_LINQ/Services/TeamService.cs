using Collections_LINQ.Interfaces;
using Common.DTOs.FunctionalModels;
using Common.DTOs.Team;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Collections_LINQ.Services
{
    class TeamService : ITeamService
    {
        private readonly string baseUrl;
        private readonly HttpClient client;

        public TeamService(string baseUrl, HttpClient client)
        {
            this.baseUrl = baseUrl;
            this.client = client;
        }

        public async Task<ICollection<TeamDTO>> GetTeams()
        {
            var teams = await client.GetStringAsync($"{baseUrl}/api/teams");
            return JsonConvert.DeserializeObject<ICollection<TeamDTO>>(teams);
        }

        public async Task<TeamDTO> GetTeamById(int id)
        {
            var team = await client.GetStringAsync($"{baseUrl}/api/teams/{id}");
            return JsonConvert.DeserializeObject<TeamDTO>(team);
        }

        public async Task<TeamDTO> CreateTeam(TeamCreateDTO team)
        {
            var response = await client.PostAsJsonAsync($"{baseUrl}/api/teams", team);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new Exception($"{response.StatusCode} : {await response.Content.ReadAsStringAsync()}");
            }
            return await response.Content.ReadAsAsync<TeamDTO>();
        }

        public async Task<TeamDTO> UpdateTeam(TeamUpdateDTO team)
        {
            var response = await client.PutAsJsonAsync($"{baseUrl}/api/teams", team);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"{response.StatusCode} : {await response.Content.ReadAsStringAsync()}");
            }
            return await response.Content.ReadAsAsync<TeamDTO>();
        }

        public async Task DeleteTeam(int id)
        {
            var response = await client.DeleteAsync($"{baseUrl}/api/teams/{id}");
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                throw new Exception($"{response.StatusCode} : {await response.Content.ReadAsStringAsync()}");
            }
        }

        public async Task<IEnumerable<Team_UsersDTO>> GetTeamsUsersSortedByRegisteredDate()
        {
            var teams = await client.GetStringAsync($"{baseUrl}/api/teams/team-users-sorted-by-registered-date");
            return JsonConvert.DeserializeObject<IEnumerable<Team_UsersDTO>>(teams);
        }
    }
}
