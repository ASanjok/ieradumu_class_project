using class_project_ieradumi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<string?> LoginAsync(string username, string password);
        Task<string?> RegisterAsync(string username, string password);
        Task<User?> GetUserAsync(string id);
        Task<bool> UpdateUserAsync(string id, string username, string password, string? email, bool? archived);
    }
}
