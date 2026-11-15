using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using maui_app_ieradumi_2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static Android.Net.Http.SslCertificate;

namespace maui_app_ieradumi_2.ViewModels
{
    public partial class ScheduleViewModel : ObservableObject
    {

        private readonly IDataService _dataService;

        [ObservableProperty]
        private Schedule currentSchedule = new()
        {
            ID = "Загрузка...",
            UserID = "...",
            ScheduledHabits = new()
        };

        public ScheduleViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task LoadScheduleAsync()
        {
            var userId = await Microsoft.Maui.Storage.SecureStorage.GetAsync("LoggedInUserID");
            System.Diagnostics.Debug.WriteLine($"LoadScheduleAsync: Загрузка для пользователя {userId}");

            if (!string.IsNullOrEmpty(userId))
            {
                var scheduleFromDb = await _dataService.GetScheduleByUserIdAsync(userId);

                if (scheduleFromDb != null)
                {
                    CurrentSchedule = scheduleFromDb;
                }
            }

            RebuildDaysList();
        }

        [ObservableProperty]
        private ObservableCollection<DayScheduleGroup> daysOfWeek = new();


        private async void RebuildDaysList()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("\tRebuildDaysList ======= 1");

                if (DaysOfWeek == null)
                {
                    DaysOfWeek = new ObservableCollection<DayScheduleGroup>();
                    System.Diagnostics.Debug.WriteLine("\tRebuildDaysList ======= 1.5");
                }
                System.Diagnostics.Debug.WriteLine("\tRebuildDaysList ======= 1.7");

                DaysOfWeek.Clear();
                System.Diagnostics.Debug.WriteLine("\tRebuildDaysList ======= 2");

                string[] allDays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

                System.Diagnostics.Debug.WriteLine("\tRebuildDaysList ======= 3");
                string userId = string.Empty;
                try
                {
                    userId = await Microsoft.Maui.Storage.SecureStorage.GetAsync("LoggedInUserID") ?? string.Empty;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка SecureStorage: {ex.Message}");
                }

                System.Diagnostics.Debug.WriteLine($"\tRebuildDaysList ======= 4 (UserID: {userId})");

                List<Habit> allUserHabits = new();
                if (_dataService != null && !string.IsNullOrEmpty(userId))
                {
                    try
                    {
                        var habitsResult = await _dataService.GetHabitListAsync(userId);
                        if (habitsResult != null)
                        {
                            allUserHabits = habitsResult;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Ошибка GetHabitListAsync: {ex.Message}");
                    }
                }
                System.Diagnostics.Debug.WriteLine("\tRebuildDaysList ======= 5");

                foreach (var day in allDays)
                {
                    var dayGroup = new DayScheduleGroup { DayName = day };

                    if (CurrentSchedule?.ScheduledHabits != null)
                    {
                        foreach (var habit in CurrentSchedule.ScheduledHabits)
                        {
                            if (habit != null && habit.WhenIsScheduled != null && habit.WhenIsScheduled.ContainsKey(day))
                            {
                                var foundHabit = allUserHabits.FirstOrDefault(h => h.ID == habit.HabitID);
                                string habitName = foundHabit?.HabitName ?? habit.HabitID ?? "Привычка";

                                if (habit.WhenIsScheduled[day] != null)
                                {
                                    foreach (var time in habit.WhenIsScheduled[day])
                                    {
                                        dayGroup.ScheduledItems.Add(new DayScheduleItem
                                        {
                                            HabitID = habit.HabitID ?? string.Empty,
                                            HabitName = habitName,
                                            Time = time,
                                            DayName = day
                                        });
                                    }
                                }
                            }
                        }

                        dayGroup.ScheduledItems = dayGroup.ScheduledItems.OrderBy(i => i.Time).ToList();
                    }

                    DaysOfWeek.Add(dayGroup);
                }
                System.Diagnostics.Debug.WriteLine("\tRebuildDaysList ======= УСПЕШНО ЗАВЕРШЕНО");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"КРИТИЧЕСКАЯ ОШИБКА В RebuildDaysList: {ex.Message} \n {ex.StackTrace}");
            }
        }



        [RelayCommand]
        private async Task AddToDay(string dayName)
        {
            System.Diagnostics.Debug.WriteLine($"Нажата кнопка + для дня: {dayName}");

            if (CurrentSchedule == null || string.IsNullOrEmpty(CurrentSchedule.ID))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Расписание еще не загружено", "ОК");
                return;
            }

            await Shell.Current.GoToAsync($"//AddHabitToSchedulePage?day={dayName}&scheduleId={CurrentSchedule.ID}");
        }



        

        //[RelayCommand]
        //private async Task RemoveTime(DayScheduleItem item)
        //{
        //    if (item == null || CurrentSchedule == null) return;
        //    await Shell.Current.GoToAsync($"//SchedulePage");
        //    CurrentSchedule.RemoveTimeFromScheduledHabit(item.HabitID, item.DayName, new List<TimeOnly> { item.Time });
        //    _ = _dataService.UpdateScheduleAsync(CurrentSchedule);
        //    System.Diagnostics.Debug.WriteLine($"Успешно удалено время {item.Time}");
        //    RebuildDaysList();
        //}



        [RelayCommand]
        private async Task RemoveTime(DayScheduleItem item)
        {
            if (item == null || CurrentSchedule == null) return;
            CurrentSchedule.RemoveTimeFromScheduledHabit(item.HabitID, item.DayName, new List<TimeOnly> { item.Time });
            _ = _dataService.UpdateScheduleAsync(CurrentSchedule);
            System.Diagnostics.Debug.WriteLine($"Успешно удалено время {item.Time}");
            await Shell.Current.GoToAsync($"//DeleteHabitFromSchedulePage");
        }


    }
    public class DayScheduleGroup
    {
        public string DayName { get; set; }
        public List<DayScheduleItem> ScheduledItems { get; set; } = new();
    }

    public class DayScheduleItem
    {
        public string HabitID { get; set; } = string.Empty;
        public string HabitName { get; set; } = string.Empty;
        public TimeOnly Time { get; set; }
        public string DayName { get; set; } = string.Empty;
    }
}