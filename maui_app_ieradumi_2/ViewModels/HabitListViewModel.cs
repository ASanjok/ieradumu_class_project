using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_app_ieradumi_2.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace maui_app_ieradumi_2.ViewModels
{
    public partial class HabitListViewModel : ObservableObject
    {
        [ObservableProperty]
        private string topText = "Ieradumi";
        [ObservableProperty]
        private string refreshButtonText = "Refresh";
        private readonly Services.IDataService _dataService;
        public ObservableCollection<Habit> Habits { get; set; } = new();
        public HabitListViewModel(Services.IDataService dataService)
        {
            _dataService = dataService;
        }
        public async Task LoadHabitsAsync()
        {
            System.Diagnostics.Debug.WriteLine("запусщена фукнция LoadHabitsAsync");
            if (Habits.Count > 0) return;

            var userId = await SecureStorage.GetAsync("LoggedInUserID");

            System.Diagnostics.Debug.WriteLine("залогинен ID = "+userId);
            if (!string.IsNullOrEmpty(userId))
            {
                var habits = await _dataService.GetHabitListAsync(userId);
                Habits.Clear();
                foreach (var habit in habits)
                {
                    Habits.Add(habit);
                    System.Diagnostics.Debug.WriteLine("в коллекцию добавлен "+ habit.HabitName);
                }
            }
        }
        

        [RelayCommand]
        private async Task RefreshButton()
        {
            System.Diagnostics.Debug.WriteLine("запусщена фукнция RefreshButton");
            await LoadHabitsAsync();
        }


        // create form for creating new habit
        [ObservableProperty]
        private string newHabitName = "";
        [ObservableProperty]
        private string newHabitDescription = "";

        [RelayCommand]
        private async Task AddHabit()
        {
            System.Diagnostics.Debug.WriteLine("запусщена фукнция AddHabit");
            var userId = await SecureStorage.GetAsync("LoggedInUserID");
            if (!string.IsNullOrEmpty(userId))
            {
                
                await _dataService.SaveHabitAsync(NewHabitName, NewHabitDescription);
                await LoadHabitsAsync();
                System.Diagnostics.Debug.WriteLine("новый привычка добавлена в коллекцию");
            }
        }




        [RelayCommand]
        private async Task EditHabitButton(Habit habit)
        {
            System.Diagnostics.Debug.WriteLine("запусчена EditHabitButtonCommand");
            if (habit == null) return;
            await Shell.Current.GoToAsync("//EditHabitPage?habitId=" + habit.ID);

        }
    }
}
