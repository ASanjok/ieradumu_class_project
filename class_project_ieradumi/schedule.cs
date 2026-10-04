using System;
using System.Collections.Generic;
using System.Text;

namespace class_project_ieradumi
{
    internal class scheduled_habits
    {
        private string habit_ID;
        private Dictionary<string, List<TimeOnly>> when_is_scheduled;
        private bool archived;
        public scheduled_habits(string habit_ID, List<TimeOnly> monday, List<TimeOnly> tuesday, List<TimeOnly> wednesday, List<TimeOnly> thursday, List<TimeOnly> friday, List<TimeOnly> saturday, List<TimeOnly> sunday)
        {
            this.habit_ID = habit_ID;
            this.when_is_scheduled = new Dictionary<string, List<TimeOnly>>();
            this.when_is_scheduled.Add("Monday", monday);
            this.when_is_scheduled.Add("Tuesday", tuesday);
            this.when_is_scheduled.Add("Wednesday", wednesday);
            this.when_is_scheduled.Add("Thursday", thursday);
            this.when_is_scheduled.Add("Friday", friday);
            this.when_is_scheduled.Add("Saturday", saturday);
            this.when_is_scheduled.Add("Sunday", sunday);
            this.archived = false;
        }

        // Getters for scheduled_habits class
        public string get_habit_ID()
        {
            return this.habit_ID;
        }
        public Dictionary<string, List<TimeOnly>> get_when_is_scheduled()
        {
            return this.when_is_scheduled;
        }
        public bool is_archived()
        {
            return this.archived;
        }

        //setters for scheduled_habits class
        public void set_archived()
        {
            this.archived = true;
        }
        public void set_unarchived()
        {
            this.archived = false;
        }


        //add methods for scheduled_habits class

        public void add_scheduled_time(string day_of_week, List<TimeOnly> time)
        {
            if (this.when_is_scheduled.ContainsKey(day_of_week))
            {
                this.when_is_scheduled[day_of_week].AddRange(time);
                this.when_is_scheduled[day_of_week] = this.when_is_scheduled[day_of_week].Distinct().ToList(); // Remove duplicates
            }
            else
            {
                throw new ArgumentException("Invalid day of the week.");
            }
        }


        //delete methods for scheduled_habits class
        public void remove_scheduled_time(string day_of_week, List<TimeOnly> time)
        {
            if (this.when_is_scheduled.ContainsKey(day_of_week))
            {
                foreach (var t in time)
                {
                    this.when_is_scheduled[day_of_week].Remove(t);
                }
            }
            else
            {
                throw new ArgumentException("Invalid day of the week.");
            }
        }

    }





    internal class schedule
    {
        private string ID;
        private string user_ID;
        private bool archived;
        private List<scheduled_habits> scheduled_habits;
        private DateTime schedule_creation_date;
        private DateTime schedule_last_change_date;

        public schedule(string ID, string user_ID, bool archived, List<scheduled_habits> scheduled_habits, DateTime schedule_creation_date, DateTime schedule_last_change_date) // Constructor for creating object of already existing schedule
        {
            string start_of_ID = ID.Substring(0, 12);
            if (start_of_ID != "schedule_ID_")
            {
                throw new ArgumentException("ID is not valid. It must start with 'schedule_ID_<unique_id>'.");
            }
            this.ID = ID;
            this.user_ID = user_ID;
            this.archived = archived;
            this.scheduled_habits = scheduled_habits;
            this.schedule_creation_date = schedule_creation_date;
            this.schedule_last_change_date = schedule_last_change_date;
        }
        public schedule(string user_ID,string habit_ID, List<TimeOnly> monday, List<TimeOnly> tuesday, List<TimeOnly> wednesday, List<TimeOnly> thursday, List<TimeOnly> friday, List<TimeOnly> saturday, List<TimeOnly> sunday) // Constructor for creating new schedule object
        {
            this.ID = program.get_unique_ID_for_schedule();
            this.user_ID = user_ID;
            this.archived = false;
            this.scheduled_habits = new List<scheduled_habits>
            {
                new scheduled_habits(habit_ID, monday, tuesday, wednesday, thursday, friday, saturday, sunday)
            };
            this.schedule_creation_date = DateTime.Now;
            this.schedule_last_change_date = DateTime.Now;
        }

