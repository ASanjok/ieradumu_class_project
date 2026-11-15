using maui_app_ieradumi_2.ViewModels;

namespace maui_app_ieradumi_2.Views;

public partial class UserInfoPage : ContentPage
{
	readonly UserInfoViewModel _viewmodel;
	public UserInfoPage(UserInfoViewModel viewmodel)
	{
		InitializeComponent();
		_viewmodel = viewmodel; // переменная нужна для того чтобы использовать данные пользователя в методе OnAppearing, а также для привязки данных к интерфейсу
        BindingContext = _viewmodel;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewmodel.LoadUserAsync();
    }
}