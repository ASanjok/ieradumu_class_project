using System;
using System.Collections.Generic;
using System.Text;

namespace class_project_ieradumi
{
    internal class completion_record
    {
        private string ID;
        private string habit_ID;
        private Dictionary<string, TimeOnly> scheduled_time;
        private DateTime completion_date;
        private string user_ID;
        private bool in_time;


        public completion_record(string habit_ID, DateTime completion_date, string user_ID, bool in_time, string ID, Dictionary<string, TimeOnly> scheduled_time) // Constructor for creating object of already existing completion_record
        {
            if(ID.Substring(0, 21) != "completion_record_ID_")
            {
                throw new ArgumentException("ID is not valid. It must start with 'completion_record_ID_<unique_id>'.");
            }
            this.ID = ID;
            this.habit_ID = habit_ID;
            this.completion_date = completion_date;
            this.user_ID = user_ID;
            this.in_time = in_time;
            this.scheduled_time = scheduled_time;
        }
        public completion_record(string habit_ID, string user_ID, Dictionary<string, TimeOnly> scheduled_time) // Constructor for creating new completion_record object
        {
            this.ID = program.get_unique_ID_for_completion_record();
            this.habit_ID = habit_ID;
            this.completion_date = DateTime.Now;
            this.user_ID = user_ID;
            this.scheduled_time = scheduled_time;
            if (scheduled_time.ContainsKey(completion_date.DayOfWeek.ToString()))
            {
                TimeOnly scheduled = scheduled_time[completion_date.DayOfWeek.ToString()];
                TimeOnly completion_time = TimeOnly.FromDateTime(completion_date);
                this.in_time = completion_time <= scheduled;
            }
            else // maybe i will add check if the habit is done before the scheduled day
            {
                this.in_time = false; // If there is no scheduled time for the day, consider it as not in time
            }
        }
        public string get_ID()
        {
            return this.ID;
        }
        public string get_habit_ID()
        {
            return this.habit_ID;
        }
        public DateTime get_completion_date()
        {
            return this.completion_date;
        }
        public string get_user_ID()
        {
            return this.user_ID;
        }
        public bool is_in_time()
        {
            return this.in_time;
        }

    }
}
