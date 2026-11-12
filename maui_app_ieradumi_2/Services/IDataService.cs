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
        Task<List<Habit>> GetHabitListAsync(string userIdTask);
        Task SaveHabitAsync(string habitName, string habitDescription);
        Task SaveHabitAsync(Habit habit);
        Task EditHabitAsync(string ID,Habit habit);
        Task<Habit?> GetHabitAsync(string habitId);

    }
}
