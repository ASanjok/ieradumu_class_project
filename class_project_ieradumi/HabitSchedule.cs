using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace class_project_ieradumi
{
    public class HabitSchedule
    {
        [PrimaryKey]
        public string ID { get; set; } = string.Empty; // Уникальный ID записи ("item_ID_...")

        [Indexed]
        public string ScheduleID { get; set; } = string.Empty; // Внешний ключ (FK) к Schedule

        [Indexed]
        public string HabitID { get; set; } = string.Empty; // Внешний ключ (FK) к Habit

        public string DayOfWeek { get; set; } = string.Empty; // "Monday", "Tuesday" и т.д.
        public TimeSpan ScheduledTime { get; set; } // Время (SQLite отлично работает с TimeSpan или string)

        public DateTime CreatedAt { get; set; } // Когда добавлено в расписание
        public DateTime? DeletedAt { get; set; } // NULL, если активно. Дата удаления, если удалено.

        public bool IsActive => DeletedAt == null;
        public HabitSchedule()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
