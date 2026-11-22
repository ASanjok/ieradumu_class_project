using System;
using System.Collections.Generic;
using System.Text;
using class_project_ieradumi;

namespace Application.Interfaces
{
    public interface IHabitRepository
    {
        Task<bool> AddHabitAsync(Habit habit);
        Task<List<Habit>> GetHabitsAsync(string userid);
        Task<Habit?> GetHabitAsync(string id);
        Task<bool> UpdateHabitAsync(string id, string habitname, string habitdescription, string authorid, bool archived);
        Task<bool> DeleteHabitAsync(string id);
    }
}
