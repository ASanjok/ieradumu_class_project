namespace maui_app_ieradumi;

public partial class LoginPage : ContentPage
{
	public LoginPage(ViewModels.loginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}