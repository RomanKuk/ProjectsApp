﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.User
{
    public class UserCreateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int? TeamId { get; set; }
    }
}
