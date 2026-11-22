using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly DBContext _dbContext;
        public ScheduleRepository(DBContext context)
        {
            _dbContext = context;
        }
        public async Task<Schedule> GetScheduleAsync(string userid)
        {
            var schedule = await _dbContext.Schedules
                .FirstOrDefaultAsync(s => s.UserID == userid);

            if (schedule == null)
                return null!;

            schedule.HabitSchedules = await _dbContext.HabitSchedules
                .Where(hs => hs.ScheduleID == schedule.ID && hs.DeletedAt == null)
                .ToListAsync();

            return schedule;
        }

        public async Task<bool> SetScheduleAsync(Schedule schedule)
        {
            if (schedule == null)
                return false;

            await _dbContext.Schedules.AddAsync(schedule);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateScheduleAsync(Schedule schedule)
        {
            if (schedule == null)
                return false;

            _dbContext.Schedules.Update(schedule);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
