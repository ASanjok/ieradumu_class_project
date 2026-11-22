using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IScheduleRepository
    {
        Task<Schedule> GetScheduleAsync(string userid);
        Task<bool> SetScheduleAsync(Schedule schedule);
        Task<bool> UpdateScheduleAsync(Schedule schedule);
    }
}
