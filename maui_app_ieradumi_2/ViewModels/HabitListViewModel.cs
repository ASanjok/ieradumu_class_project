using class_project_ieradumi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using maui_app_ieradumi_2.Views;
using maui_app_ieradumi_2.Views.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace maui_app_ieradumi_2.ViewModels
{
    public partial class HabitListViewModel : ObservableObject
    {
        [ObservableProperty]
        private string topText = "Ieradumi";

        [ObservableProperty]
        private string refreshButtonText = "Refresh";

        private readonly Services.IDataService _dataService;

        // Коллекция привычек, привязанная к визуальному списку на экране
        public ObservableCollection<Habit> Habits { get; set; } = new();

        public HabitListViewModel(Services.IDataService dataService)
        {
            _dataService = dataService;

            WeakReferenceMessenger.Default.Register<DeleteHabitMessage>(this, async (recipient, message) => // подписка на ожидание сообщения о удалении привычки (habitinfoelement.xaml.cs)
            {
                var habit = message.Value;
                await _dataService.DeleteHabitAsync(habit.ID);

                if (Habits.Contains(habit))
                {
                    Habits.Remove(habit);
                }
            });

            WeakReferenceMessenger.Default.Register<EditHabitMessage>(this, async (recipient, message) =>
            {
                var habit = message.Value;
                await EditHabitButton(habit);
            });
        }

        // загружает список привычек текущего пользователя
        public async Task LoadHabitsAsync()
        {
            System.Diagnostics.Debug.WriteLine("запущена функция LoadHabitsAsync");

            var userId = await SecureStorage.GetAsync("LoggedInUserID");

            System.Diagnostics.Debug.WriteLine("залогинен ID = " + userId);

            if (!string.IsNullOrEmpty(userId))
            {
                // Получаем список привычек для этого пользователя
                var habits = await _dataService.GetHabitListAsync(userId);

                // Очищаем локальную коллекцию и заполняем заново
                Habits.Clear();
                foreach (var habit in habits)
                {
                    Habits.Add(habit);
                    System.Diagnostics.Debug.WriteLine("в коллекцию добавлен " + habit.HabitName);
                }
            }
        }

        [RelayCommand]
        private async Task RefreshButton()
        {
            System.Diagnostics.Debug.WriteLine("запущена функция RefreshButton");
            await LoadHabitsAsync();
        }

        // Поля для формы создания новой привычки
        [ObservableProperty]
        private string newHabitName = "";

        [ObservableProperty]
        private string newHabitDescription = "";

        // Команда для сохранения и добавления новой привычки.
        [RelayCommand]
        private async Task AddHabit()
        {
            System.Diagnostics.Debug.WriteLine("запущена функция AddHabit");
            var userId = await SecureStorage.GetAsync("LoggedInUserID");

            if (!string.IsNullOrEmpty(userId))
            {
                await _dataService.SaveHabitAsync(NewHabitName, NewHabitDescription);

                // Обновляем список на экране
                await LoadHabitsAsync();
                System.Diagnostics.Debug.WriteLine("новая привычка добавлена в коллекцию");
            }
        }

        [RelayCommand]
        private async Task EditHabitButton(Habit habit)
        {
            System.Diagnostics.Debug.WriteLine("запущена EditHabitButtonCommand");
            if (habit == null) return;

            await Shell.Current.GoToAsync("//EditHabitPage?habitId=" + habit.ID);
        }

        [RelayCommand]
        private async Task DeleteHabitButton(Habit habit)
        {
            System.Diagnostics.Debug.WriteLine("запущена DeleteHabitButtonCommand");
            if (habit == null) return;

            await _dataService.DeleteHabitAsync(habit.ID);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Habits.Contains(habit))
                {
                    Habits.Remove(habit);
                    System.Diagnostics.Debug.WriteLine("Привычка успешно удалена с экрана");
                }
            });
        }

        [ObservableProperty]
        private string addHabitButtonText = "Pievienot ieradumu";

        [RelayCommand]
        private async Task AddHabitButton()
        {
            System.Diagnostics.Debug.WriteLine("запущена AddHabitButtonCommand");
            await Shell.Current.GoToAsync("//AddHabitPage");
        }
    }
}