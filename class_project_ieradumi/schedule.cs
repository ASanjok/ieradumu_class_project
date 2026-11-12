using SQLite;
using System.Text.Json;

namespace class_project_ieradumi
{
    public class ScheduledHabits
    {
        private string _habitID;
        public string HabitID
        {
            get => _habitID;
            set => _habitID = "habit_schedule_ID_" + value;
        }

        private Dictionary<string, List<TimeOnly>> _whenIsScheduled;
        [Ignore]
        public Dictionary<string, List<TimeOnly>> WhenIsScheduled
        {
            get => _whenIsScheduled;
            set => _whenIsScheduled = value;
        }
        public string WhenIsScheduledJSON
        {
            get => JsonSerializer.Serialize(_whenIsScheduled);
            set => _whenIsScheduled = string.IsNullOrEmpty(value)
                ? new Dictionary<string, List<TimeOnly>>()
                : JsonSerializer.Deserialize<Dictionary<string, List<TimeOnly>>>(value) ?? new Dictionary<string, List<TimeOnly>>();
        }

        private bool _archived;
        public bool Archived
        {
            get => _archived;
            set => _archived = value;
        }

        public ScheduledHabits(string habit_ID, List<TimeOnly> monday, List<TimeOnly> tuesday, List<TimeOnly> wednesday, List<TimeOnly> thursday, List<TimeOnly> friday, List<TimeOnly> saturday, List<TimeOnly> sunday)
        {
            this._habitID = habit_ID;
            this._whenIsScheduled = new Dictionary<string, List<TimeOnly>>();
            this._whenIsScheduled.Add("Monday", monday);
            this._whenIsScheduled.Add("Tuesday", tuesday);
            this._whenIsScheduled.Add("Wednesday", wednesday);
            this._whenIsScheduled.Add("Thursday", thursday);
            this._whenIsScheduled.Add("Friday", friday);
            this._whenIsScheduled.Add("Saturday", saturday);
            this._whenIsScheduled.Add("Sunday", sunday);
            this._archived = false;
        }
        //add methods for scheduled_habits class

        public void AddScheduledTime(string DayOfWeek, List<TimeOnly> Time)
        {
            if (this._whenIsScheduled.ContainsKey(DayOfWeek))
            {
                this._whenIsScheduled[DayOfWeek].AddRange(Time);
                this._whenIsScheduled[DayOfWeek] = this._whenIsScheduled[DayOfWeek].Distinct().ToList(); // Remove duplicates
            }
            else
            {
                throw new ArgumentException("Invalid day of the week."); // надо как-то поменять 
            }
        }


        //delete methods for scheduled_habits class
        public void RemoveScheduledTime(string DayOfWeek, List<TimeOnly> Time)
        {
            if (this._whenIsScheduled.ContainsKey(DayOfWeek))
            {
                foreach (var t in Time)
                {
                    this._whenIsScheduled[DayOfWeek].Remove(t);
                }
            }
            else
            {
                throw new ArgumentException("Invalid day of the week.");  // надо как-то поменять 
            }
        }

    }





    public class Schedule
    {
        private string _ID;
        [PrimaryKey]
        public string ID { 
            get => _ID; 
            set 
            { 
                _ID = value;
                _scheduleLastChangeDate = DateTime.Now;
            } 
        }

        private string _userID;
        public string UserID
        {
            get => _userID;
            set
            {
                _userID = value;
                _scheduleLastChangeDate = DateTime.Now;
            }
        }

        private bool _archived;
        public bool Archived
        {
            get => _archived;
            set
            {
                _archived = value;
                _scheduleLastChangeDate = DateTime.Now;
            }
        }

        private List<ScheduledHabits> _scheduledHabits;
        [Ignore]
        public List<ScheduledHabits> ScheduledHabits
        {
            get => _scheduledHabits;
            set
            {
                _scheduledHabits = value;
                _scheduleLastChangeDate = DateTime.Now;
            }
        }
        public string ScheduledHabitsJSON
        {
            get => JsonSerializer.Serialize(_scheduledHabits);
            set => _scheduledHabits = string.IsNullOrEmpty(value)
                ? new List<ScheduledHabits>()
                : JsonSerializer.Deserialize<List<ScheduledHabits>>(value) ?? new List<ScheduledHabits>();
        }

        private DateTime _scheduleCreationDate;
        public DateTime ScheduleCreationDate { get => _scheduleCreationDate; init => _scheduleCreationDate = value; }

        private DateTime _scheduleLastChangeDate;
        public DateTime ScheduleLastChangeDate { get => _scheduleLastChangeDate; set => _scheduleLastChangeDate = value; } //не использовать из вне, оставить init для того чтобы sqlite мог работать 

