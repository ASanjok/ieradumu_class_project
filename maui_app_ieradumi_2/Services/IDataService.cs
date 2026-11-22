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
        Task SaveHabitAsync(string habitName, string habitDescription, bool archived = false);
        Task EditHabitAsync(string ID,Habit habit);
        Task<Habit?> GetHabitAsync(string habitId);
        Task DeleteHabitAsync(string habitId);
        Task<User?> GetUserByIdAsync(string userId);
        Task UpdateUserAsync(User user);
        Task<Schedule?> GetScheduleByUserIdAsync(string userId);
        Task SoftDeleteHabitScheduleAsync(string scheduleId, string habitId, string dayOfWeek, TimeSpan scheduledTime);
        Task UpdateScheduleAsync(Schedule schedule);
        Task<List<CompletionRecord>> GetCompletionsByUserAndDateAsync(string userId, DateTime date);
        Task AddCompletionAsync(string habitId, string userId, DateTime completionDate, bool inTime, bool hasTheRecordBeenUsed, Dictionary<string, TimeOnly> scheduleTime);
    }
}
