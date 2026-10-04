namespace class_project_ieradumi
{
    internal class habit
    {
        private string ID;
        private string habit_name;
        private string habit_description;
        private string author_ID;
        private bool archived;
        
        private DateTime habit_creation_date;
        private DateTime habit_last_change_date;
        public habit(string ID, string habit_name, string habit_description, string author_ID, bool archived, DateTime habit_creation_date, DateTime habit_last_change_date) // Constructor for creating object of already existing habit
        {
            string start_of_ID = ID.Substring(0, 9);
            if (start_of_ID != "habit_ID_")
            {
                throw new ArgumentException("ID is not valid. It must start with 'habit_ID_<unique_id>'.");
            }
            this.ID = ID;
            this.habit_name = habit_name;
            this.habit_description = habit_description;
            this.author_ID = author_ID;
            this.archived = archived;
            this.habit_creation_date = habit_creation_date;
            this.habit_last_change_date = habit_last_change_date;
        }
        public habit(string habit_name, string habit_description, string author_ID) // Constructor for creating new habit object
        {
            this.ID = program.get_unique_ID_for_habit();
            this.habit_name = habit_name;
            this.habit_description = habit_description;
            this.author_ID = author_ID;
            this.archived = false;
            this.habit_creation_date = DateTime.Now;
            this.habit_last_change_date = DateTime.Now;
        }

        // Getters for habit class
        public string get_unique_ID()
        {
            return this.ID;
        }
        public string get_habit_name()
        {
            return this.habit_name;
        }
        public string get_habit_description()
        {
            return this.habit_description;
        }
        public string get_author_ID()
        {
            return this.author_ID;
        }
        public bool is_archived()
        {
            return this.archived;
        }
        public DateTime get_habit_creation_date()
        {
            return this.habit_creation_date;
        }
        public DateTime get_habit_last_change_date()
        {
            return this.habit_last_change_date;
        }


        // setters for habit class
        public void set_habit_name(string habit_name)
        {
            if (habit_name.Length < 3 || habit_name.Length > 50)
            {
                throw new ArgumentException("Habit name must be between 3 and 50 characters long.");
            }
            this.habit_name = habit_name;
            this.habit_last_change_date = DateTime.Now;
        }
        public void set_habit_description(string habit_description)
        {
            if (habit_description.Length < 3 || habit_description.Length > 200)
            {
                throw new ArgumentException("Habit description must be between 3 and 200 characters long.");
            }
            this.habit_description = habit_description;
            this.habit_last_change_date = DateTime.Now;
        }
        public void set_author_ID(string author_ID)
        {
            if (author_ID.Length < 9)
            {
                throw new ArgumentException("Author ID must be greater than 9 characters long. \"user_ID_<unique_ID>\"");
            }
            this.author_ID = author_ID;
            this.habit_last_change_date = DateTime.Now;
        }
        public void set_archived()
        {
            this.archived = true;
            this.habit_last_change_date = DateTime.Now;
        }
        public void set_unarchived()
        {
            this.archived = false;
            this.habit_last_change_date = DateTime.Now;
        }
    }
}
