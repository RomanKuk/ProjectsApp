using Common.DTOs.Project;
using Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Team
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<UserDTO> Participants { get; set; }

        public override string ToString()
        {
            return string.Format($"id:\t{Id}\n\t" +
                $"name:\t{Name ?? "This team has no name"}\n\t" +
                $"createdAt:\t{CreatedAt}");
        }
    }
}
