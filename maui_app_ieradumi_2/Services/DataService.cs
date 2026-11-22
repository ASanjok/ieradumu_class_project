using class_project_ieradumi;
using SQLite;

namespace maui_app_ieradumi_2.Services
{
    internal class DataService : IDataService
    {
        private readonly SQLiteAsyncConnection _connection;
        string DBPath;
        public DataService()
        {
            System.Diagnostics.Debug.WriteLine("запущен конструктор DataSevice");
            DBPath = Path.Combine(FileSystem.AppDataDirectory, "ieradumi.db3");
            _connection = new SQLiteAsyncConnection(DBPath);
            _connection.CreateTableAsync<User>().Wait();
            _connection.CreateTableAsync<Habit>().Wait();
            _connection.CreateTableAsync<HabitSchedule>().Wait();
            _connection.CreateTableAsync<CompletionRecord>().Wait();
            _connection.CreateTableAsync<Schedule>().Wait();

            _connection.CloseAsync();
            SQLiteAsyncConnection.ResetPool();

            if (File.Exists(DBPath))
            {
                File.Delete(DBPath);
                System.Diagnostics.Debug.WriteLine(">>> База данных полностью удалена!");
            }
            _connection.CreateTableAsync<User>();
            _connection.CreateTableAsync<Habit>();
            _connection.CreateTableAsync<HabitSchedule>();
            _connection.CreateTableAsync<CompletionRecord>();
            _connection.CreateTableAsync<Schedule>();

            var existingUsersCount = _connection.Table<User>().CountAsync().Result;

            if (existingUsersCount == 0)
            {
                var user = new User
                {
                    ID = "12345",
                    Username = "sanj",
                    Password = "123",
                    Archived = false,
                };
                _connection.InsertAsync(user).Wait();
                System.Diagnostics.Debug.WriteLine("сохранён пользователь \n ID = user_ID_12345?\nUsername = sanj\nPassword = 123");
                var createduser = _connection.FindAsync<User>(u => u.Username == "sanj").Result;
                System.Diagnostics.Debug.WriteLine("фактические данные \n ID = " + createduser.ID + "\nUsername = " + createduser.Username + "\nPassword = " + createduser.Password);
            }


            var existingHabitsCount = _connection.Table<Habit>().CountAsync().Result;

            if (existingHabitsCount == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    var habit = new Habit
                    {
                        ID = Guid.NewGuid().ToString(),
                        HabitName = "habit_name_" + i,
                        HabitDescription = "habit description " + i,
                        AuthorID = "12345",
                        Archived = (i % 2 == 0) ? false : true,
                    };
                    _connection.InsertAsync(habit).Wait();
                    System.Diagnostics.Debug.WriteLine("сохранена привычка " + i);
                }


            }
        }
        //^конструктор







