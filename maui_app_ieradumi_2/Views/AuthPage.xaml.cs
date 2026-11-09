using maui_app_ieradumi_2;
namespace maui_app_ieradumi_2.Views;

public partial class AuthPage : ContentPage
{
	public AuthPage(ViewModels.AuthViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}