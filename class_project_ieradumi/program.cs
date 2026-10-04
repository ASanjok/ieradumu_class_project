namespace class_project_ieradumi
{
    public class program
    {
        static List<habit> habits_list;
        static List<user> users_list;
        static List<schedule> schedule_list;
        static public string get_unique_ID_for_habit()
        {
            for (int i = 0; ; i++) // Limit the number of attempts to avoid infinite loops
            {
                string new_ID = " habit_ID_" + i;
                bool ID_exists = habits_list.Any(habit => habit.get_unique_ID() == new_ID);
                if (!ID_exists)
                {
                    return new_ID;
                }
            }
        }
        static public string get_unique_ID_for_user()
        {
            for (int i = 0; ; i++) // Limit the number of attempts to avoid infinite loops
            {
                string new_ID = "user_ID_" + i;
                bool ID_exists = users_list.Any(user => user.get_ID() == new_ID);
                if (!ID_exists)
                {
                    return new_ID;
                }
            }
        }
        static public string get_unique_ID_for_schedule()
        {
            for (int i = 0; ; i++) // Limit the number of attempts to avoid infinite loops
            {
                string new_ID = "schedule_ID_" + i;
                bool ID_exists = schedule_list.Any(schedule => schedule.get_ID() == new_ID);
                if (!ID_exists)
                {
                    return new_ID;
                }
            }
        }
        static public string get_unique_ID_for_completion_record()
        {
            for (int i = 0; ; i++) // Limit the number of attempts to avoid infinite loops
            {
                string new_ID = "completion_record_ID_" + i;
                bool ID_exists = false; // Initialize to false
                if (!ID_exists)
                {
                    return new_ID;
                }
            }
        }
    }
}
