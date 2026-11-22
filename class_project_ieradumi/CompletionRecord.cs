using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json;

namespace class_project_ieradumi
{
    public class CompletionRecord
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string HabitID { get; set; }
        public string UserID { get; set; }
        public DateTime CompletionDate { get; set; } // дата выполнения привычки
        public bool InTime { get; set; } // выполнена ли привычка в срок
        public bool HasTheRecordBeenUsed { get; set; } // прошла ли дата этой привычки, либо была ли она использована
        [Ignore]
        [NotMapped]
        public Dictionary<string, TimeOnly> ScheduleTime { get; set; }
        public string ScheduleTimeJSON
        {
            get => JsonSerializer.Serialize(ScheduleTime);
            set => ScheduleTime = string.IsNullOrEmpty(value)
               ? new Dictionary<string, TimeOnly>()
               : JsonSerializer.Deserialize<Dictionary<string, TimeOnly>>(value) ?? new Dictionary<string, TimeOnly>();
        }


        public CompletionRecord()
        {
        }
        public CompletionRecord(string HabitID, DateTime CompletionDate, string UserID, bool InTime, string ID, Dictionary<string, TimeOnly> ScheduleTime) // Constructor for creating object of already existing completion_record
        {
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
    }
}
