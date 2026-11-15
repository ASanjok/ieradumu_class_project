using maui_app_ieradumi_2.ViewModels;

namespace maui_app_ieradumi_2.Views;

public partial class CompletionPage : ContentPage
{
	public CompletionPage(CompletionViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}