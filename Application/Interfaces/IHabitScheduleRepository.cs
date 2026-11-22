using class_project_ieradumi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IHabitScheduleRepository
    {
        Task<bool> SetHabitScheduleAsync(HabitSchedule habitschedule);
        Task<bool> SoftDeleteHabitScheduleAsync(string habitscheduleid);
    }
}
