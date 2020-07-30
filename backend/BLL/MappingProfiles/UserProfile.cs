using AutoMapper;
using Common.DTOs.User;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserUpdateDTO, User>();
        }
    }
}
