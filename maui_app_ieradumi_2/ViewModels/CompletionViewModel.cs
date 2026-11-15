
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
            selectedDate = DateTime.Today; // По умолчанию выбран сегодняшний день
        }

        // Коллекция задач для отображения на экране
        public ObservableCollection<HabitProgressDisplayItem> TasksList { get; set; } = new();

        [ObservableProperty]
        private DateTime selectedDate;

        [ObservableProperty]
        private bool isBusy;

        // Метод вызывается при изменении даты (например, через DatePicker)
        partial void OnSelectedDateChanged(DateTime value)
        {
            _ = LoadDataForSelectedDateAsync();
        }

        // Загрузка данных и расчет статусов для выбранного дня
        public async Task LoadDataForSelectedDateAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var userId = await Microsoft.Maui.Storage.SecureStorage.GetAsync("LoggedInUserID");
                if (string.IsNullOrEmpty(userId)) return;

                // 1. Получаем расписание пользователя и его активные привязки HabitSchedule
                var schedule = await _dataService.GetScheduleByUserIdAsync(userId);
                if (schedule == null) return;

                // Загружаем все элементы расписания
                var allScheduleItems = schedule.HabitSchedules ?? new List<HabitSchedule>();

                // 2. Получаем список всех привычек (для имен)
                var habits = await _dataService.GetHabitListAsync(userId) ?? new();

                // 3. Получаем реальные отметки выполнения (CompletionRecord) из базы
                // (Предполагается, что в IDataService есть метод получения записей выполнения за дату)
                // var completions = await _dataService.GetCompletionRecordsByDateAsync(userId, SelectedDate);
                var completions = new List<CompletionRecord>(); // Заглушка, если метод еще не написан

                TasksList.Clear();

                string dayOfWeekStr = SelectedDate.DayOfWeek.ToString(); // Например, "Monday"
                var now = DateTime.Now;

                // Фильтруем привязки:
                // - Нужный день недели
                // - Привычка была создана/добавлена ДО или В ЭТОТ день (CreatedAt <= SelectedDate)
                // - Привычка еще не была удалена на этот день (DeletedAt == null ИЛИ DeletedAt > SelectedDate)
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

                    // Ищем, выполнена ли эта привычка в этот день
                    var completionRecord = completions.FirstOrDefault(c =>
                        c.HabitID == item.HabitID &&
                        c.CompletionDate.Date == SelectedDate.Date);

                    string status = "Pending"; // По умолчанию: предстоит выполнить

                    if (completionRecord != null)
                    {
                        status = completionRecord.InTime ? "Completed" : "CompletedLate";
                    }
                    else if (scheduledDateTime < now)
                    {
                        // Если время уже прошло, а отметки нет -> Просрочено / Пропущено
                        status = "Overdue";
                    }

                    // Если выбранный день в будущем — статус всегда Pending
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

        // Команда отметки привычки как выполненной
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

            // Создаем вашу модель CompletionRecord (она сама посчитает InTime)
            var record = new CompletionRecord(task.HabitID, userId, scheduleDict)
            {
                CompletionDate = SelectedDate.Date + task.ScheduledTime.ToTimeSpan(),
                HasTheRecordBeenUsed = true
            };

            // TODO: Сохраняем в базу данных через сервис
            // await _dataService.SaveCompletionRecordAsync(record);

            task.Status = "Completed";
        }
    }
    public partial class HabitProgressDisplayItem : ObservableObject
    {
        public string HabitID { get; set; } = string.Empty;
        public string HabitName { get; set; } = string.Empty;
        public TimeOnly ScheduledTime { get; set; }

        [ObservableProperty]
        private string status = "Pending"; // Pending (предстоит), Completed (выполнено), Overdue (просрочено)
    }
}
