using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_app_ieradumi_2.Services;
using System.Collections.ObjectModel;

namespace maui_app_ieradumi_2.ViewModels
{
    public partial class CompletionViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        public CompletionViewModel(IDataService dataService)
        {
            _dataService = dataService;
            selectedDate = DateTime.Today;
        }

        public ObservableCollection<HabitProgressDisplayItem> TasksList { get; set; } = new();

        [ObservableProperty]
        private DateTime selectedDate;

        [ObservableProperty]
        private bool isBusy;

        partial void OnSelectedDateChanged(DateTime value)
        {
            _ = LoadDataForSelectedDateAsync();
        }
        public async Task LoadDataForSelectedDateAsync()
        {
            if (IsBusy || _dataService == null) return;
            IsBusy = true;

            try
            {
                var userId = await Microsoft.Maui.Storage.SecureStorage.GetAsync("LoggedInUserID");
                if (string.IsNullOrEmpty(userId)) return;

                var schedule = await _dataService.GetScheduleByUserIdAsync(userId);
                var allScheduleItems = schedule?.HabitSchedules ?? new List<HabitSchedule>();

                var habits = await _dataService.GetHabitListAsync(userId) ?? new List<Habit>();
                var completions = await _dataService.GetCompletionsByUserAndDateAsync(userId, SelectedDate)
                                  ?? new List<CompletionRecord>();

                TasksList.Clear();

                string dayOfWeekStr = SelectedDate.DayOfWeek.ToString();
                var now = DateTime.Now;

                var activeItemsForDay = allScheduleItems
                    .Where(hs =>
                        hs.DayOfWeek == dayOfWeekStr &&
                        hs.CreatedAt.Date <= SelectedDate.Date &&
                        (hs.DeletedAt == null || hs.DeletedAt.Value.Date > SelectedDate.Date))
                    .OrderBy(hs => hs.ScheduledTime)
                    .ToList();

                foreach (var item in activeItemsForDay)
                {
                    var habitInfo = habits.FirstOrDefault(h => h.ID == item.HabitID);
                    string habitName = habitInfo?.HabitName ?? "Привычка";

                    var scheduledDateTime = SelectedDate.Date + item.ScheduledTime;
                    var scheduledTimeOnly = TimeOnly.FromTimeSpan(item.ScheduledTime);

                    var completionRecord = completions.FirstOrDefault(c =>
                        c.HabitID == item.HabitID &&
                        c.CompletionDate.Date == SelectedDate.Date);

                    string status;

                    if (completionRecord != null)
                    {
                        status = completionRecord.InTime ? "Completed" : "CompletedLate";
                    }
                    else if (SelectedDate.Date > now.Date)
                    {
                        status = "Upcoming"; // предстоит выполнить в будущем
                    }
                    else if (now > scheduledDateTime)
                    {
                        status = "Overdue"; // время прошло, отметки нет
                    }
                    else
                    {
                        status = "Pending"; // ещё предстоит сегодня
                    }

                    TasksList.Add(new HabitProgressDisplayItem
                    {
                        HabitID = item.HabitID,
                        HabitName = habitName,
                        ScheduledTime = scheduledTimeOnly,
                        ScheduledDateTime = scheduledDateTime,
                        Status = status
                    });
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CompleteTask(HabitProgressDisplayItem? task)
        {
            if (task == null || _dataService == null) return;
            if (task.Status is "Completed" or "CompletedLate") return;

            var userId = await Microsoft.Maui.Storage.SecureStorage.GetAsync("LoggedInUserID");
            if (string.IsNullOrEmpty(userId)) return;

            var now = DateTime.Now;
            var scheduledDateTime = task.ScheduledDateTime != default
                ? task.ScheduledDateTime
                : SelectedDate.Date + task.ScheduledTime.ToTimeSpan();

            // Вовремя, если выполняем не позже запланированного времени
            // (при желании можно добавить допуск, например scheduledDateTime.AddMinutes(30))
            bool isInTime = now <= scheduledDateTime;

            var scheduleDict = new Dictionary<string, TimeOnly>
            {
                { SelectedDate.DayOfWeek.ToString(), task.ScheduledTime }
            };

            var record = new CompletionRecord(task.HabitID, userId, scheduleDict)
            {
                CompletionDate = now, 
                InTime = isInTime,
                HasTheRecordBeenUsed = true
            };

            await _dataService.AddCompletionAsync(record.HabitID, record.UserID, record.CompletionDate, record.InTime, record.HasTheRecordBeenUsed, record.ScheduleTime);

            task.Status = isInTime ? "Completed" : "CompletedLate";
        }
    }

    public partial class HabitProgressDisplayItem : ObservableObject
    {
        public string HabitID { get; set; } = string.Empty;
        public string HabitName { get; set; } = string.Empty;
        public TimeOnly ScheduledTime { get; set; }
        public DateTime ScheduledDateTime { get; set; }

        [ObservableProperty]
        private string status = "Pending";
    }
}