using SQLite;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace class_project_ieradumi
{
    public class ScheduledHabits
    {
        private string _habitID;
        public string HabitID
        {
            get => _habitID;
            set => _habitID = value;
        }

        private Dictionary<string, List<TimeOnly>> _whenIsScheduled;
        [Ignore]
        public Dictionary<string, List<TimeOnly>> WhenIsScheduled
        {
            get => _whenIsScheduled;
            set => _whenIsScheduled = value ?? new Dictionary<string, List<TimeOnly>>();
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

        public ScheduledHabits()
        {
            _whenIsScheduled = new Dictionary<string, List<TimeOnly>>
            {
                { "Monday", new List<TimeOnly>() },
                { "Tuesday", new List<TimeOnly>() },
                { "Wednesday", new List<TimeOnly>() },
                { "Thursday", new List<TimeOnly>() },
                { "Friday", new List<TimeOnly>() },
                { "Saturday", new List<TimeOnly>() },
                { "Sunday", new List<TimeOnly>() }
            };
            _archived = false;
        }

        [JsonConstructor]
        public ScheduledHabits(string habitID, Dictionary<string, List<TimeOnly>> whenIsScheduled, bool archived)
        {
            _habitID = habitID;
            _whenIsScheduled = whenIsScheduled ?? new Dictionary<string, List<TimeOnly>>();
            _archived = archived;
        }

        public ScheduledHabits(string habit_ID, List<TimeOnly> monday, List<TimeOnly> tuesday, List<TimeOnly> wednesday, List<TimeOnly> thursday, List<TimeOnly> friday, List<TimeOnly> saturday, List<TimeOnly> sunday)
        {
            this._habitID = habit_ID;
            this._whenIsScheduled = new Dictionary<string, List<TimeOnly>>
            {
                { "Monday", monday ?? new List<TimeOnly>() },
                { "Tuesday", tuesday ?? new List<TimeOnly>() },
                { "Wednesday", wednesday ?? new List<TimeOnly>() },
                { "Thursday", thursday ?? new List<TimeOnly>() },
                { "Friday", friday ?? new List<TimeOnly>() },
                { "Saturday", saturday ?? new List<TimeOnly>() },
                { "Sunday", sunday ?? new List<TimeOnly>() }
            };
            this._archived = false;
        }

        public void AddScheduledTime(string DayOfWeek, List<TimeOnly> Time)
        {
            if (this._whenIsScheduled.ContainsKey(DayOfWeek))
            {
                this._whenIsScheduled[DayOfWeek].AddRange(Time);
                this._whenIsScheduled[DayOfWeek] = this._whenIsScheduled[DayOfWeek].Distinct().ToList(); // Remove duplicates
            }
            else
            {
                throw new ArgumentException("Invalid day of the week.");
            }
        }

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
                throw new ArgumentException("Invalid day of the week.");
            }
        }
    }





    public class Schedule
    {
        private string _ID;
        [PrimaryKey]
        public string ID
        {
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

        private List<ScheduledHabits> _scheduledHabits = new();
        [Ignore]
        public List<ScheduledHabits> ScheduledHabits
        {
            get => _scheduledHabits;
            set
            {
                _scheduledHabits = value ?? new List<ScheduledHabits>();
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
        public DateTime ScheduleLastChangeDate { get => _scheduleLastChangeDate; set => _scheduleLastChangeDate = value; }

        public Schedule()
        {
            _scheduleCreationDate = DateTime.Now;
            _scheduleLastChangeDate = DateTime.Now;
            _scheduledHabits = new List<ScheduledHabits>();
        }

        public Schedule(string ID, string UserID, bool archived, List<ScheduledHabits> ScheduledHabits, DateTime ScheduleCreationDate, DateTime ScheduleLastChangeDate)
        {
            if (string.IsNullOrEmpty(ID) || ID.Length < 12 || !ID.StartsWith("schedule_ID_"))
            {
                throw new ArgumentException("ID is not valid. It must start with 'schedule_ID_<unique_id>'.");
            }
            this._ID = ID;
            this._userID = UserID;
            this._archived = archived;
            this._scheduledHabits = ScheduledHabits ?? new List<ScheduledHabits>();
            this._scheduleCreationDate = ScheduleCreationDate;
            this._scheduleLastChangeDate = ScheduleLastChangeDate;
        }

        public Schedule(string UserID, string HabitID, List<TimeOnly> Monday, List<TimeOnly> Tuesday, List<TimeOnly> Wednesday, List<TimeOnly> Thursday, List<TimeOnly> Friday, List<TimeOnly> Saturday, List<TimeOnly> Sunday)
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

        public void AddScheduledHabit(string HabitID, List<TimeOnly> Monday, List<TimeOnly> Tuesday, List<TimeOnly> Wednesday, List<TimeOnly> Thursday, List<TimeOnly> Friday, List<TimeOnly> Saturday, List<TimeOnly> Sunday)
        {
            if (string.IsNullOrEmpty(HabitID) || HabitID.Length < 12 || HabitID.Substring(0, 12) != "habit_ID_")
            {
                throw new ArgumentException("Habit ID is not valid. It must start with 'habit_ID_<unique_id>'.");
            }

            if (this._scheduledHabits.Exists(Habit => Habit.HabitID == HabitID))
            {
                var target = this._scheduledHabits.First(Habit => Habit.HabitID == HabitID);
                target.AddScheduledTime("Monday", Monday);
                target.AddScheduledTime("Tuesday", Tuesday);
                target.AddScheduledTime("Wednesday", Wednesday);
                target.AddScheduledTime("Thursday", Thursday);
                target.AddScheduledTime("Friday", Friday);
                target.AddScheduledTime("Saturday", Saturday);
                target.AddScheduledTime("Sunday", Sunday);
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
            if (this._scheduledHabits == null)
            {
                this._scheduledHabits = new List<ScheduledHabits>();
            }

            var existingHabit = this._scheduledHabits.FirstOrDefault(Habit => Habit.HabitID == HabitID);
            if (existingHabit != null)
            {
                existingHabit.AddScheduledTime(DayOfWeek, Time);
                this._scheduleLastChangeDate = DateTime.Now;
            }
            else
            {
                var newScheduledHabit = new ScheduledHabits(
                    HabitID,
                    new List<TimeOnly>(),
                    new List<TimeOnly>(),
                    new List<TimeOnly>(),
                    new List<TimeOnly>(),
                    new List<TimeOnly>(),
                    new List<TimeOnly>(),
                    new List<TimeOnly>()
                );
                newScheduledHabit.AddScheduledTime(DayOfWeek, Time);
                this._scheduledHabits.Add(newScheduledHabit);
                this._scheduleLastChangeDate = DateTime.Now;
            }
        }

        public void RemoveTimeFromScheduledHabit(string HabitID, string DayOfWeek, List<TimeOnly> Time)
        {
            
            if (this._scheduledHabits.Exists(Habit => Habit.HabitID == HabitID))
            {
                this._scheduledHabits.First(Habit => Habit.HabitID == HabitID).RemoveScheduledTime(DayOfWeek, Time);
                this._scheduleLastChangeDate = DateTime.Now;
            }
            else
            {
                throw new ArgumentException("Habit ID does not exist in the schedule.");
            }
        }

        public void RemoveScheduledHabit(string HabitID)
        {
            if (string.IsNullOrEmpty(HabitID) || HabitID.Length < 9 || HabitID.Substring(0, 9) != "habit_ID_")
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