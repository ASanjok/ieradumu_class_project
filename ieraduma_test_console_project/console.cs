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


var habit7 = new habit("habit7", "description1", "user2");
program_data.habit_list.Add(habit7);
var habit8 = new habit("habit8", "description2", "user2");
program_data.habit_list.Add(habit8);       
var habit9 = new habit("habit9", "description3", "user2");
program_data.habit_list.Add(habit9);
var habit10 = new habit("habit10", "description4", "user2");
program_data.habit_list.Add(habit10);
var habit11 = new habit("habit11", "description5", "user2");
program_data.habit_list.Add(habit11);
var habit12 = new habit("habit12", "description6", "user2");
program_data.habit_list.Add(habit12);


var habit13 = new habit("habit13", "description1", "user3");
program_data.habit_list.Add(habit13);
var habit14 = new habit("habit14", "description2", "user3");
program_data.habit_list.Add(habit14);
var habit15 = new habit("habit15", "description3", "user3");
program_data.habit_list.Add(habit15);
var habit16 = new habit("habit16", "description4", "user3");
program_data.habit_list.Add(habit16);      
var habit17 = new habit("habit17", "description5", "user4");
program_data.habit_list.Add(habit17);
var habit18 = new habit("habit18", "description6", "user5");
program_data.habit_list.Add(habit18);


Console.WriteLine("habits created\n\n");

Console.WriteLine("-------------------------------------------------\n\n");

Console.WriteLine("shouldntn be able to create records\n");
habit_record habit_record1 = new habit_record("user_2_ID", "habit_1_ID");
habit_record habit_record2 = new habit_record("user_3_ID", "habit_3_ID");
Console.WriteLine("end of (shouldntn be able to create records)\n\n");

Console.WriteLine("should be able to create records\n");
schedule schedule1 = new schedule(user2.get_user_ID(), habit1.get_habit_ID(), true, false, false, false, false, false, false);
program_data.schedule_list.Add(schedule1);
habit_record habit_record3 = new habit_record(user2.get_user_ID(), habit1.get_habit_ID());
program_data.habit_record_list.Add(habit_record3);
Console.WriteLine("end of (should be able to create records)\n\n");

Console.WriteLine("shouldnt be able to create records\n");
schedule schedule2 = new schedule(user3.get_user_ID(), habit2.get_habit_ID(), true, false, false, false, false, false, false);
program_data.schedule_list.Add(schedule2);
habit2.archive_habit();
habit_record habit_record4 = new habit_record(user3.get_user_ID(), habit2.get_habit_ID());
program_data.habit_record_list.Add(habit_record4);
Console.WriteLine("end of (shouldnt be able to create records)\n\n");
