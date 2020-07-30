using Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.TaskModel
{
    public class TaskModelCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FinishedAt { get; set; }
        public int ProjectId { get; set; }
        public int PerformerId { get; set; }
    }
}
