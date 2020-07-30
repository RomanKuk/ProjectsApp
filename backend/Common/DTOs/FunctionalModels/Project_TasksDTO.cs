using Common.DTOs.Project;
using Common.DTOs.TaskModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.FunctionalModels
{
    public class Project_TasksDTO
    {
        public ProjectDTO Project { get; set; }
        public TaskModelDTO LongestTaskByDescription { get; set; }
        public TaskModelDTO ShortestTaskByName { get; set; }
        public int TotalUsersCount { get; set; }
    }
}
