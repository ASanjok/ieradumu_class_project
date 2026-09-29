using System;
using System.Collections.Generic;
using System.Text;

namespace ieradumu_class_project
{
    public class user
    {
        private static int user_counter = 0;
        private string user_ID;
        private string user_name;
        private string user_email;
        private string user_password;
        private DateTime user_created_at;
        private DateTime user_updated_at;
        public user(string user_name, string user_email, string user_password)
        {
            user_counter++;
            this.user_ID = "user_" + user_counter.ToString()+"_ID";
            this.user_name = user_name;
            this.user_email = user_email;
            this.user_password = user_password;
            this.user_created_at = DateTime.Now;
            this.user_updated_at = DateTime.Now;
        }
        public string get_user_ID()
        {
            return this.user_ID;
        }

        public void change_user(string new_user_name, string new_user_email, string new_user_password)
        {
            if (!string.IsNullOrEmpty(new_user_name))
            {
                this.user_name = new_user_name;
            }
            if (!string.IsNullOrEmpty(new_user_email))
            {
                this.user_email = new_user_email;
            }
            if (!string.IsNullOrEmpty(new_user_password))
            {
                this.user_password = new_user_password;
            }
            if (string.IsNullOrEmpty(new_user_name) && string.IsNullOrEmpty(new_user_email) && string.IsNullOrEmpty(new_user_password))
            {
                throw new ArgumentException("user hasnt been changed. all name, email and password are null or empty.");
            }
            this.user_updated_at = DateTime.Now;
        }

        public string get_user_data()
        {
            return $"User ID: {this.user_ID}, Name: {this.user_name}, Email: {this.user_email}, Password: {this.user_password}, Created At: {this.user_created_at}, Updated At: {this.user_updated_at}";
        }
    }
}
