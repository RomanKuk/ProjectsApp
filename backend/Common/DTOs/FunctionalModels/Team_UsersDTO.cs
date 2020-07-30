using Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.FunctionalModels
{
    public class Team_UsersDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserDTO> Users { get; set; }
    }
}
