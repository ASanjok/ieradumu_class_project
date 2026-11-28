using System.Net.Http.Json;
using class_project_ieradumi;

//для тестового комита в отдельную ветку

namespace maui_app_ieradumi_2.Services
{
    internal class DataService : IDataService
    {
        private readonly HttpClient _httpClient;

        public DataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // --- USER ---

        public async Task<bool> LoginAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            try
            {
                var payload = new { username, password };
                var response = await _httpClient.PostAsJsonAsync("api/User/login", payload);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    if (result != null && !string.IsNullOrEmpty(result.Id))
                    {
                        await SecureStorage.SetAsync("LoggedInUserID", result.Id);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] LoginAsync: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            try
            {
                var payload = new { username, password };
                var response = await _httpClient.PostAsJsonAsync("api/User/register", payload);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    return result != null && !string.IsNullOrEmpty(result.Id);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] RegisterAsync: {ex.Message}");
            }
            return false;
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;

            try
            {
                return await _httpClient.GetFromJsonAsync<User>($"api/User/{userId}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] GetUserByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.ID)) return;

            try
            {
                var payload = new
                {
                    username = user.Username,
                    email = user.Email,
                    password = user.Password,
                    archived = user.Archived
                };
                var response = await _httpClient.PutAsJsonAsync($"api/User/{user.ID}", payload);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] UpdateUserAsync: {ex.Message}");
                throw;
            }
        }

        // --- HABIT ---

