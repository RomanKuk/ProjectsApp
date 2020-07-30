using Common.DTOs.TaskModel;
using Common.DTOs.Team;
using Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DTOs.Project
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Deadline { get; set; }

        public ICollection<TaskModelDTO> Tasks { get; set; }

        public TeamDTO Team { get; set; }
        public UserDTO Author { get; set; }

        public override string ToString()
        {
            return string.Format($"id:\t{Id}\n" +
                $"name:\t{Name ?? "This project has no name"}\n\n" +
                $"description:\t{Description ?? "This project has no description"}\n\n" +
                $"createdAt:\t{CreatedAt}\ndeadline:\t{Deadline}\ntasks:\n[\n" +
                $"{(Tasks.Count == 0 ? "\tThis project has no tasks\n" : Tasks.Aggregate("", (cur, next) => cur += next))}]\n\n" +
                $"author:\n[\n\t{Author}\n]\n\nteam:\n[\n\t{Team}\n]\n");
        }
    }
}