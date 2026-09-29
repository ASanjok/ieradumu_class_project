using System;
using System.Collections.Generic;
using System.Text;


//THIS CLASS MAY BE REMOVED
//and if that is the case, most likely schedule data will be implemented in user class


namespace ieradumu_class_project
{
    public struct habit_schedule //struct for habit schedule
    {
        public string habit_ID;
        public bool[] is_active_on_weekday;
    }

    public class schedule
    {
        private static int schedule_counter = 0; // static counter to keep track of the number of schedules created
                                                 // a temporary solution for unique IDs
        private string schedule_ID;
        private string user_ID;
        private List<habit_schedule> habit_schedules;
        public schedule(string user_ID, string habit_ID, bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday) //constructor for creating a new schedule
        {
            if (program_data.schedule_list.Find(s => s.get_user_ID() == user_ID) == null) // if the user does not have a schedule yet, create a new schedule
            {
                schedule_counter++;
                this.schedule_ID = user_ID + "_schedule_" + schedule_counter + "_ID";
                this.user_ID = user_ID;
                this.habit_schedules = new List<habit_schedule>();
                this.habit_schedules.Add(new habit_schedule
                {
                    habit_ID = habit_ID,
                    is_active_on_weekday = new bool[] { sunday, monday, tuesday, wednesday, thursday, friday, saturday },
                });
            }
            else // if the user already has a schedule, add the new habit to the existing schedule
            {
                var existing_schedule = program_data.schedule_list.Find(s => s.get_user_ID() == user_ID);
                this.schedule_ID = existing_schedule.schedule_ID;
                this.user_ID = existing_schedule.user_ID;
                this.habit_schedules = existing_schedule.habit_schedules;
                if (this.habit_schedules.Exists(h => h.habit_ID == habit_ID))
                {
                    throw new ArgumentException("Habit ID already exists in the schedule.");
                }
                this.habit_schedules.Add(new habit_schedule
                {
                    habit_ID = habit_ID,
                    is_active_on_weekday = new bool[] { sunday, monday, tuesday, wednesday, thursday, friday, saturday },
                });
            }
        }
        public schedule(string schedule_ID, string user_ID, List<habit_schedule> habit_schedules) //constructor for loading schedule data from "DB"
        {
            this.schedule_ID = schedule_ID;
            this.user_ID = user_ID;
            this.habit_schedules = habit_schedules;
        }
        public string get_user_ID()
        {
            return this.user_ID;
        }
        public bool has_habbit(string habit_ID) // returns true if the habit is in the schedule, false otherwise
        {
            return this.habit_schedules.Exists(h => h.habit_ID == habit_ID);
        }
        public bool is_scheduled_for_today(string habit_ID) // returns true if the habit is scheduled for today, false otherwise
        {
            var habit_schedule = this.habit_schedules.Find(h => h.habit_ID == habit_ID);
            if (habit_schedule.habit_ID == null)
            {
                return false;
            }
            return habit_schedule.is_active_on_weekday[(int)DateTime.Now.DayOfWeek];
        }
        public string get_schedule_data()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Schedule ID: {this.schedule_ID}, User ID: {this.user_ID}");
            sb.AppendLine($"\t\t\t\t\t\t\tsun,\tmon,\ttue,\twed,\tthu,\tfri,\tsat");
            foreach (var habit_schedule in this.habit_schedules)
            {
                sb.AppendLine($"\tHabit ID: {habit_schedule.habit_ID}, Active on Weekdays:\t{string.Join(",\t", habit_schedule.is_active_on_weekday)}");
            }
            return sb.ToString();
        }
    }
}
