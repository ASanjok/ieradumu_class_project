using System;
using System.Collections.Generic;
using System.Text;

namespace shared_object_ieradumi
{
    public class SharedHabitSchedule
    {
        public class FullHabitScheduleData
        {
            public string ID { get; set; } 
            public string ScheduleID { get; set; }
            public string HabitID { get; set; }
            public string DayOfWeek { get; set; }
            public TimeSpan ScheduledTime { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? DeletedAt { get; set; }
            public bool IsActive{ get; set; }
        }
    }
}
