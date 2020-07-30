using Common.DTOs.FunctionalModels;
using Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Collections_LINQ.Interfaces
{
    interface IUserService
    {
        Task<ICollection<UserDTO>> GetUsers();
        Task<UserDTO> GetUserById(int id);
        Task<UserDTO> CreateUser(UserCreateDTO user);

        Task<UserDTO> UpdateUser(UserUpdateDTO user);

        Task DeleteUser(int id);
    }
}
