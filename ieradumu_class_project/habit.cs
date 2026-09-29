using System;
using System.Collections.Generic;
using System.Text;

namespace ieradumu_class_project
{
    public class habit
    {
        private static int habit_counter = 0; // static counter to keep track of the number of habits created
                                              // a temporary solution for unique IDs
        private string habit_ID;
        private string creator_user_ID;
        private string habit_name;
        private string habit_description;
        private bool habit_status; // (Bool) 0|false - archived, 1|true - active
        private DateTime habit_created_at;
        private DateTime habit_updated_at;

        public habit(string habit_name, string habit_description, string creator_user_ID) //constructor for creating a new habit
        {
            if (string.IsNullOrEmpty(habit_name))
            {
                throw new ArgumentException("Habit name cannot be null or empty.");
            }
            if (habit_name.Length > 40)
            {
                throw new ArgumentException("Habit name cannot exceed 40 characters.");
            }
            if (string.IsNullOrEmpty(creator_user_ID))
            {
                throw new ArgumentException("Creator user ID cannot be null or empty.");
            }
            habit_counter++;
            this.habit_ID = "habit_" + habit_counter.ToString()+"_ID";
            this.creator_user_ID = creator_user_ID;
            this.habit_name = habit_name;
            this.habit_description = habit_description;
            this.habit_status = true; // Active by default
            this.habit_created_at = DateTime.Now;
            this.habit_updated_at = DateTime.Now;
        }
        public habit(string habit_ID, string habit_name, string habit_description, bool habit_status, string creator_user_ID, DateTime habit_created_at, DateTime habit_updated_at) // constructor for loading habit data from "DB"
        {
            this.habit_ID = habit_ID;
            this.creator_user_ID = creator_user_ID;
            this.habit_name = habit_name;
            this.habit_description = habit_description;
            this.habit_status = habit_status;
            this.habit_created_at = habit_created_at;
            this.habit_updated_at = habit_updated_at;
        }
        public void archive_habit()
        {
            this.habit_status = false;
            this.habit_updated_at = DateTime.Now;
        }
        public void change_habit(string new_habit_name, string new_habit_description)
        {
            if (string.IsNullOrEmpty(new_habit_name) && string.IsNullOrEmpty(new_habit_description)) //if both the new habit name and description are null or empty, throw an exception
            {
                throw new ArgumentException("habit hasnt been changed. both name and description are null or empty.");
            }
            if (!string.IsNullOrEmpty(new_habit_name)) //if the new habit name is not null or empty, change the habit name
            {
                this.habit_name = new_habit_name;
            }
            if (!string.IsNullOrEmpty(new_habit_description)) //if the new habit description is not null or empty, change the habit description
            {
                this.habit_description = new_habit_description;
            }
            this.habit_updated_at = DateTime.Now;
        }

        public string get_habit_ID()
        {
            return this.habit_ID;
        }
        public string get_habit_name()
        {
            return this.habit_name;
        }
        public bool get_habit_status()
        {
            return this.habit_status;
        }
        public string get_habit_creator_user_ID()
        {
            return this.creator_user_ID;
        }
        public string get_habit_creation_date()
        {
            return this.habit_created_at.ToString();
        }
        public string get_habit_last_updated_date()
        {
            return this.habit_updated_at.ToString();
        }
        public string get_habit_data()
        {
            return $"Habit ID: {this.habit_ID}, Name: {this.habit_name}, Description: {this.habit_description}, Status: {(this.habit_status ? "Active" : "Archived")}, Created At: {this.habit_created_at}, Updated At: {this.habit_updated_at}";
        }

    }
}
