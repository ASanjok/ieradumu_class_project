using maui_app_ieradumi_2.ViewModels;

namespace maui_app_ieradumi_2.Views;

public partial class SchedulePage : ContentPage
{
    private readonly ScheduleViewModel _viewModel;
    public SchedulePage(ScheduleViewModel viewmodel)
	{
		InitializeComponent();
		_viewModel = viewmodel; // переменная нужна для того чтобы при открытии страницы SchedulePage вызывался метод LoadScheduleAsync() из ScheduleViewModel
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadScheduleAsync();
    }
}