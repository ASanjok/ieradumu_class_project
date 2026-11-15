namespace maui_app_ieradumi_2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
        private async void OnLogoutClicked(object sender, EventArgs e) // функция для кнопки выхода из аккаунта
        {
            SecureStorage.Remove("LoggedInUserID");
            await Shell.Current.GoToAsync("//AuthPage");
            Shell.Current.FlyoutIsPresented = false; // закрыть меню после выхода
        }
    }
}
