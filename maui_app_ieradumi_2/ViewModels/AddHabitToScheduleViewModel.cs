using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_app_ieradumi_2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

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
            System.Diagnostics.Debug.WriteLine($"LoadDataAsync\nпользователь ид - " + userId);

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
                await Shell.Current.DisplayAlert("Ошибка", "Пожалуйста, выберите привычку", "ОК");
                return;
            }

            if (string.IsNullOrEmpty(ScheduleId))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Не найден ID расписания", "ОК");
                return;
            }
            var userId = await SecureStorage.GetAsync("LoggedInUserID");
            var schedule = await _dataService.GetScheduleByUserIdAsync(userId);

            if (schedule != null)
            {
                schedule.HabitSchedules ??= new List<HabitSchedule>();

                var newHabitSchedule = new HabitSchedule
                {
                    ID = "item_ID_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                    ScheduleID = schedule.ID,
                    HabitID = SelectedHabit.ID,
                    DayOfWeek = Day, // Например, "Monday"
                    ScheduledTime = SelectedTime,
                    CreatedAt = DateTime.Now,
                    DeletedAt = null
                };

                schedule.HabitSchedules.Add(newHabitSchedule);

                // Сохраняем расписание и дочерний элемент в базу
                await _dataService.UpdateScheduleAsync(schedule);

                System.Diagnostics.Debug.WriteLine("Привычка успешно сохранена в базу!");
                await Shell.Current.GoToAsync("//SchedulePage");
            }
        }
    }
}