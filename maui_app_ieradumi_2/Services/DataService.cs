using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using class_project_ieradumi;
using maui_app_ieradumi_2.Views;

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
        public async Task<bool> LoginAsync(string username, string password)
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
        public async Task<bool> RegisterAsync(string username, string password)
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
        public async Task SaveHabitAsync(string name, string description, bool archived = false)
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
        public async Task SaveHabitAsync(Habit habit)
        {
            if (habit != null)
            {
                await _connection.InsertAsync(habit);

            }
        }
        public async Task<List<Habit>> GetHabitListAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var habits = await _connection.Table<Habit>().Where(h => h.AuthorID == userId).ToListAsync();
                return habits;
            }
            return new List<Habit>();
        }
        public async Task<Habit?> GetHabitAsync(string habitId)
        {
            if (!string.IsNullOrEmpty(habitId))
            {
                Habit habit = await _connection.FindAsync<Habit>(h => h.ID == habitId);
                return habit;
            }
            return null;
        }
        public async Task EditHabitAsync(string ID, Habit habit)
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
        public async Task DeleteHabitAsync(string habitId)
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
        public async Task<User?> GetUserByIdAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                User user = await _connection.FindAsync<User>(u => u.ID == userId);
                return user;
            }
            return null;
        }
        public async Task UpdateUserAsync(User user)
        {
            if (user != null)
            {
                await _connection.UpdateAsync(user);
            }
        }
    }
}
        

