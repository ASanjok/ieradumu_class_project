using maui_app_ieradumi_2.ViewModels;

namespace maui_app_ieradumi_2.Views;

public partial class AddHabitPage : ContentPage
{
	public AddHabitPage(AddHabitViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}