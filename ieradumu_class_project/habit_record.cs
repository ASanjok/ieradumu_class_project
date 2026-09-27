using System;
using System.Collections.Generic;
using System.Text;

namespace ieradumu_class_project
{
    public class habit_record
    {
        private static int habit_record_counter = 0;
        private string habit_record_ID;
        private string user_ID;
        private string habit_ID;
        private DateTime habit_record_done_at;

        public habit_record(string user_ID, string habit_ID)
        {
            if (string.IsNullOrEmpty(user_ID))
            {
                throw new ArgumentException("User ID cannot be null or empty.");
            }
            if (string.IsNullOrEmpty(habit_ID))
            {
                throw new ArgumentException("Habit ID cannot be null or empty.");
            }
            if (program_data.user_list.Find(u => u.get_user_ID() == user_ID) == null)
            {
                throw new ArgumentException("User ID does not exist.");
            }
            if (program_data.habit_list.Find(h => h.get_habit_ID() == habit_ID) == null)
            {
                throw new ArgumentException("Habit ID does not exist.");
            }
            if (program_data.habit_record_list.Exists(hr => hr.user_ID == user_ID && hr.habit_ID == habit_ID && hr.habit_record_done_at.Date == DateTime.Now.Date))
            {
                throw new ArgumentException("Habit record for this habit and user already exists for today.");
            }
            if (program_data.habit_list.Exists(h => h.get_habit_ID() == habit_ID && h.get_habit_status() == false))
            {
                throw new ArgumentException("Habit is archived and cannot be recorded.");
            }
            if (!program_data.schedule_list.Exists(s => s.get_user_ID() == user_ID && s.is_scheduled_for_today(habit_ID)))
            {
                throw new ArgumentException("Habit is not scheduled for today.");
            }
            habit_record_counter++;
            this.habit_record_ID = "habit_record_" + user_ID + "_" + habit_ID + "_ID";
            this.user_ID = user_ID;
            this.habit_ID = habit_ID;
            this.habit_record_done_at = DateTime.Now;
        }

    }

}
