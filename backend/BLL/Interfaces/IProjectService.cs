using Common.DTOs.FunctionalModels;
using Common.DTOs.Project;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IProjectService : IDisposable
    {
        Task<ICollection<ProjectDTO>> GetAllProjects();
        Task<ProjectDTO> GetProjectById(int id);
        Task<ProjectDTO> CreateProject(ProjectCreateDTO projectDto);
        Task<ProjectDTO> UpdateProject(ProjectUpdateDTO projectDto);
        Task DeleteProjectById(int id);
        Task<Dictionary<ProjectDTO, int>> GetUserProjectTasksCountByUserId(int userId);
        Task<IEnumerable<Project_TasksDTO>> CreateNewProjectStructureAsync();
    }
}
