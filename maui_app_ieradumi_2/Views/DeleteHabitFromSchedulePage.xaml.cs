
namespace maui_app_ieradumi_2.Views;

public partial class DeleteHabitFromSchedulePage : ContentPage
{
	public DeleteHabitFromSchedulePage()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Shell.Current.GoToAsync($"//SchedulePage");
    }
}