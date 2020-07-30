using Common.DTOs.Project;
using Common.DTOs.TaskModel;
using Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.FunctionalModels
{
    public class UserLastProjectTasksDTO
    {
        public UserDTO User { get; set; }
        public ProjectDTO LastProject { get; set; }
        public int? LastProjectTasksCount { get; set; }
        public int UndoneOrCanceledTasksCount { get; set; }
        public TaskModelDTO LongestTask { get; set; }
    }
}
