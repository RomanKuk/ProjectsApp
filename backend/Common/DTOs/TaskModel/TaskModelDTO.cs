using Common.DTOs.Project;
using Common.DTOs.TaskState;
using Common.DTOs.User;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.TaskModel
{
    public class TaskModelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public TaskStateDTO State { get; set; }
        public int ProjectId { get; set; }
        public UserDTO Performer { get; set; }

        public override string ToString()
        {
            return string.Format($"\tid:\t{Id}\n\t" +
                $"name:\t{Name ?? "This task has no name"}\n\n\t" +
                $"description:\t{Description ?? "This task has no description"}\n\n\t" +
                $"createdAt:\t{CreatedAt}\n\tfinishedAt:\t{FinishedAt}\n\tstate:\t{State}\n\t" +
                $"performer:\n\t[\n\t{Performer}\n\t]\n\n");
        }
    }
}