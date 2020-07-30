using AutoMapper;
using Common.DTOs.Team;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.MappingProfiles
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<Team, TeamDTO>();
            CreateMap<TeamDTO, Team>();
            CreateMap<TeamCreateDTO, Team>();
            CreateMap<TeamUpdateDTO, Team>();
        }
    }
}
