using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_app_ieradumi_2.Services;
using System.Collections.ObjectModel;

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

        public async Task LoadScheduleAsync() // метод для загрузки schedule и HabitSchedule
        {
            var userId = await SecureStorage.GetAsync("LoggedInUserID");
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

        private async void RebuildDaysList() // метод для перестройки списка дней недели с привязанными привычками
        {
            System.Diagnostics.Debug.WriteLine("\tRebuildDaysList ======= Старт");

            // Если коллекция дней недели еще не инициализирована, создаем её
            if (DaysOfWeek == null)
            {
                DaysOfWeek = new ObservableCollection<DayScheduleGroup>();
            }

            // Очищаем текущую коллекцию перед заполнением новыми данными
            DaysOfWeek.Clear();

            string[] allDays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            string userId = string.Empty;
            userId = await SecureStorage.GetAsync("LoggedInUserID") ?? string.Empty;

            // Список для хранения всех привычек пользователя
            List<Habit> allUserHabits = new();

            // Если сервис данных доступен и ID пользователя найден, запрашиваем список привычек
            if (_dataService != null && !string.IsNullOrEmpty(userId))
            {
                var habitsResult = await _dataService.GetHabitListAsync(userId);
                if (habitsResult != null)
                {
                    allUserHabits = habitsResult;
                }
            }

            // Проходим по каждому дню недели и сразу добавляем группы в DaysOfWeek
            foreach (var day in allDays)
            {
                var dayGroup = new DayScheduleGroup { DayName = day };
                var itemsForDay = new List<DayScheduleItem>();

                // Проверяем, загружено ли расписание и содержатся ли в нем привязки привычек
                if (CurrentSchedule?.HabitSchedules != null)
                {
                    // Выбираем только те записи расписания, которые относятся к текущему дню и являются активными (не удалены)
                    var daySchedules = CurrentSchedule.HabitSchedules
                        .Where(hs => hs.DayOfWeek == day && hs.IsActive);

                    foreach (var scheduleItem in daySchedules)
                    {
                        // Ищем имя привычки по её ID среди всех привычек пользователя
                        var foundHabit = allUserHabits.FirstOrDefault(h => h.ID == scheduleItem.HabitID);
                        string habitName = foundHabit?.HabitName ?? "Привычка";

                        // Добавляем элемент в список элементов конкретного дня
                        itemsForDay.Add(new DayScheduleItem
                        {
                            HabitID = scheduleItem.HabitID,
                            HabitName = habitName,
                            Time = TimeOnly.FromTimeSpan(scheduleItem.ScheduledTime), // Конвертируем TimeSpan в TimeOnly
                            DayName = day
                        });
                    }
                }

                // Сортируем привычки внутри дня по времени и сохраняем в группу
                dayGroup.ScheduledItems = itemsForDay.OrderBy(i => i.Time).ToList();

                // Добавляем готовую группу прямо в основную коллекцию
                DaysOfWeek.Add(dayGroup);
            }

            // Логируем успешное завершение работы метода
            System.Diagnostics.Debug.WriteLine("\tRebuildDaysList ======= УСПЕШНО ЗАВЕРШЕНО");
        }

        [RelayCommand]
        private async Task AddToDay(string dayName) // метод для кнопки + для дня недели 
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
            if (item == null || CurrentSchedule == null) return;

            await _dataService.SoftDeleteHabitScheduleAsync(CurrentSchedule.ID, item.HabitID, item.DayName, item.Time.ToTimeSpan());

            System.Diagnostics.Debug.WriteLine($"Успешно удалено время {item.Time} (мягкое удаление)");

            await Shell.Current.GoToAsync("//DeleteHabitFromSchedulePage");
        }
    }

    // класс нужен для того чтобы отображать день недели и хранить в ней привычки которые нужно отобразить у отдельного дня
    public class DayScheduleGroup
    {
        public string DayName { get; set; } = string.Empty;
        public List<DayScheduleItem> ScheduledItems { get; set; } = new();
    }

    // класс нужен для того чтобы отображать отдельные привычки их время на которое они запланированы
    public class DayScheduleItem
    {
        public string HabitID { get; set; } = string.Empty;
        public string HabitName { get; set; } = string.Empty;
        public TimeOnly Time { get; set; }
        public string DayName { get; set; } = string.Empty;
    }
}