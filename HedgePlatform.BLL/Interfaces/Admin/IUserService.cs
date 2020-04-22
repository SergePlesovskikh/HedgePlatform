using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IUserService
    {
        UserDTO GetUser(int? id);
        IEnumerable<UserDTO> GetUsers();
        void CreateUser(UserDTO user);
        void EditUser(UserDTO user);
        void DeleteUser(int? id);
        void Dispose();
    }
}
