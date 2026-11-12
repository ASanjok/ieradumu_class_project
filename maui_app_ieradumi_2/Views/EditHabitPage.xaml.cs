using class_project_ieradumi;
using maui_app_ieradumi_2.Services;
using maui_app_ieradumi_2.ViewModels;

namespace maui_app_ieradumi_2.Views;


public partial class EditHabitPage : ContentPage
{
    private readonly EditHabitViewModel _viewModel;
    public EditHabitPage(EditHabitViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadHabitAsync();
    }
}