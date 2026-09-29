using ieradumu_class_project;
Console.WriteLine("program started\n\n");

Console.WriteLine("creating users\n");
var user1 = new user("user1", "user1@gmail.com", "password1");
program_data.user_list.Add(user1);
var user2 = new user("user2", "user2@gmail.com", "password2");
program_data.user_list.Add(user2);
var user3 = new user("user3", "user3@gmail.com", "password3");
program_data.user_list.Add(user3);
var user4 = new user("user4", "user4@gmail.com", "password4");
program_data.user_list.Add(user4);
var user5 = new user("user5", "user5@gmail.com", "password5");
program_data.user_list.Add(user5);
Console.WriteLine("users created\n\n");

Console.WriteLine("creating habits\n");
var habit1 = new habit("habit1", "description1", "user1");
program_data.habit_list.Add(habit1);
var habit2 = new habit("habit2", "description2", "user1");
program_data.habit_list.Add(habit2);
var habit3 = new habit("habit3", "description3", "user1");
program_data.habit_list.Add(habit3);
var habit4 = new habit("habit4", "description4", "user1");
program_data.habit_list.Add(habit4);
var habit5 = new habit("habit5", "description5", "user1");
program_data.habit_list.Add(habit5);
var habit6 = new habit("habit6", "description6", "user1");
program_data.habit_list.Add(habit6);


var habit7 = new habit("habit7", "description7", "user2");
program_data.habit_list.Add(habit7);
var habit8 = new habit("habit8", "description8", "user2");
program_data.habit_list.Add(habit8);       
var habit9 = new habit("habit9", "description9", "user2");
program_data.habit_list.Add(habit9);
var habit10 = new habit("habit10", "description10", "user2");
program_data.habit_list.Add(habit10);
var habit11 = new habit("habit11", "description11", "user2");
program_data.habit_list.Add(habit11);
var habit12 = new habit("habit12", "description12", "user2");
program_data.habit_list.Add(habit12);


var habit13 = new habit("habit13", "description13", "user3");
program_data.habit_list.Add(habit13);
var habit14 = new habit("habit14", "description14", "user3");
program_data.habit_list.Add(habit14);
var habit15 = new habit("habit15", "description15", "user3");
program_data.habit_list.Add(habit15);
var habit16 = new habit("habit16", "description16", "user3");
program_data.habit_list.Add(habit16);      
var habit17 = new habit("habit17", "description17", "user4");
program_data.habit_list.Add(habit17);
var habit18 = new habit("habit18", "description18", "user5");
program_data.habit_list.Add(habit18);
Console.WriteLine("habits created\n\n");

Console.WriteLine("archiving habits\n");
habit1.archive_habit();
habit3.archive_habit();
habit5.archive_habit();
habit7.archive_habit();
habit8.archive_habit();
habit9.archive_habit();
Console.WriteLine("habits archived\n\n");

Console.WriteLine("schedules creation\n");
var schedule1 = new schedule(user1.get_user_ID(), habit2.get_habit_ID(), true, true, true, true, true, false, false);
program_data.schedule_list.Add(schedule1);
var schedule2 = new schedule(user1.get_user_ID(), habit4.get_habit_ID(), true, true, true, true, true, false, false);
program_data.schedule_list.Add(schedule2);
var schedule3 = new schedule(user1.get_user_ID(), habit6.get_habit_ID(), true, true, true, true, true, false, false);
program_data.schedule_list.Add(schedule3);

var schedule4 = new schedule(user2.get_user_ID(), habit12.get_habit_ID(), true, true, true, true, true, false, false);
program_data.schedule_list.Add(schedule4);
var schedule5 = new schedule(user2.get_user_ID(), habit14.get_habit_ID(), true, true, true, true, true, false, false);
program_data.schedule_list.Add(schedule5);  

var schedule6 = new schedule(user3.get_user_ID(), habit6.get_habit_ID(), true, true, true, true, true, false, false);
program_data.schedule_list.Add(schedule6);

var schedule7 = new schedule(user4.get_user_ID(), habit2.get_habit_ID(), true, true, true, true, true, false, false);
program_data.schedule_list.Add(schedule7);
var schedule8 = new schedule(user4.get_user_ID(), habit15.get_habit_ID(), true, true, true, true, true, false, false);
program_data.schedule_list.Add(schedule8);
Console.WriteLine("schedules created\n\n");

var control_date = new DateTime(2026, 9, 28,14,30,00);

Console.WriteLine("habit completion records creation\n");
var habit_record1 = new habit_record("habit_record_1000", user1.get_user_ID(), habit1.get_habit_ID(), control_date.AddDays(-7));
program_data.habit_record_list.Add(habit_record1);
var habit_record2 = new habit_record("habit_record_1001", user1.get_user_ID(), habit1.get_habit_ID(), control_date.AddDays(-14));
program_data.habit_record_list.Add(habit_record2);
var habit_record3 = new habit_record("habit_record_1002", user1.get_user_ID(), habit1.get_habit_ID(), control_date.AddDays(-21));
program_data.habit_record_list.Add(habit_record3);
var habit_record4 = new habit_record("habit_record_1003", user2.get_user_ID(), habit12.get_habit_ID(), control_date);
program_data.habit_record_list.Add(habit_record4);
var habit_record5 = new habit_record("habit_record_1004", user3.get_user_ID(), habit6.get_habit_ID(), control_date);
program_data.habit_record_list.Add(habit_record5);
var habit_record6 = new habit_record("habit_record_1005", user4.get_user_ID(), habit2.get_habit_ID(), control_date);
program_data.habit_record_list.Add(habit_record6);
var habit_record7 = new habit_record("habit_record_1006", user4.get_user_ID(), habit15.get_habit_ID(), control_date);
program_data.habit_record_list.Add(habit_record7);
Console.WriteLine("habit completion records created\n");

Console.WriteLine("-------------------------------------------------\n\n");



Thread.Sleep(5000);
user3.change_user("user3_changed", "user3_changed@example.com", "");
Thread.Sleep(2000);
user5.change_user("aleksandrs", "", "");

foreach (var user in program_data.user_list)
{
    Console.WriteLine($"user data: {user.get_user_data()}");
}
Console.WriteLine("\n\n------------------------------------------------------------------------\n\n");
foreach (var habit in program_data.habit_list)
{
    Console.WriteLine($"habit data: {habit.get_habit_data()}");
}
Console.WriteLine("\n\n------------------------------------------------------------------------\n\n");
foreach (var schedule in program_data.schedule_list)
{
    Console.WriteLine($"schedule data: {schedule.get_schedule_data()}");
}
Console.WriteLine("\n\n------------------------------------------------------------------------\n\n");
foreach (var record in program_data.habit_record_list)
{
    Console.WriteLine($"habit record data: {record.get_habit_record_data()}");
}

