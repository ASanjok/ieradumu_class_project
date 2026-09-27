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
            this.user_ID = "user_" + user_counter.ToString();
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
    }
}
