using System;
using System.Collections.Generic;
using System.Text;

namespace shared_object_ieradumi
{
    internal class shared_habit
    {
        public class create_new_habit
        {
            public string habit_name { get; set; }
            public string habit_description { get; set; }
            public string author_ID { get; set; }
        }
        public class show_full_habit
        {
            public string habit_ID { get; set; }
            public string habit_name { get; set; }
            public string habit_description { get; set; }
            public string author_ID { get; set; }
            public bool archived { get; set; } // (Bool) 0|false - active, 1|true - archived
            public DateTime habit_creation_date { get; set; }
            public DateTime habit_last_change_date { get; set; }
        }
        public class show_partial_habit
        {
            public string habit_ID { get; set; }
            public string habit_name { get; set; }
            public bool archived { get; set; } // (Bool) 0|false - active, 1|true - archived
            public DateTime habit_creation_date { get; set; }
        }
        public class update_habit
        {
            public string habit_ID { get; set; }
            public string habit_name { get; set; }
            public string habit_description { get; set; }
        }
    }
}
