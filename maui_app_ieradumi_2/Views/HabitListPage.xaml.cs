using maui_app_ieradumi_2.ViewModels;

namespace maui_app_ieradumi_2.Views;

public partial class HabitListPage : ContentPage
{
    private readonly HabitListViewModel _viewModel;
    public HabitListPage(ViewModels.HabitListViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
	protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadHabitsAsync();
    }
}