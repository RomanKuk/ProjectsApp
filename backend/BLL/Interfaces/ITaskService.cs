using Common.DTOs.FunctionalModels;
using Common.DTOs.TaskModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ITaskService : IDisposable
    {
        Task<ICollection<TaskModelDTO>> GetAllTasks();
        Task<TaskModelDTO> GetTaskById(int id);
        Task<TaskModelDTO> CreateTask(TaskModelCreateDTO taskDto);
        Task<TaskModelDTO> UpdateTask(TaskModelUpdateDTO taskDto);
        Task DeleteTaskById(int id);
        Task<IEnumerable<TaskModelDTO>> GetTasksByUserIdWithNameConditionAsync(int userId);
        Task<IEnumerable<TaskFinishedThisYearDTO>> GetTasksByUserFinishedThisYear(int userId);
        Task<IEnumerable<User_TasksDTO>> GetUsersSortedByFirstNameAndSortedTasksAsync();
        Task<IEnumerable<UserLastProjectTasksDTO>> CreateNewUserStructureAsync(int userId);
        Task<IEnumerable<TaskModelDTO>> GetUnfinishedTasksByUser(int userId);
        Task<IEnumerable<TaskModelDTO>> GetUnfinishedTasks();
    }
}
