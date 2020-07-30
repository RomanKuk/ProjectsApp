using Common.DTOs.FunctionalModels;
using Common.DTOs.TaskModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Collections_LINQ.Interfaces
{
    internal interface ITaskService
    {
        Task<ICollection<TaskModelDTO>> GetTasks();
        Task<TaskModelDTO> GetTaskById(int id);

        Task<TaskModelDTO> CreateTask(TaskModelCreateDTO task);

        Task<TaskModelDTO> UpdateTask(TaskModelUpdateDTO task);

        Task DeleteTask(int id);

        Task<IEnumerable<TaskModelDTO>> GetTasksByUserIdWithNameCondition(int id);

        Task<IEnumerable<TaskFinishedThisYearDTO>> GetTasksByUserFinishedThisYear(int id);

        Task<IEnumerable<User_TasksDTO>> GetUsersSortedByFirstNameAndSortedTasks();

        Task<IEnumerable<UserLastProjectTasksDTO>> GetNewUserStructure(int id);
        Task<IEnumerable<TaskModelDTO>> GetUnfinishedTasksByUser(int userId);
        Task<int> MarkRandomTaskWithDelay(int delay);
    }
}
