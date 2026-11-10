using System;
using System.Collections.Generic;
using System.Text;
using class_project_ieradumi;  

namespace maui_app_ieradumi_2.Services
{
    public interface IDataService
    {
        Task<bool> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string password);
        Task<List<Habit>> GetHabitList(string userIdTask);
    }
}
