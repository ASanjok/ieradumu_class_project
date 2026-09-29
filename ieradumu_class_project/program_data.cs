using System;
using System.Collections.Generic;
using System.Text;

namespace ieradumu_class_project
{
    public class program_data // class to store all the data in memory
                              // in the future, need to implement tool for clearing memory 
                              //                    OR
                              // do to something that im not aware of, right now
    {
        public static List<habit> habit_list = new List<habit>();
        public static List<habit_record> habit_record_list = new List<habit_record>();
        public static List<schedule> schedule_list = new List<schedule>();
        public static List<user> user_list = new List<user>();
    }
}
