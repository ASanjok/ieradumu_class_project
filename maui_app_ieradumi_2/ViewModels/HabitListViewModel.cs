using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
                var habits = await _dataService.GetHabitList(userId);
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
    }
}
