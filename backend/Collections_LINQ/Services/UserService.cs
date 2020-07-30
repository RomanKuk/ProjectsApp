using Collections_LINQ.Interfaces;
using Common.DTOs.FunctionalModels;
using Common.DTOs.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Collections_LINQ.Services
{
    internal class UserService : IUserService
    {
        private readonly string baseUrl;
        private readonly HttpClient client;

        public UserService(string baseUrl, HttpClient client)
        {
            this.baseUrl = baseUrl;
            this.client = client;
        }

        public async Task<ICollection<UserDTO>> GetUsers()
        {
            var users = await client.GetStringAsync($"{baseUrl}/api/users");
            return JsonConvert.DeserializeObject<ICollection<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await client.GetStringAsync($"{baseUrl}/api/users/{id}");
            return JsonConvert.DeserializeObject<UserDTO>(user);
        }

        public async Task<UserDTO> CreateUser(UserCreateDTO user)
        {
            var response = await client.PostAsJsonAsync($"{baseUrl}/api/users", user);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new Exception($"{response.StatusCode} : {await response.Content.ReadAsStringAsync()}");
            }
            return await response.Content.ReadAsAsync<UserDTO>();
        }

        public async Task<UserDTO> UpdateUser(UserUpdateDTO user)
        {
            var response = await client.PutAsJsonAsync($"{baseUrl}/api/users", user);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"{response.StatusCode} : {await response.Content.ReadAsStringAsync()}");
            }
            return await response.Content.ReadAsAsync<UserDTO>();
        }

        public async Task DeleteUser(int id)
        {
            var response = await client.DeleteAsync($"{baseUrl}/api/users/{id}");
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                throw new Exception($"{response.StatusCode} : {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}
