using System;
using System.Collections.Generic;
using System.Text;

namespace class_project_ieradumi
{
    internal class user
    {
        private string ID;
        private string username;
        private string email;
        private string password;
        private bool archived;
        private DateTime user_creation_date;
        private DateTime user_last_change_date;
        public user(string ID, string username, string email, string password, bool archived, DateTime user_creation_date, DateTime user_last_change_date) // Constructor for creating object of already existing user
        {
            string start_of_ID = ID.Substring(0, 8);
            if (start_of_ID != "user_ID_")
            {
                throw new ArgumentException("ID is not valid. It must start with 'user_ID_<unique_id>'.");
            }
            this.ID = ID;
            this.username = username;
            this.email = email;
            this.password = password;
            this.archived = archived;
            this.user_creation_date = user_creation_date;
            this.user_last_change_date = user_last_change_date;
        }
        public user(string username, string email, string password) // Constructor for creating new user object
        {
            this.ID = program.get_unique_ID_for_user();
            this.username = username;
            this.email = email;
            this.password = password;
            this.archived = false;
            this.user_creation_date = DateTime.Now;
            this.user_last_change_date = DateTime.Now;
        }
        // Getters for user class
        public string get_ID()
        {
            return this.ID;
        }
        public string get_username()
        {
            return this.username;
        }
        public string get_email()
        {
            return this.email;
        }
        public string get_password()
        {
            return this.password;
        }
        public DateTime get_user_creation_date()
        {
            return this.user_creation_date;
        }
        public DateTime get_user_last_change_date()
        {
            return this.user_last_change_date;
        }
        public bool is_archived()
        {
            return this.archived;
        }

        // setters for user class
        public void set_username(string username)
        {
            if(string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be empty or whitespace.");
            }
            this.username = username;
            this.user_last_change_date = DateTime.Now;
        }
        public void set_email(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty or whitespace.");
            }
            this.email = email;
            this.user_last_change_date = DateTime.Now;
        }
        public void set_password(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty or whitespace.");
            }
            this.password = password;
            this.user_last_change_date = DateTime.Now;
        }
        public void set_archived()
        {
            this.archived = true;
            this.user_last_change_date = DateTime.Now;
        }
        public void set_unarchived()
        {
            this.archived = false;
            this.user_last_change_date = DateTime.Now;
        }
    }
}
