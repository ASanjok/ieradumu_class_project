using System;
using System.Collections.Generic;
using System.Text;

namespace shared_object_ieradumi
{
    internal class shared_schedule
    {
        public class shared_scheduled_habit_dto
        {
            public string habit_ID { get; set; }
            public Dictionary<string, List<TimeOnly>> when_is_scheduled { get; set; }
            public bool archived { get; set; }
        }

        public class create_schedule
        {
            public string user_ID { get; set; }
            public string habit_ID { get; set; }
            public List<TimeOnly> monday { get; set; }
            public List<TimeOnly> tuesday { get; set; }
            public List<TimeOnly> wednesday { get; set; }
            public List<TimeOnly> thursday { get; set; }
            public List<TimeOnly> friday { get; set; }
            public List<TimeOnly> saturday { get; set; }
            public List<TimeOnly> sunday { get; set; }
        }

        public class show_full_schedule
        {
            public string ID { get; set; }
            public string user_ID { get; set; }
            public bool archived { get; set; }
            public List<shared_scheduled_habit_dto> scheduled_habits { get; set; }
            public DateTime schedule_creation_date { get; set; }
            public DateTime schedule_last_change_date { get; set; }
        }

        public class show_partial_schedule
        {
            public string ID { get; set; }
            public string user_ID { get; set; }
            public bool archived { get; set; }
            public DateTime schedule_last_change_date { get; set; }
        }

        public class add_scheduled_habit
        {
            public string schedule_ID { get; set; }
            public string habit_ID { get; set; }
            public List<TimeOnly> monday { get; set; }
            public List<TimeOnly> tuesday { get; set; }
            public List<TimeOnly> wednesday { get; set; }
            public List<TimeOnly> thursday { get; set; }
            public List<TimeOnly> friday { get; set; }
            public List<TimeOnly> saturday { get; set; }
            public List<TimeOnly> sunday { get; set; }
        }

        public class modify_habit_time
        {
            public string schedule_ID { get; set; }
            public string habit_ID { get; set; }
            public string day_of_week { get; set; } // monday, tuesday, wednesday, thursday, friday, saturday, sunday
            public List<TimeOnly> time { get; set; }
        }

        public class remove_habit_from_schedule
        {
            public string schedule_ID { get; set; }
            public string habit_ID { get; set; }
        }
    }
}
