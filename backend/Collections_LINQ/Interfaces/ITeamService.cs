using Common.DTOs.FunctionalModels;
using Common.DTOs.Team;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Collections_LINQ.Interfaces
{
    interface ITeamService
    {
        Task<ICollection<TeamDTO>> GetTeams();
        Task<TeamDTO> GetTeamById(int id);
        Task<TeamDTO> CreateTeam(TeamCreateDTO team);

        Task<TeamDTO> UpdateTeam(TeamUpdateDTO team);

        Task DeleteTeam(int id);

        Task<IEnumerable<Team_UsersDTO>> GetTeamsUsersSortedByRegisteredDate();
    }
}
