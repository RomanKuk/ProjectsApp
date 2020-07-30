using Common.DTOs.TaskState;
using Common.DTOs.User;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.TaskModel
{
    public class TaskModelUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FinishedAt { get; set; }
        public TaskStateDTO State { get; set; }
        public int ProjectId { get; set; }
        public int PerformerId { get; set; }
    }
}
