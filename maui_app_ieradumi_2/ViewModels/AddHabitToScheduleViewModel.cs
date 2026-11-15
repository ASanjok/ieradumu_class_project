using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_app_ieradumi_2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace maui_app_ieradumi_2.ViewModels
{
    [QueryProperty(nameof(Day), "day")]
    [QueryProperty(nameof(ScheduleId), "scheduleId")]
    public partial class AddHabitToScheduleViewModel : ObservableObject
    {
        private readonly IDataService _dataService;

        [ObservableProperty]
        private string day;

        [ObservableProperty]
        private string scheduleId;

        [ObservableProperty]
        private string pageTitle;

        public ObservableCollection<Habit> AvailableHabits { get; set; } = new();

        [ObservableProperty]
        private Habit selectedHabit;

        [ObservableProperty]
        private TimeSpan selectedTime = DateTime.Now.TimeOfDay;

        public AddHabitToScheduleViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task LoadDataAsync()
        {
            PageTitle = $"Добавить на: {Day}";

            AvailableHabits.Clear();
            var userId = await SecureStorage.GetAsync("LoggedInUserID");
            System.Diagnostics.Debug.WriteLine($"LoadDataAsync\nпользователь ид - "+userId);
            if (!string.IsNullOrEmpty(userId))
            {
                var habits = await _dataService.GetHabitListAsync(userId);
                foreach (var habit in habits)
                {
                    AvailableHabits.Add(habit);
                }
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            System.Diagnostics.Debug.WriteLine("запущена save");
            if (SelectedHabit == null)
            {
                System.Diagnostics.Debug.WriteLine("запущена save 1");
                await Shell.Current.DisplayAlertAsync("Ошибка", "Пожалуйста, выберите привычку", "ОК");
                return;
            }

            if (string.IsNullOrEmpty(ScheduleId))
            {
                System.Diagnostics.Debug.WriteLine("запущена save 2");
                await Shell.Current.DisplayAlertAsync("Ошибка", "Не найден ID расписания", "ОК");
                return;
            }

            var schedule = await _dataService.GetScheduleByUserIdAsync(await SecureStorage.GetAsync("LoggedInUserID"));

            System.Diagnostics.Debug.WriteLine("запущена save 3");
            if (schedule != null)
            {
                System.Diagnostics.Debug.WriteLine("запущена save 4");
                TimeOnly timeOnly = TimeOnly.FromTimeSpan(SelectedTime);
                List<TimeOnly> timeList = new List<TimeOnly> { timeOnly };

                schedule.AddTimeToScheduledHabit(SelectedHabit.ID, Day, timeList);

                await _dataService.UpdateScheduleAsync(schedule);

                await Shell.Current.GoToAsync("//SchedulePage");
            }
        }
    }

}
