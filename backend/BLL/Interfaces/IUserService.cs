using Common.DTOs.FunctionalModels;
using Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<ICollection<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUserById(int id);
        Task<UserDTO> CreateUser(UserCreateDTO userDto);
        Task<UserDTO> UpdateUser(UserUpdateDTO userDto);
        Task DeleteUserById(int id);
    }
}
