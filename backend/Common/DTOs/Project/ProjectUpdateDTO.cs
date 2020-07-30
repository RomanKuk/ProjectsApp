using Common.DTOs.Team;
using Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Project
{
    public class ProjectUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int AuthorId { get; set; }
        public int TeamId { get; set; }
    }
}
