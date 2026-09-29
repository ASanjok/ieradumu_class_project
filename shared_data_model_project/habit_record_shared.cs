using System;
using System.Collections.Generic;
using System.Text;

namespace shared_data_model_project
{
    public class mark_habit_as_done
    {
        public string user_ID { get; set; }
        public string habit_ID { get; set; }
    }
    public class unmark_habit_as_done
    {
        public string user_ID { get; set; }
        public string habit_ID { get; set; }
    }
    public class show_habit_record
    {
        public string habit_ID { get; set; }
        public DateTime habit_record_done_at { get; set; }
    }
    public class show_habit_records_for_user
    {
        public List<show_habit_record> habit_records { get; set; }
    }
}
