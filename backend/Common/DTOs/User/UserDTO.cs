using Common.DTOs.Project;
using Common.DTOs.TaskModel;
using Common.DTOs.Team;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime RegisteredAt { get; set; }
        public int? TeamId { get; set; }

        public override string ToString()
        {
            return string.Format($"id:\t{Id}\n\t" +
                $"firstName:\t{FirstName ?? "This user has no first name"}\n\t" +
                $"lastName:\t{LastName ?? "This user has no last name"}\n\t" +
                $"email:\t{Email ?? "This user has no email"}\n\t" +
                $"birthday:\t{Birthday}\n\tregisteredAt:\t{RegisteredAt}\n\t" +
                $"teamId:\t{TeamId}");
        }
    }
}
