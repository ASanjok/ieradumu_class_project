using Application.Interfaces;
using class_project_ieradumi;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class HabitScheduleRepository : IHabitScheduleRepository
    {
        private readonly DBContext _dbContext;
        public HabitScheduleRepository(DBContext context)
        {
            _dbContext = context;
        }
        public async Task<bool> SetHabitScheduleAsync(HabitSchedule habitschedule)
        {
            if (habitschedule == null)
                return false;

            await _dbContext.HabitSchedules.AddAsync(habitschedule);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteHabitScheduleAsync(string habitscheduleid)
        {
            var item = await _dbContext.HabitSchedules
                .FirstOrDefaultAsync(hs => hs.ID == habitscheduleid);

            if (item == null || item.DeletedAt != null)
                return false;

            item.DeletedAt = DateTime.UtcNow;
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
