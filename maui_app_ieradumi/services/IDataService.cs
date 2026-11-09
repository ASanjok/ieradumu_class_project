namespace maui_app_ieradumi.services
{
    public interface IDataService
    {
        Task<bool> DoesUserExistAsync(string username);
        Task<bool> DoesPasswordMatchAsync(string username, string password);
        Task CreateAccountAsync(string userName, string password);
        Task CreateNewHabitInScheduleAsync(string habitName, string habitDescription, string userID);
    }
}