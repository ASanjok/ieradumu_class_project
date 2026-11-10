using Microsoft.Extensions.DependencyInjection;

namespace maui_app_ieradumi_2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await IsUserLoggedInAndMoveToHabitListPage();
        }
        public async Task IsUserLoggedInAndMoveToHabitListPage()
        {
            var userId = await SecureStorage.GetAsync("LoggedInUserID");
            if (!string.IsNullOrEmpty(userId))
            {
                await Shell.Current.GoToAsync("//HabitListPage");
            }
            else
            {
                await Shell.Current.GoToAsync("//AuthPage");
            }
        }
    }
}