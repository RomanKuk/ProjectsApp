using Collections_LINQ.Interfaces;
using Common.DTOs.FunctionalModels;
using Common.DTOs.TaskModel;
using Common.DTOs.TaskState;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace Collections_LINQ.Services
{
    internal class TaskService : ITaskService
    {
        private readonly string baseUrl;
        private readonly HttpClient client;

        public TaskService(string baseUrl, HttpClient client)
        {
            this.baseUrl = baseUrl;
            this.client = client;
        }

        public async Task<TaskModelDTO> GetTaskById(int id)
        {
            var task = await client.GetStringAsync($"{baseUrl}/api/tasks/{id}");
            return JsonConvert.DeserializeObject<TaskModelDTO>(task);
        }

        public async Task<ICollection<TaskModelDTO>> GetTasks()
        {
            var tasks = await client.GetStringAsync($"{baseUrl}/api/tasks");
            return JsonConvert.DeserializeObject<ICollection<TaskModelDTO>>(tasks);
        }

        public async Task<TaskModelDTO> CreateTask(TaskModelCreateDTO task)
        {
            var response = await client.PostAsJsonAsync($"{baseUrl}/api/tasks", task);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new Exception($"{response.StatusCode} : {await response.Content.ReadAsStringAsync()}");
            }
            return await response.Content.ReadAsAsync<TaskModelDTO>();
        }

        public async Task<TaskModelDTO> UpdateTask(TaskModelUpdateDTO task)
        {
            var response = await client.PutAsJsonAsync($"{baseUrl}/api/tasks", task);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"{response.StatusCode} : {await response.Content.ReadAsStringAsync()}");
            }
            return await response.Content.ReadAsAsync<TaskModelDTO>();
        }

        public async Task DeleteTask(int id)
        {
            var response = await client.DeleteAsync($"{baseUrl}/api/tasks/{id}");
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                throw new Exception($"{response.StatusCode} : {await response.Content.ReadAsStringAsync()}");
            }
        }

        public async Task<IEnumerable<TaskModelDTO>> GetTasksByUserIdWithNameCondition(int id)
        {
            var tasks = await client.GetStringAsync($"{baseUrl}/api/tasks/by-user-with-name-condition/{id}");
            return JsonConvert.DeserializeObject<IEnumerable < TaskModelDTO >> (tasks);
        }

        public async Task<IEnumerable<TaskFinishedThisYearDTO>> GetTasksByUserFinishedThisYear(int id)
        {
            var tasks = await client.GetStringAsync($"{baseUrl}/api/tasks/by-user-finished-this-year/{id}");
            return JsonConvert.DeserializeObject<IEnumerable<TaskFinishedThisYearDTO>>(tasks);
        }

        public async Task<IEnumerable<User_TasksDTO>> GetUsersSortedByFirstNameAndSortedTasks()
        {
            var tasks = await client.GetStringAsync($"{baseUrl}/api/tasks/users-sorted-by-first-name-sorted-tasks");
            return JsonConvert.DeserializeObject<IEnumerable<User_TasksDTO>>(tasks);
        }

        public async Task<IEnumerable<UserLastProjectTasksDTO>> GetNewUserStructure(int id)
        {
            var tasks = await client.GetStringAsync($"{baseUrl}/api/tasks/structure-with-tasks-by-user/{id}");
            return JsonConvert.DeserializeObject<IEnumerable<UserLastProjectTasksDTO>>(tasks);
        }

        public async Task<IEnumerable<TaskModelDTO>> GetUnfinishedTasksByUser(int id)
        {
            var tasks = await client.GetStringAsync($"{baseUrl}/api/tasks/unfinished-tasks-by-user/{id}");
            return JsonConvert.DeserializeObject<IEnumerable<TaskModelDTO>>(tasks);
        }

        public async Task<IEnumerable<TaskModelDTO>> GetUnfinishedTasks()
        {
            var tasks = await client.GetStringAsync($"{baseUrl}/api/tasks/unfinished-tasks");
            return JsonConvert.DeserializeObject<IEnumerable<TaskModelDTO>>(tasks);
        }

        public async Task<int> MarkRandomTaskWithDelay(int delay)
        {
            var tcs = new TaskCompletionSource<int>();

            var timer = new Timer
            {
                Interval = delay,
                AutoReset = false
            };
            timer.Elapsed += async (o, e) =>
            {
                try
                {
                    var unfinishedTasks = await GetUnfinishedTasks();
                    if (!unfinishedTasks.Any())
                    {
                        throw new Exception("All tasks are finished!");
                    }
                    var rand = new Random();
                    int randomNumb = rand.Next(unfinishedTasks.Count());
                    var randomTask = unfinishedTasks.ElementAtOrDefault(randomNumb);
                    var updatedtask = new TaskModelUpdateDTO()
                    {
                        Id = randomTask.Id,
                        Name = randomTask.Name,
                        Description = randomTask.Description,
                        FinishedAt = randomTask.FinishedAt,
                        State = TaskStateDTO.Finished,
                        PerformerId = randomTask.Performer.Id,
                        ProjectId = randomTask.ProjectId
                    };
                    await UpdateTask(updatedtask);

                    tcs.SetResult(updatedtask.Id);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            };

            timer.Enabled = true;

            return await tcs.Task;
        }
    }
}