        // Getters for schedule class
        public string get_ID()
        {
            return this.ID;
        }
        public string get_user_ID()
        {
            return this.user_ID;
        }
        public bool is_archived()
        {
            return this.archived;
        }



        // setters for schedule class
        public void set_archived()
        {
            this.archived = true;
            this.schedule_last_change_date = DateTime.Now;
        }
        public void set_unarchived()
        {
            this.archived = false;
            this.schedule_last_change_date = DateTime.Now;
        }


        //add methods for schedule class
        public void add_scheduled_habit(string habit_ID, List<TimeOnly> monday, List<TimeOnly> tuesday, List<TimeOnly> wednesday, List<TimeOnly> thursday, List<TimeOnly> friday, List<TimeOnly> saturday, List<TimeOnly> sunday)
        {
            if (habit_ID.Substring(0, 9) != "habit_ID_")
            {
                throw new ArgumentException("Habit ID is not valid. It must start with 'habit_ID_<unique_id>'.");
            }
            if (this.scheduled_habits.Exists(habit => habit.get_habit_ID() == habit_ID))
            {
                this.scheduled_habits.Where(habit => habit.get_habit_ID() == habit_ID).First().add_scheduled_time("Monday", monday);
                this.scheduled_habits.Where(habit => habit.get_habit_ID() == habit_ID).First().add_scheduled_time("Tuesday", tuesday);
                this.scheduled_habits.Where(habit => habit.get_habit_ID() == habit_ID).First().add_scheduled_time("Wednesday", wednesday);
                this.scheduled_habits.Where(habit => habit.get_habit_ID() == habit_ID).First().add_scheduled_time("Thursday", thursday);
                this.scheduled_habits.Where(habit => habit.get_habit_ID() == habit_ID).First().add_scheduled_time("Friday", friday);
                this.scheduled_habits.Where(habit => habit.get_habit_ID() == habit_ID).First().add_scheduled_time("Saturday", saturday);
                this.scheduled_habits.Where(habit => habit.get_habit_ID() == habit_ID).First().add_scheduled_time("Sunday", sunday);
                this.schedule_last_change_date = DateTime.Now;
            }
            else
            {
                this.scheduled_habits.Add(new scheduled_habits(habit_ID, monday, tuesday, wednesday, thursday, friday, saturday, sunday));
                this.schedule_last_change_date = DateTime.Now;
            }
        }
        public void add_time_to_scheduled_habit(string habit_ID, string day_of_week, List<TimeOnly> time)
        {
            if (habit_ID.Substring(0, 9) != "habit_ID_")
            {
                throw new ArgumentException("Habit ID is not valid. It must start with 'habit_ID_<unique_id>'.");
            }
            if (this.scheduled_habits.Exists(habit => habit.get_habit_ID() == habit_ID))
            {
                this.scheduled_habits.Where(habit => habit.get_habit_ID() == habit_ID).First().add_scheduled_time(day_of_week, time);
                this.schedule_last_change_date = DateTime.Now;
            }
            else
            {
                throw new ArgumentException("Habit ID does not exist in the schedule.");
            }
        }

        //delete methods for schedule class
        public void remove_time_from_scheduled_habit(string habit_ID, string day_of_week, List<TimeOnly> time)
        {
            if (habit_ID.Substring(0, 9) != "habit_ID_")
            {
                throw new ArgumentException("Habit ID is not valid. It must start with 'habit_ID_<unique_id>'.");
            }
            if (this.scheduled_habits.Exists(habit => habit.get_habit_ID() == habit_ID))
            {
                this.scheduled_habits.Where(habit => habit.get_habit_ID() == habit_ID).First().remove_scheduled_time(day_of_week, time);
                this.schedule_last_change_date = DateTime.Now;
            }
            else
            {
                throw new ArgumentException("Habit ID does not exist in the schedule.");
            }
        }
        public void remove_scheduled_habit(string habit_ID)
        {
            if (habit_ID.Substring(0, 9) != "habit_ID_")
            {
                throw new ArgumentException("Habit ID is not valid. It must start with 'habit_ID_<unique_id>'.");
            }
            if (this.scheduled_habits.Exists(habit => habit.get_habit_ID() == habit_ID))
            {
                this.scheduled_habits.RemoveAll(habit => habit.get_habit_ID() == habit_ID);
                this.schedule_last_change_date = DateTime.Now;
            }
            else
            {
                throw new ArgumentException("Habit ID does not exist in the schedule.");
            }
        }
    }
}
