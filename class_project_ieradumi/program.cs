namespace class_project_ieradumi
{
    public class Program
    {
        static List<Habit> habits_list = new List<Habit>();
        static List<User> users_list = new List<User>();
        static List<Schedule> schedule_list = new List<Schedule>();
        static List<CompletionRecord> completion_record_list = new List<CompletionRecord>();
        static public string GetUniqueIdForHabit()
        {
            for (int i = 0; ; i++) // Limit the number of attempts to avoid infinite loops
            {
                string new_ID = " habit_ID_" + i;
                bool ID_exists = habits_list.Any(habit => habit.GetUniqueID() == new_ID);
                if (!ID_exists)
                {
                    return new_ID;
                }
            }
        }
        static public string GetUniqueIDForUser()
        {
            for (int i = 0; ; i++) // Limit the number of attempts to avoid infinite loops
            {
                string new_ID = "user_ID_" + i;
                bool ID_exists = users_list.Any(user => user.GetID() == new_ID);
                if (!ID_exists)
                {
                    return new_ID;
                }
            }
        }
        static public string GetUniqueIDForSchedule()
        {
            for (int i = 0; ; i++) // Limit the number of attempts to avoid infinite loops
            {
                string new_ID = "schedule_ID_" + i;
                bool ID_exists = schedule_list.Any(schedule => schedule.GetID() == new_ID);
                if (!ID_exists)
                {
                    return new_ID;
                }
            }
        }
        static public string GetUniqueIDForCompletionRecord()
        {
            for (int i = 0; ; i++) // Limit the number of attempts to avoid infinite loops
            {
                string new_ID = "completion_record_ID_" + i;
                bool ID_exists = completion_record_list.Any(record => record.GetID() == new_ID); // Initialize to false
                if (!ID_exists)
                {
                    return new_ID;
                }
            }
        }
    }
}
