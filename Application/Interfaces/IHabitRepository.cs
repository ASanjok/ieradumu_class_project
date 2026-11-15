using System;
using System.Collections.Generic;
using System.Text;
using class_project_ieradumi;

namespace Application.Interfaces
{
    public interface IHabitRepository
    {
        Task<List<Habit>> GetAllAsync(string userId);
        Task<Habit?> GetByIdAsync(string id);
        Task AddAsync(Habit habit);
        Task UpdateAsync(Habit habit);
        Task ArchiveAsync(string id);
    }
}
