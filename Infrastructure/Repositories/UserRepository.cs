using Application.Interfaces;
using class_project_ieradumi;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly DBContext _dbContext;
        public UserRepository(DBContext context)
        {
            _dbContext = context;
        }
        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            return user?.ID;
        }

        public async Task<string?> RegisterAsync(string username, string password)
        {
            var exists = await _dbContext.Users
                .AnyAsync(u => u.Username == username);

            if (exists)
                return null;

            var user = new User
            {
                ID = Guid.NewGuid().ToString(),
                Username = username,
                Password = password,
                Archived = false
            };

            await _dbContext.Users.AddAsync(user);
            var saved = await _dbContext.SaveChangesAsync() > 0;

            return saved ? user.ID : null;
        }

        public async Task<User?> GetUserAsync(string id)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.ID == id);
        }

        public async Task<bool> UpdateUserAsync(
            string id,
            string username,
            string password,
            string? email,
            bool? archived)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.ID == id);

            if (user == null)
                return false;

            user.Username = username;
            user.Password = password;

            if (email != null)
                user.Email = email;

            if (archived.HasValue)
                user.Archived = archived.Value;

            user.UserLastChangeDate = DateTime.UtcNow;

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
