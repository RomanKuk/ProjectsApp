using Common.DTOs.FunctionalModels;
using Common.DTOs.Project;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Collections_LINQ.Interfaces
{
    interface IProjectService
    {
        Task<ICollection<ProjectDTO>> GetProjects();
        Task<ProjectDTO> GetProjectById(int id);
        Task<ProjectDTO> CreateProject(ProjectCreateDTO project);
        Task<ProjectDTO> UpdateProject(ProjectUpdateDTO project);
        Task DeleteProject(int id);
        Task<IEnumerable<Project_TasksDTO>> GetNewProjectStructure();
        Task<Dictionary<ProjectDTO, int>> GetUserProjectTasksCountByUserId(int id);
    }
}
