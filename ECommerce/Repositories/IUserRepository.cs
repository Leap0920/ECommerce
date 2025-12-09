using System.Collections.Generic;
using ECommerce.Models;

namespace ECommerce.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetById(int id);
        User GetByEmail(string email);
        User Create(User user);
        bool Update(User user);
        bool Delete(int id);
        bool EmailExists(string email);
    }
}
