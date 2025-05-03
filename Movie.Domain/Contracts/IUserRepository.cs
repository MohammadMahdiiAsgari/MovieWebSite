using Movie.Domain.DTOs;
using Movie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Domain.Contracts
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();

        User GetUserById(int UserId);

        User GetUserForLogin(string Email, string Password);

        void AddUser(User user);

        void UpdateUser(User user);

        void DeleteUser(User User);

        void DeleteUserById(int UserId);

        bool UserNameExist(string Username);

        bool EmialExist(string Email);

        void Save();

        bool DuplicatedUserName(string UserName, int userId);

        bool DuplicatedEmail(string Emial, int userId);

        int GetUserCount();

        List<UserDTO> GetAllUsers(int take = 10, int skip = 0);
    }
}
