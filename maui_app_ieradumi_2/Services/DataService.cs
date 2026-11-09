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
                    ID = "user_default_1",
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
                return true;
            }
            return false;
        }
    }
}
