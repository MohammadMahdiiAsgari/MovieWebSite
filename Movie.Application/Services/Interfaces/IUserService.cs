using Movie.Domain.DTOs;
using Movie.Domain.Enums;
using Movie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Application.Services.Interfaces
{
    public interface IUserService
    {
        ResultRegisterUser RegisterUser(RegisterDTO register);

        User UserLogin(LoginDTO login);

        ResultChangePassword UserChangePassword(int UserId, ChangePasswordDTO changePassword);

        UserDTO GetUserDetailsById(int UserId);

        ResultCreateUser CreateUser(CreateUserDTO createUser);

        EditUserDTO EditUser(int userId);

        ResultEditUser EditUser(EditUserDTO editUser);

        int GetUserCount();

        List<UserDTO> GetAllUsers(int take=10, int skip=0);

        bool Delete(int userId);

    }
}
