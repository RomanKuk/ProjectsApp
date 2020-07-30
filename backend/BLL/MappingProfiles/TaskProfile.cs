using AutoMapper;
using Common.DTOs.TaskModel;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.MappingProfiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskModel, TaskModelDTO>();
            CreateMap<TaskModelDTO, TaskModel>();
            CreateMap<TaskModelCreateDTO, TaskModel>();
            CreateMap<TaskModelUpdateDTO, TaskModel>();
        }
    }
}