        public async Task<bool> LoginAsync(string username, string password) // создан api endpoint UserController
        {
            System.Diagnostics.Debug.WriteLine("запущена фукнция LoginAsync " + username + " | " + password);
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var user = await _connection.FindAsync<User>(u => u.Username == username && u.Password == password);
                if (user != null)
                {
                    await SecureStorage.SetAsync("LoggedInUserID", user.ID);
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> RegisterAsync(string username, string password) // создан api endpoint UserController
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var existingUser = await _connection.FindAsync<User>(u => u.Username == username);
                if (existingUser == null)
                {
                    var newUser = new User
                    {
                        ID = Guid.NewGuid().ToString(),
                        Username = username,
                        Password = password
                    };
                    await _connection.InsertAsync(newUser);
                    return true;
                }
            }
            return false;
        }
        public async Task SaveHabitAsync(string name, string description, bool archived = false)// создан api endpoint HabitController
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(description))
            {
                var loggedInUser = await SecureStorage.GetAsync("LoggedInUserID");
                if (!string.IsNullOrEmpty(loggedInUser))
                {
                    var newHabit = new Habit
                    {
                        ID = Guid.NewGuid().ToString(),
                        HabitName = name,
                        HabitDescription = description,
                        AuthorID = loggedInUser,
                        Archived = archived,
                    };
                    await _connection.InsertAsync(newHabit);

                }
            }
        }
        //public async Task SaveHabitAsync(Habit habit)
        //{
        //    if (habit != null)
        //    {
        //        await _connection.InsertAsync(habit);
        //    }
        //}
        public async Task<List<Habit>> GetHabitListAsync(string userId) // создан api endpoint HabitController
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var habits = await _connection.Table<Habit>().Where(h => h.AuthorID == userId).ToListAsync();
                return habits;
            }
            return new List<Habit>();
        }
        public async Task<Habit?> GetHabitAsync(string habitId)// создан api endpoint HabitController
        {
            if (!string.IsNullOrEmpty(habitId))
            {
                Habit habit = await _connection.FindAsync<Habit>(h => h.ID == habitId);
                return habit;
            }
            return null;
        }
        public async Task EditHabitAsync(string ID, Habit habit)// создан api endpoint HabitController
        {
            if (!string.IsNullOrEmpty(ID) && habit != null)
            {
                var existingHabit = _connection.FindAsync<Habit>(h => h.ID == ID).Result;
                if (existingHabit != null)
                {
                    existingHabit.HabitName = habit.HabitName;
                    existingHabit.HabitDescription = habit.HabitDescription;
                    existingHabit.Archived = habit.Archived;
                    existingHabit.HabitLastChangeDate = DateTime.Now;
                    _connection.UpdateAsync(existingHabit);
                }
            }
        }
        public async Task DeleteHabitAsync(string habitId)// создан api endpoint HabitController
        {
            System.Diagnostics.Debug.WriteLine("запущена функция Delete habit async");
            if (!string.IsNullOrEmpty(habitId))
            {
                var existingHabit = await _connection.FindAsync<Habit>(habitId);

                if (existingHabit != null)
                {
                    await _connection.DeleteAsync(existingHabit);
                    System.Diagnostics.Debug.WriteLine("Привычка успешно удалена из базы");
                }
            }
        }
        public async Task<User?> GetUserByIdAsync(string userId) // создан api endpoint UserController
        {
            if (!string.IsNullOrEmpty(userId))
            {
                User user = await _connection.FindAsync<User>(u => u.ID == userId);
                return user;
            }
            return null;
        }
        public async Task UpdateUserAsync(User user) // создан api endpoint UserController
        {
            if (user != null)
            {
                await _connection.UpdateAsync(user);
            }
        }
        public async Task<Schedule?> GetScheduleByUserIdAsync(string userId)// создан api endpoint ScheduleController
        {
            if (string.IsNullOrEmpty(userId)) return null;

            Schedule schedule = await _connection.Table<Schedule>().Where(s => s.UserID == userId).FirstOrDefaultAsync();

            if (schedule == null)
            {
                schedule = new Schedule
                {
                    ID = "schedule_ID_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                    UserID = userId,
                    Archived = false,
                    ScheduleCreationDate = DateTime.Now,
                    ScheduleLastChangeDate = DateTime.Now
                };

                await _connection.InsertAsync(schedule);
            }

            schedule.HabitSchedules = await _connection.Table<HabitSchedule>()
                .Where(hs => hs.ScheduleID == schedule.ID && hs.DeletedAt == null)
                .ToListAsync();

            return schedule;
        }

        public async Task UpdateScheduleAsync(Schedule schedule)// создан api endpoint ScheduleController
        {
            System.Diagnostics.Debug.WriteLine("UpdateScheduleAsync (Реляционный) -----");

            if (schedule != null)
            {
                schedule.ScheduleLastChangeDate = DateTime.Now;
                await _connection.UpdateAsync(schedule);

                if (schedule.HabitSchedules != null)
                {
                    foreach (var item in schedule.HabitSchedules)
                    {
                        var existing = await _connection.FindAsync<HabitSchedule>(item.ID);
                        if (existing == null)
                        {
                            await _connection.InsertAsync(item);
                            System.Diagnostics.Debug.WriteLine($"Вставлен новый HabitSchedule: {item.ID}");
                        }
                        else
                        {
                            await _connection.UpdateAsync(item);
                        }
                    }
                }
            }
        }

        public async Task SoftDeleteHabitScheduleAsync(string scheduleId, string habitId, string dayOfWeek, TimeSpan scheduledTime)// создан api endpoint ScheduleController
        {
            if (string.IsNullOrEmpty(scheduleId) || string.IsNullOrEmpty(habitId)) return;

            // Ищем конкретную активную запись в таблице HabitSchedule
            var targetRecord = await _connection.Table<HabitSchedule>()
                .Where(hs => hs.ScheduleID == scheduleId &&
                             hs.HabitID == habitId &&
                             hs.DayOfWeek == dayOfWeek &&
                             hs.ScheduledTime == scheduledTime &&
                             hs.DeletedAt == null)
                .FirstOrDefaultAsync();

            if (targetRecord != null)
            {
                // Выполняем мягкое удаление
                targetRecord.DeletedAt = DateTime.Now;

                // Обновляем запись в базе данных
                await _connection.UpdateAsync(targetRecord);

                System.Diagnostics.Debug.WriteLine($"[DataService] HabitSchedule {targetRecord.ID} успешно помечен как удаленный (DeletedAt).");
            }
        }

        public async Task<List<CompletionRecord>> GetCompletionsByUserAndDateAsync(string userId, DateTime date)// создан api endpoint CompletionRecordController
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await _connection.Table<CompletionRecord>()
                .Where(c => c.UserID == userId &&
                            c.CompletionDate >= startOfDay &&
                            c.CompletionDate < endOfDay)
                .ToListAsync();
        }

        public async Task AddCompletionAsync(string habitId, string userId, DateTime completionDate, bool inTime, bool hasTheRecordBeenUsed, Dictionary<string, TimeOnly> scheduleTime)// создан api endpoint CompletionRecordController
        {
            CompletionRecord record = new CompletionRecord()
            {
                ID = "completion_record_ID_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                HabitID = habitId,
                UserID = userId,
                CompletionDate = completionDate,
                InTime = inTime,
                HasTheRecordBeenUsed = hasTheRecordBeenUsed,
                ScheduleTime = scheduleTime,
            };

            await _connection.InsertAsync(record);
            return;
        }
    }
}


