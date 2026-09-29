using System;
using System.Collections.Generic;
using System.Text;

namespace shared_data_model_project
{
    public class create_user
    {
        public string user_name { get; set; }
        public string user_email { get; set; }
        public string user_password { get; set; }
    }
    public class show_user_full
    {
        public string user_ID { get; set; }
        public string user_name { get; set; }
        public string user_email { get; set; }
        public DateTime user_created_at { get; set; }
    }
    
    public class update_user    
    {
        public string user_ID { get; set; }
        public string user_name { get; set; }
        public string user_email { get; set; }
        public string user_password { get; set; }
    }
    public class delete_user
    {
        public string user_ID { get; set; }
    }
}
