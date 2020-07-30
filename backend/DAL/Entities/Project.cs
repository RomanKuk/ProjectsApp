using DAL.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class Project : BaseEntity
    {
        public Project()
        {
            Tasks = new HashSet<TaskModel>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Deadline { get; set; }

        public int AuthorId { get; set; }
        public virtual User Author { get; set; }

        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        public virtual ICollection<TaskModel> Tasks { get; }
    }
}
