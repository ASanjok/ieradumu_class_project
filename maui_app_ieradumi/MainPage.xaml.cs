namespace maui_app_ieradumi
{
    public partial class MainPage : ContentPage
    {
        public MainPage(ViewModels.mainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
