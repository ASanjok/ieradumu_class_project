using System.Threading.Tasks;

namespace maui_app_ieradumi.services
{
    public class DataService : IDataService
    {
        // Имитируем проверку пользователя (пока через заглушку)
        public async Task<bool> DoesUserExistAsync(string username)
        {
            await Task.Delay(50); // Имитация запроса к БД
            return username == "aleksandrs";
        }

        // Имитация проверки пароля
        public async Task<bool> DoesPasswordMatchAsync(string username, string password)
        {
            await Task.Delay(50);
            return username == "aleksandrs" && password == "1234";
        }

        // Имитация создания аккаунта
        public async Task CreateAccountAsync(string userName, string password)
        {
            await Task.Delay(100);
            // Здесь в будущем будет сохранение в БД: await _db.InsertAsync(...)
        }

        // Имитация создания привычки с расписанием
        public async Task CreateNewHabitInScheduleAsync(string habitName, string habitDescription, string userID)
        {
            await Task.Delay(100);
            // Здесь в будущем будет сохранение привычки в БД
        }
    }
}