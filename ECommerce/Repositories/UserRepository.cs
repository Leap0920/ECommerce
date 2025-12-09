using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using ECommerce.Data;
using ECommerce.Models;

namespace ECommerce.Repositories
{
    public class UserRepository : IUserRepository
    {
        public List<User> GetAll()
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    SELECT id as Id, first_name as FirstName, last_name as LastName, 
                           email as Email, password as Password, role as Role, 
                           is_active as IsActive, created_at as CreatedDate 
                    FROM users 
                    ORDER BY created_at DESC";
                return connection.Query<User>(sql).ToList();
            }
        }

        public User GetById(int id)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    SELECT id as Id, first_name as FirstName, last_name as LastName, 
                           email as Email, password as Password, role as Role, 
                           is_active as IsActive, created_at as CreatedDate 
                    FROM users 
                    WHERE id = @Id";
                return connection.QueryFirstOrDefault<User>(sql, new { Id = id });
            }
        }

        public User GetByEmail(string email)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    SELECT id as Id, first_name as FirstName, last_name as LastName, 
                           email as Email, password as Password, role as Role, 
                           is_active as IsActive, created_at as CreatedDate 
                    FROM users 
                    WHERE LOWER(email) = LOWER(@Email)";
                return connection.QueryFirstOrDefault<User>(sql, new { Email = email });
            }
        }

        public User Create(User user)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    INSERT INTO users (first_name, last_name, email, password, role, is_active, created_at) 
                    VALUES (@FirstName, @LastName, @Email, @Password, @Role, @IsActive, @CreatedDate);
                    SELECT LAST_INSERT_ID();";
                
                user.Id = connection.ExecuteScalar<int>(sql, new
                {
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Password,
                    user.Role,
                    user.IsActive,
                    user.CreatedDate
                });
                return user;
            }
        }

        public bool Update(User user)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    UPDATE users 
                    SET first_name = @FirstName, 
                        last_name = @LastName, 
                        email = @Email, 
                        password = @Password, 
                        role = @Role, 
                        is_active = @IsActive 
                    WHERE id = @Id";
                
                var rowsAffected = connection.Execute(sql, user);
                return rowsAffected > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = "DELETE FROM users WHERE id = @Id";
                var rowsAffected = connection.Execute(sql, new { Id = id });
                return rowsAffected > 0;
            }
        }

        public bool EmailExists(string email)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = "SELECT COUNT(1) FROM users WHERE LOWER(email) = LOWER(@Email)";
                var count = connection.ExecuteScalar<int>(sql, new { Email = email });
                return count > 0;
            }
        }
    }
}
