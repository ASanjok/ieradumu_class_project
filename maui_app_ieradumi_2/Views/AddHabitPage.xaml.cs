using maui_app_ieradumi_2.ViewModels;

namespace maui_app_ieradumi_2.Views;

public partial class AddHabitPage : ContentPage
{
    private readonly AddHabitViewModel _viewModel;
    public AddHabitPage(AddHabitViewModel viewModel)
	{
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}