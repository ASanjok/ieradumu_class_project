using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_app_ieradumi_2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
            UserID = "..."
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
                System.Diagnostics.Debug.WriteLine("\tRebuildDaysList ======= Старт (Реляционная версия)");

                if (DaysOfWeek == null)
                {
                    DaysOfWeek = new ObservableCollection<DayScheduleGroup>();
                }

                DaysOfWeek.Clear();

                string[] allDays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

                string userId = string.Empty;
                try
                {
                    userId = await Microsoft.Maui.Storage.SecureStorage.GetAsync("LoggedInUserID") ?? string.Empty;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка SecureStorage: {ex.Message}");
                }

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

                var newDaysOfWeek = new ObservableCollection<DayScheduleGroup>();

                foreach (var day in allDays)
                {
                    var dayGroup = new DayScheduleGroup { DayName = day };
                    var itemsForDay = new List<DayScheduleItem>();

                    // Берем активные элементы из реляционного списка CurrentSchedule.HabitSchedules
                    if (CurrentSchedule?.HabitSchedules != null)
                    {
                        var daySchedules = CurrentSchedule.HabitSchedules
                            .Where(hs => hs.DayOfWeek == day && hs.IsActive);

                        foreach (var scheduleItem in daySchedules)
                        {
                            var foundHabit = allUserHabits.FirstOrDefault(h => h.ID == scheduleItem.HabitID);
                            string habitName = foundHabit?.HabitName ?? "Привычка";

                            itemsForDay.Add(new DayScheduleItem
                            {
                                HabitID = scheduleItem.HabitID,
                                HabitName = habitName,
                                Time = TimeOnly.FromTimeSpan(scheduleItem.ScheduledTime),
                                DayName = day
                            });
                        }
                    }

                    // Сортируем по времени
                    dayGroup.ScheduledItems = itemsForDay.OrderBy(i => i.Time).ToList();
                    newDaysOfWeek.Add(dayGroup);
                }

                DaysOfWeek = newDaysOfWeek;
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

        [RelayCommand]
        private async Task RemoveTime(DayScheduleItem item)
        {
            if (item == null || CurrentSchedule?.HabitSchedules == null) return;

            // 1. Ищем конкретный элемент в памяти по HabitID, Дню недели и Времени
            var targetRecord = CurrentSchedule.HabitSchedules.FirstOrDefault(hs =>
                hs.HabitID == item.HabitID &&
                hs.DayOfWeek == item.DayName &&
                hs.ScheduledTime == item.Time.ToTimeSpan() &&
                hs.IsActive);

            if (targetRecord != null)
            {
                // 2. Выполняем мягкое удаление (ставим отметку DeletedAt)
                targetRecord.DeletedAt = DateTime.Now;

                // 3. Сохраняем изменения в базу через обновленный сервис
                await _dataService.UpdateScheduleAsync(CurrentSchedule);
                System.Diagnostics.Debug.WriteLine($"Успешно удалено время {item.Time} (мягкое удаление)");

                // 4. Перестраиваем список на экране
                await Shell.Current.GoToAsync("//DeleteHabitFromSchedulePage");
            }
        }
    }

    public class DayScheduleGroup
    {
        public string DayName { get; set; } = string.Empty;
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