
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
        public CompletionViewModel()
        {
        }
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
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var userId = await Microsoft.Maui.Storage.SecureStorage.GetAsync("LoggedInUserID");
                if (string.IsNullOrEmpty(userId)) return;

                var schedule = await _dataService.GetScheduleByUserIdAsync(userId);
                if (schedule == null) return;

                var allScheduleItems = schedule.HabitSchedules ?? new List<HabitSchedule>();

                var habits = await _dataService.GetHabitListAsync(userId) ?? new();

                var completions = new List<CompletionRecord>(); 

                TasksList.Clear();

                string dayOfWeekStr = SelectedDate.DayOfWeek.ToString(); 
                var now = DateTime.Now;

                var activeItemsForDay = allScheduleItems.Where(hs =>
                    hs.DayOfWeek == dayOfWeekStr &&
                    hs.CreatedAt.Date <= SelectedDate.Date &&
                    (hs.DeletedAt == null || hs.DeletedAt.Value.Date > SelectedDate.Date)
                ).ToList();

                foreach (var item in activeItemsForDay)
                {
                    var habitInfo = habits.FirstOrDefault(h => h.ID == item.HabitID);
                    string habitName = habitInfo?.HabitName ?? "Привычка";

                    var scheduledDateTime = SelectedDate.Date + item.ScheduledTime;

                    var completionRecord = completions.FirstOrDefault(c =>
                        c.HabitID == item.HabitID &&
                        c.CompletionDate.Date == SelectedDate.Date);

                    string status = "Pending"; 

                    if (completionRecord != null)
                    {
                        status = completionRecord.InTime ? "Completed" : "CompletedLate";
                    }
                    else if (scheduledDateTime < now)
                    {
                        
                        status = "Overdue";
                    }

                    if (SelectedDate.Date > now.Date)
                    {
                        status = "Pending";
                    }

                    TasksList.Add(new HabitProgressDisplayItem
                    {
                        HabitID = item.HabitID,
                        HabitName = habitName,
                        ScheduledTime = TimeOnly.FromTimeSpan(item.ScheduledTime),
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
        private async Task CompleteTask(HabitProgressDisplayItem task)
        {
            if (task == null) return;

            var userId = await Microsoft.Maui.Storage.SecureStorage.GetAsync("LoggedInUserID");
            if (string.IsNullOrEmpty(userId)) return;

            var scheduleDict = new Dictionary<string, TimeOnly>
            {
                { SelectedDate.DayOfWeek.ToString(), task.ScheduledTime }
            };

            var record = new CompletionRecord(task.HabitID, userId, scheduleDict)
            {
                CompletionDate = SelectedDate.Date + task.ScheduledTime.ToTimeSpan(),
                HasTheRecordBeenUsed = true
            };
            task.Status = "Completed";
        }
    }
    public partial class HabitProgressDisplayItem : ObservableObject
    {
        public string HabitID { get; set; } = string.Empty;
        public string HabitName { get; set; } = string.Empty;
        public TimeOnly ScheduledTime { get; set; }

        [ObservableProperty]
        private string status = "Pending";
    }
}
