namespace maui_app_ieradumi_2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            SecureStorage.Remove("LoggedInUserID");
            await Shell.Current.GoToAsync("//AuthPage");
            Shell.Current.FlyoutIsPresented = false; // Close the flyout after logout
        }
    }
}
