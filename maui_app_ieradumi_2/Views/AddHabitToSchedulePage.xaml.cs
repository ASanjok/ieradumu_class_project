
using maui_app_ieradumi_2.ViewModels;

namespace maui_app_ieradumi_2.Views;

public partial class AddHabitToSchedulePage : ContentPage
{
    private readonly AddHabitToScheduleViewModel _viewModel;
    public AddHabitToSchedulePage(AddHabitToScheduleViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDataAsync();
    }
}