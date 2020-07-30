using DAL.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class TaskModel: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public TaskState State { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public int PerformerId { get; set; }
        public virtual User Performer { get; set; }
    }
}
