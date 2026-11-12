using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_app_ieradumi_2.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace maui_app_ieradumi_2.ViewModels
{

    [QueryProperty(nameof(HabitId), "habitId")]
    public partial class EditHabitViewModel : ObservableObject
    {

        [ObservableProperty]
        private string habitId;
        [ObservableProperty]
        private Habit currentHabit;

        private readonly IDataService _dataService;

        public EditHabitViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }



        public async Task LoadHabitAsync()
        {
            CurrentHabit = await _dataService.GetHabitAsync(HabitId);
        }

        [RelayCommand]
        private async Task SaveButton()
        {
            if (CurrentHabit != null)
            {
                await _dataService.EditHabitAsync(CurrentHabit.ID, CurrentHabit);

                await Shell.Current.GoToAsync("//HabitListPage");
            }
        }
    }
}
