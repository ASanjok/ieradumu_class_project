using System;
using System.Collections.Generic;
using System.Text;

namespace shared_data_model_project
{
    public class create_new_habit
    {
        public string habit_name { get; set; }
        public string habit_description { get; set; }
        public string creator_user_ID { get; set; }
    }
    public class show_full_habit
    {
        public string habit_ID { get; set; }
        public string habit_name { get; set; }
        public string habit_description { get; set; }
        public string creator_user_ID { get; set; }
        public bool habit_status { get; set; } // (Bool) 0|false - archived, 1|true - active
        public DateTime habit_created_at { get; set; }
    }
    public class show_partial_habit
    {
        public string habit_ID { get; set; }
        public string habit_name { get; set; }
        public bool habit_status { get; set; } // (Bool) 0|false - archived, 1|true - active
        public DateTime habit_created_at { get; set; }
    }
    public class update_habit 
    {
        public string habit_ID { get; set; }
        public string habit_name { get; set; }
        public string habit_description { get; set; }
    }
    public class delete_habit
    {
        public string habit_ID { get; set; }
        public string user_ID { get; set; }
    }
}