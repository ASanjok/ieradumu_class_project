using System;
using System.Collections.Generic;
using System.Text;

namespace shared_data_model_project

{
    public class habit_schedule // class that represents structure from schedule class,
    {
        public string habit_ID { get; set; }
        public bool[] is_active_on_weekday { get; set; }
    }
    public class get_schedule_for_user
    {
        public string user_ID { get; set; }
        public List<habit_schedule> habit_schedules { get; set; }
    }
    public class create_schedule
    {
        public string user_ID { get; set; }
        public List<habit_schedule> habit_schedules { get; set; }
    }
    public class update_schedule
    {
        public string user_ID { get; set; }
        public List<habit_schedule> habit_schedules { get; set; }
    }
    
}
