using class_project_ieradumi;

namespace maui_app_ieradumi.ViewModels
{
    public class HabitDisplayItem
    {
        public Habit HabitObject { get; set; }
        public ScheduledHabits ScheduleObject { get; set; }

        public string Title => HabitObject?.get_habit_name() ?? string.Empty;
        public string Description => HabitObject?.get_habit_description() ?? string.Empty;
        public string HabitId => HabitObject?.get_unique_ID() ?? string.Empty;

        // Поля для UI и истории
        public bool IsCompleted { get; set; }
        public bool IsInTime { get; set; }
        public string ScheduledTimeFormatted { get; set; }
    }
}