using SQLite;

public class Schedule
{
    [PrimaryKey]
    public string ID { get; set; } = string.Empty; // "schedule_ID_..."
    public string UserID { get; set; } = string.Empty;
    public bool Archived { get; set; }
    public DateTime ScheduleCreationDate { get; set; }
    public DateTime ScheduleLastChangeDate { get; set; }
    public Schedule()
    {

        ScheduleCreationDate = DateTime.Now;
        ScheduleLastChangeDate = DateTime.Now;
    }
}