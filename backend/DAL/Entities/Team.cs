using DAL.Entities.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace DAL.Entities
{
    public class Team: BaseEntity
    {
        public Team()
        {
            Participants = new HashSet<User>();
            Projects = new HashSet<Project>();
        }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<User> Participants { get; }
        public virtual ICollection<Project> Projects { get; }
    }
}