        public Schedule()
        {
            _scheduleCreationDate = DateTime.Now;
            _scheduleLastChangeDate = DateTime.Now;
        }
        public Schedule(string ID, string UserID, bool archived, List<ScheduledHabits> ScheduledHabits, DateTime ScheduleCreationDate, DateTime ScheduleLastChangeDate) // Constructor for creating object of already existing schedule
        {
            if (string.IsNullOrEmpty(ID) || ID.Length < 12 || !ID.StartsWith("schedule_ID_"))
            {
                throw new ArgumentException("ID is not valid. It must start with 'schedule_ID_<unique_id>'."); // поменять 
            }
            this._ID = ID;
            this._userID = UserID;
            this._archived = archived;
            this._scheduledHabits = ScheduledHabits;
            this._scheduleCreationDate = ScheduleCreationDate;
            this._scheduleLastChangeDate = ScheduleLastChangeDate;
        }
        public Schedule(string UserID, string HabitID, List<TimeOnly> Monday, List<TimeOnly> Tuesday, List<TimeOnly> Wednesday, List<TimeOnly> Thursday, List<TimeOnly> Friday, List<TimeOnly> Saturday, List<TimeOnly> Sunday) // Constructor for creating new schedule object
        {
            this._ID = Program.GetUniqueIDForSchedule();
            this._userID = UserID;
            this._archived = false;
            this._scheduledHabits = new List<ScheduledHabits>
            {
                new ScheduledHabits(HabitID, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday)
            };
            this._scheduleCreationDate = DateTime.Now;
            this._scheduleLastChangeDate = DateTime.Now;
        }


        //add methods for schedule class
        public void AddScheduledHabit(string HabitID, List<TimeOnly> Monday, List<TimeOnly> Tuesday, List<TimeOnly> Wednesday, List<TimeOnly> Thursday, List<TimeOnly> Friday, List<TimeOnly> Saturday, List<TimeOnly> Sunday)
        {
            if (string.IsNullOrEmpty(HabitID) || HabitID.Length < 12 || HabitID.Substring(0, 12) != "habit_ID_")
            {
                throw new ArgumentException("Habit ID is not valid. It must start with 'habit_ID_<unique_id>'.");
            }
            if (this._scheduledHabits.Exists(Habit => Habit.HabitID == HabitID))
            {
                this._scheduledHabits.Where(Habit => Habit.HabitID == HabitID).First().AddScheduledTime("Monday", Monday);
                this._scheduledHabits.Where(Habit => Habit.HabitID == HabitID).First().AddScheduledTime("Tuesday", Tuesday);
                this._scheduledHabits.Where(Habit => Habit.HabitID == HabitID).First().AddScheduledTime("Wednesday", Wednesday);
                this._scheduledHabits.Where(Habit => Habit.HabitID == HabitID).First().AddScheduledTime("Thursday", Thursday);
                this._scheduledHabits.Where(Habit => Habit.HabitID == HabitID).First().AddScheduledTime("Friday", Friday);
                this._scheduledHabits.Where(Habit => Habit.HabitID == HabitID).First().AddScheduledTime("Saturday", Saturday);
                this._scheduledHabits.Where(Habit => Habit.HabitID == HabitID).First().AddScheduledTime("Sunday", Sunday);
                this._scheduleLastChangeDate = DateTime.Now;
            }
            else
            {
                this._scheduledHabits.Add(new ScheduledHabits(HabitID, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday));
                this._scheduleLastChangeDate = DateTime.Now;
            }
        }
        public void AddTimeToScheduledHabit(string HabitID, string DayOfWeek, List<TimeOnly> Time)
        {
            if (HabitID.Substring(0, 9) != "habit_ID_")
            {
                throw new ArgumentException("Habit ID is not valid. It must start with 'habit_ID_<unique_id>'.");
            }
            if (this._scheduledHabits.Exists(Habit => Habit.HabitID == HabitID))
            {
                this._scheduledHabits.Where(Habit => Habit.HabitID == HabitID).First().AddScheduledTime(DayOfWeek, Time);
                this._scheduleLastChangeDate = DateTime.Now;
            }
            else
            {
                throw new ArgumentException("Habit ID does not exist in the schedule.");
            }
        }

        //delete methods for schedule class
        public void RemoveTimeFromScheduledHabit(string HabitID, string DayOfWeek, List<TimeOnly> Time)
        {
            if (HabitID.Substring(0, 9) != "habit_ID_")
            {
                throw new ArgumentException("Habit ID is not valid. It must start with 'habit_ID_<unique_id>'.");
            }
            if (this._scheduledHabits.Exists(Habit => Habit.HabitID == HabitID))
            {
                this._scheduledHabits.Where(Habit => Habit.HabitID == HabitID).First().RemoveScheduledTime(DayOfWeek, Time);
                this._scheduleLastChangeDate = DateTime.Now;
            }
            else
            {
                throw new ArgumentException("Habit ID does not exist in the schedule.");
            }
        }
        public void RemoveScheduledHabit(string HabitID)
        {
            if (HabitID.Substring(0, 9) != "habit_ID_")
            {
                throw new ArgumentException("Habit ID is not valid. It must start with 'habit_ID_<unique_id>'.");
            }
            if (this._scheduledHabits.Exists(Habit => Habit.HabitID == HabitID))
            {
                this._scheduledHabits.RemoveAll(Habit => Habit.HabitID == HabitID);
                this._scheduleLastChangeDate = DateTime.Now;
            }
            else
            {
                throw new ArgumentException("Habit ID does not exist in the schedule.");
            }
        }
    }
}
