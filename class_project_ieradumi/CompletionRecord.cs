using System;
using System.Collections.Generic;
using System.Text;

namespace class_project_ieradumi
{
    public class CompletionRecord
    {
        public string ID { get; set; }
        public string HabitID { get; set; }
        public Dictionary<string, TimeOnly> ScheduleTime { get; set; }
        public DateTime CompletionDate { get; set; }
        public string UserID { get; set; }
        public bool InTime { get; set; }

        public CompletionRecord()
        {

        }
        public CompletionRecord(string HabitID, DateTime CompletionDate, string UserID, bool InTime, string ID, Dictionary<string, TimeOnly> ScheduleTime) // Constructor for creating object of already existing completion_record
        {
            if (ID.Substring(0, 21) != "completion_record_ID_")
            {
                throw new ArgumentException("ID is not valid. It must start with 'completion_record_ID_<unique_id>'.");
            }
            this.ID = ID;
            this.HabitID = HabitID;
            this.CompletionDate = CompletionDate;
            this.UserID = UserID;
            this.InTime = InTime;
            this.ScheduleTime = ScheduleTime;
        }
        public CompletionRecord(string HabitID, string UserID, Dictionary<string, TimeOnly> ScheduleTime) // Constructor for creating new completion_record object
        {
            this.ID = Program.GetUniqueIDForCompletionRecord();
            this.HabitID = HabitID;
            this.CompletionDate = DateTime.Now;
            this.UserID = UserID;
            this.ScheduleTime = ScheduleTime;
            if (ScheduleTime.ContainsKey(CompletionDate.DayOfWeek.ToString()))
            {
                TimeOnly scheduled = ScheduleTime[CompletionDate.DayOfWeek.ToString()];
                TimeOnly completion_time = TimeOnly.FromDateTime(CompletionDate);
                this.InTime = completion_time <= scheduled;
            }
            else // maybe i will add check if the habit is done before the scheduled day
            {
                this.InTime = false; // If there is no scheduled time for the day, consider it as not in time
            }
        }
        public string GetID()
        {
            return this.ID;
        }
        public string GetHabitID()
        {
            return this.HabitID;
        }
        public DateTime GetCompletionDate()
        {
            return this.CompletionDate;
        }
        public string GetUserID()
        {
            return this.UserID;
        }
        public bool IsInTime()
        {
            return this.InTime;
        }

    }
}
