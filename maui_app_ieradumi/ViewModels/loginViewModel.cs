using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_app_ieradumi.services;

namespace maui_app_ieradumi.ViewModels
{
    public partial class loginViewModel : ObservableObject
    {
        private readonly IDataService _dataService;

        public loginViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        [ObservableProperty]
        private bool isLoginMode = true;

        [ObservableProperty]
        private string topText = "Log in";

        [ObservableProperty]
        private string enterButtonText = "login";

        [ObservableProperty]
        private string userName = string.Empty;

        [ObservableProperty]
        private string switchButtonText = "create new account";

        [ObservableProperty]
        private string password = string.Empty;
        [RelayCommand]
        private async void EnterButton()
        {
            if (IsLoginMode)
            {
                // 1. Обязательно используем await для асинхронных методов
                bool userExists = await _dataService.DoesUserExistAsync(UserName);

                if (userExists)
                {
                    bool isPasswordCorrect = await _dataService.DoesPasswordMatchAsync(UserName, Password);

                    if (isPasswordCorrect)
                    {
                        TopText = "You entered correctly";
                        await Shell.Current.GoToAsync("//MainPage");
                        return; // 2. Выходим из метода, чтобы код ниже не выполнялся при успехе!
                    }
                }

                // Эта строка выполнится только если пользователь не найден ИЛИ пароль неверный
                TopText = "Wrong credentials";
            }
            else
            {
                TopText = "";
                bool allowed = true;
                if (string.IsNullOrEmpty(UserName))
                {
                    TopText = TopText + "Enter Username\n";
                    allowed = false;
                }
                if (string.IsNullOrEmpty(Password))
                {
                    TopText = TopText + "Enter Password\n";
                    allowed = false;
                }
                if (allowed)
                {
                    await _dataService.CreateAccountAsync(UserName, Password);
                    SwitchButton();
                }
            }
        }

        [RelayCommand]
        private void SwitchButton()
        {
            IsLoginMode = !IsLoginMode;
            if (IsLoginMode)
            {
                TopText = "Log in";
                EnterButtonText = "login";
                SwitchButtonText = "create new account";
            }
            else
            {
                TopText = "Register";
                EnterButtonText = "register";
                SwitchButtonText = "already have an account?";
            }
        }

    }
}
