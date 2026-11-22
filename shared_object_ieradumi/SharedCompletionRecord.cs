using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json;

namespace shared_object_ieradumi
{
    public class SharedCompletionRecord
    {
        //public class mark_habit_as_done
        //{
        //    public string user_ID { get; set; }
        //    public string habit_ID { get; set; }
        //    public Dictionary<string, TimeOnly> scheduled_time { get; set; } // This is needed to determine if the habit was done in time or not
        //}
        //public class unmark_habit_as_done
        //{
        //    public string user_ID { get; set; }
        //    public string completion_record_ID { get; set; }
        //}
        //public class show_habit_record
        //{
        //    public string ID { get; set; }
        //    public string habit_ID { get; set; }
        //    public string user_ID { get; set; }
        //    public bool in_time { get; set; }
        //    public DateTime habit_record_done_at { get; set; }
        //}
        //public class show_habit_records_for_user
        //{
        //    public List<show_habit_record> habit_records { get; set; }
        //}

        public class FullCompletionRecordData
        {
            public string ID { get; set; }
            public string HabitID { get; set; }
            public string UserID { get; set; }
            public DateTime CompletionDate { get; set; } 
            public bool InTime { get; set; } 
            public bool HasTheRecordBeenUsed { get; set; } 
            public Dictionary<string, TimeOnly> ScheduleTime { get; set; }
        }
    }
}
