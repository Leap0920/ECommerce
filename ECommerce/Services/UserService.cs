using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerce.Models;
using ECommerce.Models.ViewModels;

namespace ECommerce.Services
{
    public class UserService
    {
        private readonly HttpContextBase _httpContext;

        public UserService(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public List<User> GetAllUsers()
        {
            return CacheDataService.GetUsers(_httpContext);
        } 

        public User GetUserById(int id)
        {
            var users = GetAllUsers();
            return users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByEmail(string email)
        {
            var users = GetAllUsers();
            return users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

        public User RegisterUser(RegisterViewModel model)
        {
            var users = GetAllUsers();
            
            // Check if email already exists
            if (users.Any(u => u.Email.ToLower() == model.Email.ToLower()))
            {
                return null; // Email already exists
            }

            var newUser = new User
            {
                Id = users.Any() ? users.Max(u => u.Id) + 1 : 1,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password, // In production, hash this with BCrypt
                Role = "Customer",
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            users.Add(newUser);
            _httpContext.Cache["Users"] = users;
            return newUser;
        }

        public User AuthenticateUser(LoginViewModel model)
        {
            var user = GetUserByEmail(model.Email);
            if (user != null && user.Password == model.Password && user.IsActive)
            {
                return user;
            }
            return null;
        }
    }
}

