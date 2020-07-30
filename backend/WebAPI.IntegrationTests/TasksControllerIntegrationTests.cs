using Common.DTOs.Project;
using Common.DTOs.TaskModel;
using Common.DTOs.User;
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
    public class TasksControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly HttpClient client;

        public TasksControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            client = factory.CreateClient();
        }

        [Fact]
        public async Task DeleteById_ThanResponseWithCode204AndGetByIdResponseCode404()
        {
            var httpResponseProjects = await client.GetAsync($"api/projects");
            var stringResponseProjects = await httpResponseProjects.Content.ReadAsStringAsync();
            var projects = JsonConvert.DeserializeObject<ICollection<ProjectDTO>>(stringResponseProjects);

            var httpResponseUsers = await client.GetAsync($"api/users");
            var stringResponseUsers = await httpResponseUsers.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<ICollection<UserDTO>>(stringResponseUsers);

            var taskCreate = new TaskModelCreateDTO
            {
                FinishedAt = DateTime.Now + new TimeSpan(1,0,0,0),
                ProjectId = projects.Max(p => p.Id),
                PerformerId = users.Max(u => u.Id)
            };

            string jsonInString = JsonConvert.SerializeObject(taskCreate);
            await client.PostAsync("api/tasks",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            var httpResponseTasks = await client.GetAsync($"api/tasks");
            var stringResponseTasks = await httpResponseTasks.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<ICollection<TaskModelDTO>>(stringResponseTasks);

            var httpResponse = await client.DeleteAsync($"api/tasks/{tasks.Max(t => t.Id)}");

            Assert.Equal(HttpStatusCode.NoContent, httpResponse.StatusCode);

            var httpResponseGetById = await client.GetAsync($"api/users/{tasks.Max(t => t.Id)}");

            Assert.Equal(HttpStatusCode.NotFound, httpResponseGetById.StatusCode);
        }

        [Fact]
        public async Task DeleteById_WhenUserNotExist_ThenResponseCode404()
        {

            var httpResponse = await client.DeleteAsync($"api/tasks/{0}");
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task GetUnfinishedTasksByUser_WhenUserNotExist_ThenResponseCode404()
        {
            var httpResponse = await client.GetAsync($"api/tasks/unfinished-tasks-by-user/{0}");
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task GetUnfinishedTasksByUser_WhenNewUserAndNewUnfinishedTask_ThenResponseCode200()
        {
            var userCreate = new UserCreateDTO
            {
                FirstName = "A",
                LastName = "B",
                Birthday = new DateTime(2000, 10, 10),
                TeamId = null
            };

            string jsonInStringUser = JsonConvert.SerializeObject(userCreate);
            var httpResponseUser = await client.PostAsync("api/users",
                new StringContent(jsonInStringUser, Encoding.UTF8, "application/json"));
            var stringResponseUser = await httpResponseUser.Content.ReadAsStringAsync();
            var createdUser = JsonConvert.DeserializeObject<UserDTO>(stringResponseUser);

            var httpResponseProjects = await client.GetAsync($"api/projects");
            var stringResponseProjects = await httpResponseProjects.Content.ReadAsStringAsync();
            var projects = JsonConvert.DeserializeObject<ICollection<ProjectDTO>>(stringResponseProjects);

            var taskCreate = new TaskModelCreateDTO
            {
                FinishedAt = DateTime.Now + new TimeSpan(1, 0, 0, 0),
                ProjectId = projects.Max(p => p.Id),
                PerformerId = createdUser.Id
            };

            string jsonInString = JsonConvert.SerializeObject(taskCreate);
            var httpResponseTask = await client.PostAsync("api/tasks",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var stringResponseTask = await httpResponseUser.Content.ReadAsStringAsync();
            var createdTask = JsonConvert.DeserializeObject<TaskModelDTO>(stringResponseTask);

            var httpResponseResult = await client.GetAsync($"api/tasks/unfinished-tasks-by-user/{createdUser.Id}");

            await client.DeleteAsync($"api/tasks/{createdTask.Id}");
            await client.DeleteAsync($"api/users/{createdUser.Id}");


            Assert.Equal(HttpStatusCode.OK, httpResponseResult.StatusCode);
        }

        public void Dispose()
        {
            
        }
    }
}
