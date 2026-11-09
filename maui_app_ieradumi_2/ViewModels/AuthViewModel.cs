using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace maui_app_ieradumi_2.ViewModels
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly Services.IDataService _dataService;
        public AuthViewModel(Services.IDataService dataService)
        {
            _dataService = dataService;
        }
        //variable for modes (login/register)
        [ObservableProperty]
        private bool isLoginMode = true;

        //variables for texts
        [ObservableProperty]
        private string topText = "Log in";

        [ObservableProperty]
        private string enterButtonText = "login";

        [ObservableProperty]
        private string switchButtonText = "create new account";

        //variables for user input
        [ObservableProperty]
        private string userName = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;


        //functions for buttons
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
                    //await Shell.Current.GoToAsync("//MainPage");
                    TopText = "correct!!!";
                }
                else
                {
                    TopText = "Invalid username or password\n";
                }
            }
        }

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
                bool success = await _dataService.RegisterAsync(UserName, Password);
                if (success)
                {
                    //await Shell.Current.GoToAsync("//MainPage");
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
