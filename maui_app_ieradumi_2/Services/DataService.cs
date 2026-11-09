using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using class_project_ieradumi;

namespace maui_app_ieradumi_2.Services
{
    internal class DataService : IDataService
    {
        private readonly SQLiteAsyncConnection _connection;
        string DBPath;
        public DataService()
        {
            DBPath = Path.Combine(FileSystem.AppDataDirectory, "ieradumi.db3");
            _connection = new SQLiteAsyncConnection(DBPath);
            _connection.CreateTableAsync<User>().Wait();
            _connection.CreateTableAsync<Habit>().Wait();
            _connection.CreateTableAsync<CompletionRecord>().Wait();
            _connection.CreateTableAsync<Schedule>().Wait();

            var existingUsersCount = _connection.Table<User>().CountAsync().Result;

            if (existingUsersCount == 0)
            {
                var defaultUser = new User
                {
                    ID = "DefaultUserID1",
                    Username = "aleksandrs",
                    Password = "1234"
                };

                _connection.InsertAsync(defaultUser).Wait();
            }
        }
        public async Task<bool> LoginAsync(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var user = await _connection.FindAsync<User>(u => u.Username == username && u.Password == password);
                if (user != null)
                {
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
    }
}
