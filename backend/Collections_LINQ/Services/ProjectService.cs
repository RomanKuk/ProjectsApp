using Collections_LINQ.Interfaces;
using Common.Converters;
using Common.DTOs.FunctionalModels;
using Common.DTOs.Project;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Collections_LINQ.Services
{
    internal class ProjectService: IProjectService
    {
        private readonly string baseUrl;
        private readonly HttpClient client;

        public ProjectService(string baseUrl, HttpClient client)
        {
            this.baseUrl = baseUrl;
            this.client = client;
        }

        public async Task<ICollection<ProjectDTO>> GetProjects()
        {
            var projects = await client.GetStringAsync($"{baseUrl}/api/projects");
            return JsonConvert.DeserializeObject<ICollection<ProjectDTO>>(projects);
        }

        public async Task<ProjectDTO> GetProjectById(int id)
        {
            var project = await client.GetStringAsync($"{baseUrl}/api/projects/{id}");
            return JsonConvert.DeserializeObject<ProjectDTO>(project);
        }

        public async Task<ProjectDTO> CreateProject(ProjectCreateDTO project)
        {
            var response = await client.PostAsJsonAsync($"{baseUrl}/api/projects", project);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new Exception($"{response.StatusCode} : {await response.Content.ReadAsStringAsync()}");
            }
            return await response.Content.ReadAsAsync<ProjectDTO>();
        }

        public async Task<ProjectDTO> UpdateProject(ProjectUpdateDTO project)
        {
            var response = await client.PutAsJsonAsync($"{baseUrl}/api/projects", project);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"{response.StatusCode} : {await response.Content.ReadAsStringAsync()}");
            }
            return await response.Content.ReadAsAsync<ProjectDTO>();
        }

        public async Task DeleteProject(int id)
        {
            var response = await client.DeleteAsync($"{baseUrl}/api/projects/{id}");
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                throw new Exception($"{response.StatusCode} : {await response.Content.ReadAsStringAsync()}");
            }
        }

        public async Task<IEnumerable<Project_TasksDTO>> GetNewProjectStructure()
        {
            var projects = await client.GetStringAsync($"{baseUrl}/api/projects/new-project-structure");
            return JsonConvert.DeserializeObject<IEnumerable<Project_TasksDTO>>(projects);
        }

        public async Task<Dictionary<ProjectDTO, int>> GetUserProjectTasksCountByUserId(int id)
        {
            var projects = await client.GetStringAsync($"{baseUrl}/api/projects/project-tasks-count-by-user/{id}");

            return JsonConvert.DeserializeObject<Dictionary<ProjectDTO, int>>(projects, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                ContractResolver = new DictionaryArrayConverter()
            });
        }
    }
}
