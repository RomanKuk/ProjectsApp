using Common.DTOs.TaskModel;
using Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.FunctionalModels
{
    public class User_TasksDTO
    {
        public UserDTO User { get; set; }
        public List<TaskModelDTO> Tasks { get; set; }
    }
}
