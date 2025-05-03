using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32;
using Movie.Application.Security;
using Movie.Application.Services.Interfaces;
using Movie.Domain.Contracts;
using Movie.Domain.DTOs;
using Movie.Domain.Enums;
using Movie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public ResultCreateUser CreateUser(CreateUserDTO createUser)
        {
            createUser.Email = createUser.Email.Trim().ToLower();
            createUser.UserName = createUser.UserName.Trim();

            if (userRepository.UserNameExist(createUser.UserName))
            {
                return ResultCreateUser.UserNameNotValid;
            }

            if (userRepository.EmialExist(createUser.Email))
            {
                return ResultCreateUser.EmailNotValid;
            }

            User user = new User
            {
                UserName = createUser.UserName,
                Email = createUser.Email,
                FirstName = createUser.FirstName,
                LastName = createUser.LastName,
                CreateDate = DateTime.Now,
                IsDeleted = false,
                IsAdmin = createUser.IsAdmin,
                Password = PasswordHasher.EncodePasswordMd5(createUser.Password),
            };

            userRepository.AddUser(user);
            userRepository.Save();
            return ResultCreateUser.Success;
        }

        public bool Delete(int userId)
        {
            var user=userRepository.GetUserById(userId);
            if(user == null)
                return false;

            user.IsDeleted = true;

            userRepository.UpdateUser(user);
            userRepository.Save();

            return true;
        }

        public EditUserDTO EditUser(int userId)
        {
            var user = userRepository.GetUserById(userId);

            if (user == null)
                return null;

            return new EditUserDTO
            {
                UserId = userId,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NewPassword = null,
                IsAdmin = user.IsAdmin,
                IsDeleted = user.IsDeleted

            };
        }

        public ResultEditUser EditUser(EditUserDTO editUser)
        {
            var user = userRepository.GetUserById(editUser.UserId);

            if (user == null)
                return ResultEditUser.UserNotFound;

            editUser.Email = editUser.Email.Trim().ToLower();
            editUser.UserName = editUser.UserName.Trim();

            if (userRepository.DuplicatedUserName(editUser.UserName, editUser.UserId))
                return ResultEditUser.UserNameDuplicated;

            if(userRepository.DuplicatedEmail(editUser.Email, editUser.UserId))
                return ResultEditUser.EmailDuplicated;

            user.Email = editUser.Email;
            user.UserName = editUser.UserName;
            user.FirstName = editUser.FirstName;
            user.LastName = editUser.LastName;
            user.ModifiedDate=DateTime.Now;
            user.IsAdmin = editUser.IsAdmin;
            user.IsDeleted = editUser.IsDeleted;
            
            if(!string.IsNullOrEmpty(editUser.NewPassword))
                user.Password=PasswordHasher.EncodePasswordMd5(editUser.NewPassword);

            userRepository.UpdateUser(user);
            userRepository.Save();
            
            return ResultEditUser.Success;

        }

        public List<UserDTO> GetAllUsers(int take=10, int skip=0)
        {
            return userRepository.GetAllUsers(take, skip);
        }

        public int GetUserCount()
        {
            return userRepository.GetUserCount();
        }

        public UserDTO GetUserDetailsById(int UserId)
        {
            var user = userRepository.GetUserById(UserId);

            if (user == null)
                return null;

            return new UserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                CreateDate = DateTime.Now,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin,
                Id = user.Id,
                IsDeleted = user.IsDeleted,
                ModifiedDate = DateTime.Now
            };
        }

        public ResultRegisterUser RegisterUser(RegisterDTO register)
        {
            if (userRepository.UserNameExist(register.UserName.Trim()))
            {
                return ResultRegisterUser.UserNameNotValid;
            }

            if (userRepository.EmialExist(register.Email.Trim().ToLower()))
            {
                return ResultRegisterUser.EmailNotValid;
            }

            User user = new User
            {
                UserName = register.UserName.Trim(),
                Email = register.Email.Trim().ToLower(),
                FirstName = register.FirstName,
                LastName = register.LastName,
                CreateDate = DateTime.Now,
                IsDeleted = false,
                IsAdmin = false,
                Password = PasswordHasher.EncodePasswordMd5(register.Password),
            };

            userRepository.AddUser(user);
            userRepository.Save();
            return ResultRegisterUser.Success;
        }

        public ResultChangePassword UserChangePassword(int UserId, ChangePasswordDTO changePassword)
        {
            try
            {
                var user = userRepository.GetUserById(UserId);

                string HashOldPassword = PasswordHasher.EncodePasswordMd5(changePassword.OldPassword);

                if (user.Password != HashOldPassword)
                {
                    return ResultChangePassword.OldPasswordInValid;
                }

                user.Password = PasswordHasher.EncodePasswordMd5(changePassword.Password);
                userRepository.UpdateUser(user);
                userRepository.Save();

                return ResultChangePassword.Success;
            }
            catch
            {
                return ResultChangePassword.Error;
            }
        }

        public User UserLogin(LoginDTO login)
        {
            string HashPassword = PasswordHasher.EncodePasswordMd5(login.Password);
            var user = userRepository.GetUserForLogin(login.Email, HashPassword);
            return user;
        }
    }
}