        public async Task SaveHabitAsync(string name, string description, bool archived = false)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description)) return;

            var loggedInUser = await SecureStorage.GetAsync("LoggedInUserID");
            if (string.IsNullOrEmpty(loggedInUser)) return;

            try
            {
                var newHabit = new Habit
                {
                    ID = Guid.NewGuid().ToString(),
                    HabitName = name,
                    HabitDescription = description,
                    AuthorID = loggedInUser,
                    Archived = archived,
                    HabitCreationDate = DateTime.UtcNow,
                    HabitLastChangeDate = DateTime.UtcNow
                };

                var response = await _httpClient.PostAsJsonAsync("api/Habit", newHabit);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] SaveHabitAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Habit>> GetHabitListAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return new List<Habit>();

            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<Habit>>($"api/Habit/user/{userId}");
                return result ?? new List<Habit>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] GetHabitListAsync: {ex.Message}");
                throw; // Пробрасываем наверх, чтобы UI показал ошибку "API nav sasniedzams"
            }
        }

        public async Task<Habit?> GetHabitAsync(string habitId)
        {
            if (string.IsNullOrEmpty(habitId)) return null;

            try
            {
                return await _httpClient.GetFromJsonAsync<Habit>($"api/Habit/{habitId}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] GetHabitAsync: {ex.Message}");
                return null;
            }
        }

        public async Task EditHabitAsync(string ID, Habit habit)
        {
            if (string.IsNullOrEmpty(ID) || habit == null) return;

            try
            {
                // Соответствует схеме UpdateHabitData
                var payload = new
                {
                    habitName = habit.HabitName,
                    habitDescription = habit.HabitDescription,
                    authorID = habit.AuthorID,
                    archived = habit.Archived
                };

                var response = await _httpClient.PutAsJsonAsync($"api/Habit/{ID}", payload);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] EditHabitAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteHabitAsync(string habitId)
        {
            if (string.IsNullOrEmpty(habitId)) return;

            try
            {
                var response = await _httpClient.DeleteAsync($"api/Habit/{habitId}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] DeleteHabitAsync: {ex.Message}");
                throw;
            }
        }

        // --- SCHEDULE ---

        public async Task<Schedule?> GetScheduleByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;

            try
            {
                var response = await _httpClient.GetAsync($"api/Schedule/user/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Schedule>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] GetScheduleByUserIdAsync: {ex.Message}");
            }

            // Если расписания еще нет на сервере (например, вернулся 404), создаем его через POST
            try
            {
                var newSchedule = new Schedule
                {
                    ID = "schedule_ID_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                    UserID = userId,
                    Archived = false,
                    ScheduleCreationDate = DateTime.UtcNow,
                    ScheduleLastChangeDate = DateTime.UtcNow
                };

                var createResponse = await _httpClient.PostAsJsonAsync("api/Schedule", newSchedule);
                if (createResponse.IsSuccessStatusCode)
                {
                    return await createResponse.Content.ReadFromJsonAsync<Schedule>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] CreateSchedule: {ex.Message}");
            }

            return null;
        }

        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            if (schedule == null) return;

            try
            {
                // 1. Обновляем/создаем сам Schedule
                schedule.ScheduleLastChangeDate = DateTime.UtcNow;
                await _httpClient.PostAsJsonAsync("api/Schedule", schedule);

                // 2. Отправляем элементы расписания в POST /api/Schedule/{scheduleId}/items
                if (schedule.HabitSchedules != null)
                {
                    foreach (var item in schedule.HabitSchedules)
                    {
                        await _httpClient.PostAsJsonAsync($"api/Schedule/{schedule.ID}/items", item);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] UpdateScheduleAsync: {ex.Message}");
                throw;
            }
        }

        public async Task SoftDeleteHabitScheduleAsync(string scheduleId, string habitId, string dayOfWeek, TimeSpan scheduledTime)
        {
            if (string.IsNullOrEmpty(scheduleId) || string.IsNullOrEmpty(habitId)) return;

            try
            {
                // В API удаление идет по идентификатору habitScheduleId: DELETE /api/Schedule/items/{habitScheduleId}
                var schedule = await GetScheduleByUserIdAsync(await SecureStorage.GetAsync("LoggedInUserID") ?? "");
                var target = schedule?.HabitSchedules?.FirstOrDefault(hs =>
                    hs.HabitID == habitId &&
                    hs.DayOfWeek == dayOfWeek &&
                    hs.ScheduledTime == scheduledTime &&
                    hs.DeletedAt == null);

                if (target != null)
                {
                    var response = await _httpClient.DeleteAsync($"api/Schedule/items/{target.ID}");
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] SoftDeleteHabitScheduleAsync: {ex.Message}");
                throw;
            }
        }

        // --- COMPLETION RECORD ---

        public async Task<List<CompletionRecord>> GetCompletionsByUserAndDateAsync(string userId, DateTime date)
        {
            if (string.IsNullOrEmpty(userId)) return new List<CompletionRecord>();

            try
            {
                // Форматируем дату в ISO 8601 для query-параметра ?date=...
                string isoDate = Uri.EscapeDataString(date.ToString("O"));
                var result = await _httpClient.GetFromJsonAsync<List<CompletionRecord>>(
                    $"api/CompletionRecord/user/{userId}?date={isoDate}");

                return result ?? new List<CompletionRecord>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] GetCompletionsByUserAndDateAsync: {ex.Message}");
                return new List<CompletionRecord>();
            }
        }

        public async Task AddCompletionAsync(string habitId, string userId, DateTime completionDate, bool inTime, bool hasTheRecordBeenUsed, Dictionary<string, TimeOnly> scheduleTime)
        {
            try
            {
                var record = new CompletionRecord
                {
                    ID = "completion_record_ID_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                    HabitID = habitId,
                    UserID = userId,
                    CompletionDate = completionDate,
                    InTime = inTime,
                    HasTheRecordBeenUsed = hasTheRecordBeenUsed,
                    ScheduleTime = scheduleTime
                };

                var response = await _httpClient.PostAsJsonAsync("api/CompletionRecord", record);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API Error] AddCompletionAsync: {ex.Message}");
                throw;
            }
        }

        // Вспомогательный класс для десериализации LoginResponse и RegisterResponse
        private class AuthResponse
        {
            public string Id { get; set; } = string.Empty;
        }
    }
}