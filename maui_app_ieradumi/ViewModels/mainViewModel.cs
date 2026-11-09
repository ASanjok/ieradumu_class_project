using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_app_ieradumi.services;
using System.Collections.ObjectModel;

namespace maui_app_ieradumi.ViewModels
{
    // Модель для выбора конкретного дня и времени в UI
    public partial class DayScheduleItem : ObservableObject
    {
        public string DayName { get; set; }
        public string EnglishDayName { get; set; }

        [ObservableProperty]
        private bool isSelected;

        [ObservableProperty]
        private TimeSpan selectedTime = new(12, 0, 0); // 12:00 по умолчанию
    }

    public partial class mainViewModel : ObservableObject
    {
        private readonly IDataService _dataService;

        public mainViewModel(IDataService dataService)
        {
            _dataService = dataService ?? new DataService();
        }

        [ObservableProperty]
        private string topText1 = "ieradumi ";

        [ObservableProperty]
        private ObservableCollection<string> timeRanges = new()
        {
            "dienai", "nedeļai", "menesim", "visam laikam"
        };

        [ObservableProperty]
        private string selectedTimeRange;

        [ObservableProperty]
        private string topText2 = "nakotne";

        [ObservableProperty]
        private bool previouseMode = false;

        [ObservableProperty]
        private bool scheduleMode = false;

        [ObservableProperty]
        private bool nextMode = true;

        [ObservableProperty]
        private ObservableCollection<HabitDisplayItem> nextHabits = new();

        [ObservableProperty]
        private ObservableCollection<HabitDisplayItem> previousHabits = new();

        [ObservableProperty]
        private ObservableCollection<HabitDisplayItem> allSchedules = new();

        // --- ДНИ НЕДЕЛИ ДЛЯ ФОРМЫ СОЗДАНИЯ ---
        [ObservableProperty]
        private ObservableCollection<DayScheduleItem> weekDays = new()
        {
            new DayScheduleItem { DayName = "Pirmdiena"},
            new DayScheduleItem { DayName = "Otrdiena"},
            new DayScheduleItem { DayName = "Trešdiena"},
            new DayScheduleItem { DayName = "Ceturtdiena"},
            new DayScheduleItem { DayName = "Piektdiena"},
            new DayScheduleItem { DayName = "Sestdiena"},
            new DayScheduleItem { DayName = "Svētdiena"}
        };

        private List<Habit> _allHabits = new();
        private List<ScheduledHabits> _allSchedulesData = new();

        [RelayCommand]
        public void SwitchMode()
        {
            if (nextMode)
            {
                PreviouseMode = true;
                NextMode = false;
            }
            else
            {
                if (PreviouseMode)
                {
                    PreviouseMode = false;
                    ScheduleMode = true;
                }
                else
                {
                    ScheduleMode = false;
                    NextMode = true;
                }
            }

            TopText2 = PreviouseMode ? "pagātne" : (ScheduleMode ? "grafiks" : "nakotne");
        }

        [ObservableProperty]
        private string newHabbitName = string.Empty;

        [ObservableProperty]
        private string newHabbitDescription = string.Empty;

        [ObservableProperty]
        private string userID = "user_ID_1";

        [RelayCommand]
        public async Task AddHabitWithSchedule()
        {
            if (string.IsNullOrWhiteSpace(NewHabbitName)) return;

            // 1. Создаем привычку
            Habit newHabit = new Habit(NewHabbitName, NewHabbitDescription, UserID);
            _allHabits.Add(newHabit);

            // 2. Вспомогательная функция для получения списка времени дня
            List<TimeOnly> GetTimesForDay(string englishDayName)
            {
                var dayItem = WeekDays.FirstOrDefault(d => d.EnglishDayName == englishDayName);
                if (dayItem != null && dayItem.IsSelected)
                {
                    TimeOnly parsed = TimeOnly.FromTimeSpan(dayItem.SelectedTime);
                    return new List<TimeOnly> { parsed };
                }
                return new List<TimeOnly>();
            }

            // 3. Формируем списки времени для каждого дня
            var mondayTimes = GetTimesForDay("Monday");
            var tuesdayTimes = GetTimesForDay("Tuesday");
            var wednesdayTimes = GetTimesForDay("Wednesday");
            var thursdayTimes = GetTimesForDay("Thursday");
            var fridayTimes = GetTimesForDay("Friday");
            var saturdayTimes = GetTimesForDay("Saturday");
            var sundayTimes = GetTimesForDay("Sunday");

            // 4. Передаем их в конструктор scheduled_habits
            ScheduledHabits newSchedule = new ScheduledHabits(
                newHabit.get_unique_ID(),
                mondayTimes,
                tuesdayTimes,
                wednesdayTimes,
                thursdayTimes,
                fridayTimes,
                saturdayTimes,
                sundayTimes
            );

            _allSchedulesData.Add(newSchedule);

            // 5. Вызов DataService (ровно один раз)
            if (_dataService != null)
            {
                await _dataService.CreateNewHabitInScheduleAsync(newHabit.get_habit_name(), newHabit.get_habit_description(), UserID);
            }

            RefreshLists();

            // 6. Очищаем форму (ровно один раз)
            NewHabbitName = string.Empty;
            NewHabbitDescription = string.Empty;
            foreach (var day in WeekDays)
            {
                day.IsSelected = false;
                day.SelectedTime = new TimeSpan(12, 0, 0);
            }
        }

        public void RefreshLists()
        {
            NextHabits.Clear();
            PreviousHabits.Clear();
            AllSchedules.Clear();

            for (int i = 0; i < _allHabits.Count; i++)
            {
                var h = _allHabits[i];
                var s = _allSchedulesData.FirstOrDefault(x => x.GetHabitID() == h.get_unique_ID());

                string formattedSchedule = "Nav ieplānots";

                // ЗАЩИТА: проверяем, что расписание и сам словарь существуют (не null)
                if (s != null && s.get_when_is_scheduled() != null)
                {
                    var scheduleDict = s.get_when_is_scheduled();

                    var activeDays = scheduleDict
                                      .Where(kvp => kvp.Value != null && kvp.Value.Count > 0)
                                      .Select(kvp => $"{kvp.Key}: {string.Join(", ", kvp.Value)}")
                                      .ToList(); // Безопасно материализуем в список

                    if (activeDays.Any())
                    {
                        formattedSchedule = string.Join(" | ", activeDays);
                    }
                }

                var item = new HabitDisplayItem
                {
                    HabitObject = h,
                    ScheduleObject = s,
                    ScheduledTimeFormatted = formattedSchedule
                };

                NextHabits.Add(item);
                AllSchedules.Add(item);
            }
        }

        [RelayCommand]
        public void CompleteHabit(HabitDisplayItem item)
        {
            if (item == null || item.ScheduleObject == null) return;

            var originalSchedule = item.ScheduleObject.get_when_is_scheduled();
            Dictionary<string, TimeOnly> singleTimeSchedule = new();

            // ЗАЩИТА от null в словаре
            if (originalSchedule != null)
            {
                foreach (var kvp in originalSchedule)
                {
                    if (kvp.Value != null && kvp.Value.Count > 0)
                    {
                        singleTimeSchedule[kvp.Key] = kvp.Value[0];
                    }
                }
            }

            var record = new CompletionRecord(item.HabitId, UserID, singleTimeSchedule);

            NextHabits.Remove(item);
            item.IsCompleted = true;
            item.IsInTime = record.is_in_time();
            PreviousHabits.Add(item);
        }
    }
}