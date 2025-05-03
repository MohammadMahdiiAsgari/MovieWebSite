using Movie.Data.Context;
using Movie.Domain.Contracts;
using Movie.Domain.DTOs;
using Movie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyMovieContext context;

        public UserRepository(MyMovieContext context)
        {
            this.context = context;
        }

        public void AddUser(User user)
        {
            context.Users.Add(user);
        }

        public void DeleteUser(User User)
        {
            User.IsDeleted = true;
            UpdateUser(User);
        }

        public void DeleteUserById(int UserId)
        {
            var user = GetUserById(UserId);
            user.IsDeleted = true;
            UpdateUser(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return context.Users;
        }

        public User GetUserById(int UserId)
        {
            return context.Users.Find(UserId);
        }

        public void UpdateUser(User user)
        {
            context.Users.Update(user);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public bool UserNameExist(string Username)
        {
            return context.Users.Any(u => u.UserName == Username);
        }

        public bool EmialExist(string Email)
        {
            return context.Users.Any(u => u.Email == Email);
        }

        public User GetUserForLogin(string Email, string Password)
        {
            return context.Users.SingleOrDefault(u => u.Email == Email && u.Password == Password);
        }

        public bool DuplicatedUserName(string UserName, int userId)
        {
            return context.Users.Any(u => u.UserName == UserName && u.Id != userId);
        }

        public bool DuplicatedEmail(string Email, int userId)
        {
            return context.Users.Any(u => u.Email == Email && u.Id != userId);
        }

        public int GetUserCount()
        {
            return context.Users.Count();
        }

        public List<UserDTO> GetAllUsers(int take = 10, int skip = 0)
        {
            //Projection

            return context.Users
                .OrderByDescending(u => u.CreateDate)
                .Select(u => new UserDTO()
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    CreateDate = u.CreateDate,
                    IsAdmin = u.IsAdmin,
                    IsDeleted = u.IsDeleted,
                })
                .Skip(skip)
                .Take(take)
                .ToList();

        }
    }
}
