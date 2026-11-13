using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_app_ieradumi_2.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace maui_app_ieradumi_2.ViewModels
{
    public partial class AddHabitViewModel : ObservableObject
    {
        [ObservableProperty]
        private Habit currentHabit;

        private readonly IDataService _dataService;

        public AddHabitViewModel(IDataService dataService)
        {
            _dataService = dataService;
            CurrentHabit = new Habit();
        }

        [RelayCommand]
        private async Task CreateButton()
        {
            if (CurrentHabit.HabitName != null && CurrentHabit.HabitDescription != null)
            {
                await _dataService.SaveHabitAsync(CurrentHabit.HabitName, CurrentHabit.HabitDescription, CurrentHabit.Archived);
                await Shell.Current.GoToAsync("//HabitListPage");
            }
        }
    }
}
