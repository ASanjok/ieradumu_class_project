using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace maui_app_ieradumi_2.ViewModels
{
    
    // ViewModel для экрана авторизации и регистрации (AuthPage).
    // Управляет состоянием формы, переключением между входом и регистрацией, а также валидацией полей ввода.
    
    public partial class AuthViewModel : ObservableObject
    {
        private readonly Services.IDataService _dataService;

        public AuthViewModel(Services.IDataService dataService)
        {
            _dataService = dataService;
        }

        // Флаг режима: true — вход (Login), false — регистрация (Register)
        [ObservableProperty]
        private bool isLoginMode = true;

        // Переменные для динамического изменения текстов на экране в зависимости от режима
        [ObservableProperty]
        private string topText = "Log in";

        [ObservableProperty]
        private string enterButtonText = "login";

        [ObservableProperty]
        private string switchButtonText = "create new account";

        // Поля для ввода данных пользователем
        [ObservableProperty]
        private string userName = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        
        // Команда для главной кнопки (Вход или Регистрация в зависимости от текущего режима).
        [RelayCommand]
        private async void EnterButton()
        {
            if (IsLoginMode)
            {
                await Login();
            }
            else
            {
                await Register();
            }
        }

        
        // Команда для переключения между режимами входа и регистрации (меняет тексты и поведение интерфейса).
        
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

        
        // Внутренний метод для выполнения входа пользователя в систему.
        // Проверяет заполненность полей и обращается к сервису данных.
        
        private async Task Login()
        {
            bool allowed = true;
            if (string.IsNullOrEmpty(UserName))
            {
                TopText = "Enter Username\n";
                allowed = false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                TopText = TopText + "Enter Password\n";
                allowed = false;
            }

            if (allowed)
            {
                bool success = await _dataService.LoginAsync(UserName, Password);
                if (success)
                {
                    // При успешном входе переходим на главный экран со списком привычек
                    await Shell.Current.GoToAsync("//HabitListPage");
                    TopText = "correct!!!";
                }
                else
                {
                    TopText = "Invalid username or password\n";
                }
            }
        }

        
        // Внутренний метод для регистрации нового пользователя.
        // Создает аккаунт и сразу выполняет автоматический вход.
        
        private async Task Register()
        {
            bool allowed = true;
            if (string.IsNullOrEmpty(UserName))
            {
                TopText = "Enter Username\n";
                allowed = false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                TopText = TopText + "Enter Password\n";
                allowed = false;
            }

            if (allowed)
            {
                await _dataService.RegisterAsync(UserName, Password);
                bool success = await _dataService.LoginAsync(UserName, Password);
                if (success)
                {
                    // При успешной регистрации и входе переходим на главный экран
                    await Shell.Current.GoToAsync("//HabitListPage");
                    TopText = "Registered!!!";
                }
                else
                {
                    TopText = "Username already exists\n";
                }
            }
        }
    }
}