using DAL.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            Tasks = new HashSet<TaskModel>();
            Projects = new HashSet<Project>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime RegisteredAt { get; set; }

        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }

        public virtual ICollection<TaskModel> Tasks { get; }
        public virtual ICollection<Project> Projects { get; }
    }
}
