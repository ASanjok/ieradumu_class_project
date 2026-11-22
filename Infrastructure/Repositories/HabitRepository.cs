using Application.Interfaces;
using class_project_ieradumi;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class HabitRepository : IHabitRepository
    {
        private readonly DBContext _dbContext;
        public HabitRepository(DBContext context)
        {
            _dbContext = context;
        }

        public async Task<bool> AddHabitAsync(Habit habit)
        {
            if (habit == null)
                return false;

            await _dbContext.Habits.AddAsync(habit);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<Habit>> GetHabitsAsync(string userid)
        {
            return await _dbContext.Habits
                .Where(h => h.AuthorID == userid)
                .ToListAsync();
        }

        public async Task<Habit?> GetHabitAsync(string id)
        {
            return await _dbContext.Habits
                .FirstOrDefaultAsync(h => h.ID == id);
        }

        public async Task<bool> UpdateHabitAsync(
            string id,
            string habitname,
            string habitdescription,
            string authorid,
            bool archived)
        {
            var habit = await _dbContext.Habits
                .FirstOrDefaultAsync(h => h.ID == id);

            if (habit == null)
                return false;

            habit.HabitName = habitname;
            habit.HabitDescription = habitdescription;
            habit.AuthorID = authorid;
            habit.Archived = archived;
            habit.HabitLastChangeDate = DateTime.UtcNow;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteHabitAsync(string id)
        {
            var habit = await _dbContext.Habits
                .FirstOrDefaultAsync(h => h.ID == id);

            if (habit == null)
                return false;

            _dbContext.Habits.Remove(habit);
            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
