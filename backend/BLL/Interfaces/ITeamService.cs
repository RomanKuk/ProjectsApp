using Common.DTOs.FunctionalModels;
using Common.DTOs.Team;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ITeamService : IDisposable
    {
        Task<ICollection<TeamDTO>> GetAllTeams();
        Task<TeamDTO> GetTeamById(int id);
        Task<TeamDTO> CreateTeam(TeamCreateDTO teamDto);
        Task<TeamDTO> UpdateTeam(TeamUpdateDTO teamDto);
        Task DeleteTeamById(int id);
        Task<IEnumerable<Team_UsersDTO>> GetTeamsUsersSortedByRegisteredDateAsync();
    }
}
