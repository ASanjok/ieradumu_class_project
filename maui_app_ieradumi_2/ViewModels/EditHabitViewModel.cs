using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_app_ieradumi_2.Services;
using Microsoft.Maui.Storage; // Для получения userId, если нужно
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace maui_app_ieradumi_2.ViewModels
{
    [QueryProperty(nameof(HabitId), "habitId")]
    public partial class EditHabitViewModel : ObservableObject
    {
        private readonly IDataService _dataService;

        [ObservableProperty]
        private string habitId;

        [ObservableProperty]
        private Habit currentHabit;



        [ObservableProperty]
        private bool isSearchVisible;

        [ObservableProperty]
        private bool isFormVisible; 

        [ObservableProperty]
        private string searchQuery; 

        public ObservableCollection<Habit> SearchResults { get; set; } = new();

        [ObservableProperty]
        private Habit selectedSearchHabit;

        public EditHabitViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task LoadHabitAsync()
        {
            if (!string.IsNullOrEmpty(HabitId))
            {
                CurrentHabit = await _dataService.GetHabitAsync(HabitId);
                IsSearchVisible = false;
                IsFormVisible = true;
            }
            else
            {
                IsSearchVisible = true;
                IsFormVisible = false; 
            }
        }

        async partial void OnSearchQueryChanged(string value)
        {
            SearchResults.Clear();

            if (string.IsNullOrWhiteSpace(value) || value.Length < 2) return;

            var userId = await SecureStorage.GetAsync("LoggedInUserID");
            if (!string.IsNullOrEmpty(userId))
            {
                var allHabits = await _dataService.GetHabitListAsync(userId);

                var filtered = allHabits.Where(h => h.HabitName != null &&
                                                    h.HabitName.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();

                foreach (var habit in filtered)
                {
                    SearchResults.Add(habit);
                }
            }
        }

        partial void OnSelectedSearchHabitChanged(Habit value)
        {
            if (value != null)
            {
                CurrentHabit = value;
                HabitId = value.ID;

                IsSearchVisible = false;
                IsFormVisible = true;

                SearchQuery = string.Empty;
                SearchResults.Clear();
            }
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
        [RelayCommand]
        private void CloseButton()
        {
            IsSearchVisible = true;
            IsFormVisible = false;

            CurrentHabit = null;
        }
    }
}





//using class_project_ieradumi;
//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using maui_app_ieradumi_2.Services;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace maui_app_ieradumi_2.ViewModels
//{

//    [QueryProperty(nameof(HabitId), "habitId")]
//    public partial class EditHabitViewModel : ObservableObject
//    {

//        [ObservableProperty]
//        private string habitId;
//        [ObservableProperty]
//        private Habit currentHabit;

//        private readonly IDataService _dataService;

//        public EditHabitViewModel(IDataService dataService)
//        {
//            _dataService = dataService;
//        }



//        public async Task LoadHabitAsync()
//        {
//            CurrentHabit = await _dataService.GetHabitAsync(HabitId);
//        }

//        [RelayCommand]
//        private async Task SaveButton()
//        {
//            if (CurrentHabit != null)
//            {
//                await _dataService.EditHabitAsync(CurrentHabit.ID, CurrentHabit);

//                await Shell.Current.GoToAsync("//HabitListPage");
//            }
//        }
//    }
//}
