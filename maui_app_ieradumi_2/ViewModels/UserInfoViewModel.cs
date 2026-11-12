using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_app_ieradumi_2.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace maui_app_ieradumi_2.ViewModels
{
    public partial class UserInfoViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        [ObservableProperty]
        private User currentUser;
        public UserInfoViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }
        public async Task LoadUserAsync()
        {
            System.Diagnostics.Debug.WriteLine("Запущена функция LoadUserAsync");

            var userId = await SecureStorage.GetAsync("LoggedInUserID");

            if (!string.IsNullOrEmpty(userId))
            {
                CurrentUser = await _dataService.GetUserByIdAsync(userId);
                System.Diagnostics.Debug.WriteLine($"Пользователь {CurrentUser?.Username} загружен");
            }
        }

        [RelayCommand]
        private async Task SaveUser()
        {
            System.Diagnostics.Debug.WriteLine("Запущена команда SaveUser");

            if (CurrentUser != null)
            {

                await _dataService.UpdateUserAsync(CurrentUser);
                await LoadUserAsync();
            }
        }
    }
}
