namespace ieradumu_class_project
{
    using System.Collections.Generic;
    public class program_data
    {
        public static List<habit> habit_list = new List<habit>();
        public static List<habit_record> habit_record_list = new List<habit_record>();
        public static List<schedule> schedule_list = new List<schedule>();
        public static List<user> user_list = new List<user>();
    }
    public class habit
    {
        private static int habit_counter = 0;
        private string habit_ID;
        private string creator_user_ID;
        private string habit_name;
        private string habit_description;
        private bool habit_status; // (Bool) 0|false - archived, 1|true - active
        private DateTime habit_created_at;
        private DateTime habit_updated_at;
        
        public habit(string habit_name, string habit_description,string creator_user_ID)
        {
            if(string.IsNullOrEmpty(habit_name))
            {
                throw new ArgumentException("Habit name cannot be null or empty.");
            }
            if(habit_name.Length > 40)
            {
                throw new ArgumentException("Habit name cannot exceed 40 characters.");
            }
            if (string.IsNullOrEmpty(creator_user_ID))
            {
                throw new ArgumentException("Creator user ID cannot be null or empty.");
            }
            habit_counter++;
            this.habit_ID = "habit_" + habit_counter.ToString();
            this.creator_user_ID = creator_user_ID;
            this.habit_name = habit_name;
            this.habit_description = habit_description;
            this.habit_status = true; // Active by default
            this.habit_created_at = DateTime.Now;
            this.habit_updated_at = DateTime.Now;
        }
        public void archive_habit()
        {
            this.habit_status = false;
            this.habit_updated_at = DateTime.Now;
        }
        public void change_habit(string new_habit_name, string new_habit_description)
        {
            if (!string.IsNullOrEmpty(new_habit_name))
            {
                this.habit_name = new_habit_name;
            }
            if(!string.IsNullOrEmpty(new_habit_description))
            {
                this.habit_description = new_habit_description;
            }
            if(string.IsNullOrEmpty(new_habit_name) && string.IsNullOrEmpty(new_habit_description))
            {
                throw new ArgumentException("habit hasnt been changed. both name and description are null or empty.");
            }
            this.habit_updated_at = DateTime.Now;
        }

        public string get_habit_ID()
        {
            return this.habit_ID;
        }
        public string get_habit_name()
        {
            return this.habit_name;
        }
        public string get_habit_description()
        {
            return this.habit_description;
        }
        public bool get_habit_status()
        {
            return this.habit_status;
        }
        public string get_habit_creator_user_ID()
        {
            return this.creator_user_ID;
        }
        public string get_habit_creation_date()
        {
            return this.habit_created_at.ToString();
        }
        public string get_habit_last_updated_date()
        {
            return this.habit_updated_at.ToString();
        }

    }

    public class habit_record
    {
        private static int habit_record_counter = 0;
        private string habit_record_ID;
        private string user_ID;
        private string habit_ID;
        private DateTime habit_record_done_at;

        public habit_record(string user_ID, string habit_ID)
        {
            if(string.IsNullOrEmpty(user_ID))
            {
                throw new ArgumentException("User ID cannot be null or empty.");
            }
            if(string.IsNullOrEmpty(habit_ID))
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
            if(program_data.habit_record_list.Exists(hr => hr.user_ID == user_ID && hr.habit_ID == habit_ID && hr.habit_record_done_at.Date == DateTime.Now.Date))
            {
                throw new ArgumentException("Habit record for this habit and user already exists for today.");
            }
            if(program_data.habit_list.Exists(h => h.get_habit_ID() == habit_ID && h.get_habit_status() == false))
            {
                throw new ArgumentException("Habit is archived and cannot be recorded.");
            }
            if (!program_data.schedule_list.Exists(s => s.get_user_ID() == user_ID && s.is_scheduled_for_today(habit_ID)))
            {
                throw new ArgumentException("Habit is not scheduled for today.");
            }
            habit_record_counter++;
            this.habit_record_ID = "habit_record_" + user_ID + "_" + habit_ID+"_ID";
            this.user_ID = user_ID;
            this.habit_ID = habit_ID;
            this.habit_record_done_at = DateTime.Now;
        }

    }

    public class user
    {
        private static int user_counter = 0;
        private string user_ID;
        private string user_name;
        private string user_email;
        private string user_password;
        private DateTime user_created_at;
        private DateTime user_updated_at;
        public user(string user_name, string user_email, string user_password)
        {
            user_counter++;
            this.user_ID = "user_" + user_counter.ToString();
            this.user_name = user_name;
            this.user_email = user_email;
            this.user_password = user_password;
            this.user_created_at = DateTime.Now;
            this.user_updated_at = DateTime.Now;
        }
        public string get_user_ID()
        {
            return this.user_ID;
        }
    }

    public struct habit_schedule
    {
        public string habit_ID;
        public bool[] is_active_on_weekday;
    }

    public class schedule
    {
        private static int schedule_counter = 0;
        private string schedule_ID;
        private string user_ID;
        private List<habit_schedule> habit_schedules;
        public schedule(string user_ID, string habit_ID, bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday)
        {
            schedule_counter++;
            this.schedule_ID = user_ID + "_schedule_" + schedule_counter.ToString()+"_ID";
            this.user_ID = user_ID;
            this.habit_schedules = new List<habit_schedule>();
            this.habit_schedules.Add(new habit_schedule
            {
                habit_ID = habit_ID,
                is_active_on_weekday = new bool[] { sunday, monday, tuesday, wednesday, thursday, friday, saturday },
            });
        }
        public schedule(string user_ID)
        {
            schedule_counter++;
            this.schedule_ID = user_ID + "_schedule_" + schedule_counter.ToString()+"_ID";
            this.user_ID = user_ID;
            this.habit_schedules = new List<habit_schedule>();
        }
        public string get_user_ID()
        {
            return this.user_ID;
        }
        public bool has_habbit(string habit_ID)
        {
            return this.habit_schedules.Exists(h => h.habit_ID == habit_ID);
        }
        public bool is_scheduled_for_today(string habit_ID)
        {
            var habit_schedule = this.habit_schedules.Find(h => h.habit_ID == habit_ID);
            if (habit_schedule.habit_ID == null)
            {
                throw new ArgumentException("Habit ID does not exist in the schedule.");
            }
            return habit_schedule.is_active_on_weekday[(int)DateTime.Now.DayOfWeek];
        }

    }
}
