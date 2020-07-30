using AutoMapper;
using Common.DTOs.Project;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.MappingProfiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectDTO, Project>();
            CreateMap<ProjectCreateDTO, Project>();
            CreateMap<ProjectUpdateDTO, Project>();
        }
    }
}
