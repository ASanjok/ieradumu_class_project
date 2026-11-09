namespace class_project_ieradumi
{
    public class Habit
    {
        private string _ID;
        public string ID{ 
            get => _ID; 
            set  
            {
                _ID = "habit_ID_" + value; 
                _habitLastChangeDate = DateTime.Now; 
            }
        }

        private string _habitName;
        public string HabitName
        {
            get => _habitName;
            set
            {
                _habitName = value;
                _habitLastChangeDate = DateTime.Now;
            }
        }

        private string _habitDescription;
        public string HabitDescription
        {
            get => _habitDescription;
            set
            {
                _habitDescription = value;
                _habitLastChangeDate = DateTime.Now;
            }
        }

        private string _authorID;
        public string AuthorID
        {
            get => _authorID;
            set
            {
                _authorID = value;
                _habitLastChangeDate = DateTime.Now;
            }
        }

        private bool _archived;
        public bool Archived
        {
            get => _archived;
            set
            {
                _archived = value;
                _habitLastChangeDate = DateTime.Now;
            }
        }

        private DateTime _habitCreationDate;
        public DateTime HabitCreationDate { get => _habitCreationDate; init => _habitCreationDate = value; }

        private DateTime _habitLastChangeDate;
        public DateTime HabitLastChangeDate { get => _habitCreationDate; set => _habitCreationDate = value; }
        public Habit()
        {
            _habitCreationDate = DateTime.Now;
            _habitLastChangeDate = DateTime.Now;
        }
        public Habit(string ID, string habit_name, string habit_description, string author_ID, bool archived, DateTime habit_creation_date, DateTime habit_last_change_date) // Constructor for creating object of already existing habit
        {
            string start_of_ID = ID.Substring(0, 9);
            if (start_of_ID != "habit_ID_")
            {
                throw new ArgumentException("ID is not valid. It must start with 'habit_ID_<unique_id>'.");
            }
            this._ID = ID;
            this._habitName = habit_name;
            this._habitDescription = habit_description;
            this._authorID = author_ID;
            this._archived = archived;
            this._habitCreationDate = habit_creation_date;
            this._habitLastChangeDate = habit_last_change_date;
        }
        public Habit(string habit_name, string habit_description, string author_ID) // Constructor for creating new habit object
        {
            this._ID = Program.GetUniqueIdForHabit();
            this._habitName = habit_name;
            this._habitDescription = habit_description;
            this._authorID = author_ID;
            this._archived = false;
            this._habitCreationDate = DateTime.Now;
            this._habitLastChangeDate = DateTime.Now;
        }

        // Getters for habit class
        public string GetHabitID()
        {
            return this._ID;
        }
        public string GetHabitName()
        {
            return this._habitName;
        }
        public string GetHabitDescription()
        {
            return this._habitDescription;
        }
        public string GetAuthorID()
        {
            return this._authorID;
        }
        public bool IsArchived()
        {
            return this._archived;
        }
        public DateTime GetHabitCreationDate()
        {
            return this._habitCreationDate;
        }
        public DateTime GetHabitLastChangeDate()
        {
            return this._habitLastChangeDate;
        }


        // setters for habit class
        public void SetHabitName(string HabitName)
        {
            if (HabitName.Length < 3 || HabitName.Length > 50)
            {
                throw new ArgumentException("Habit name must be between 3 and 50 characters long.");
            }
            this._habitName = HabitName;
            this._habitLastChangeDate = DateTime.Now;
        }
        public void SetHabitDescription(string HabitDescription)
        {
            if (HabitDescription.Length < 3 || HabitDescription.Length > 200)
            {
                throw new ArgumentException("Habit description must be between 3 and 200 characters long.");
            }
            this._habitDescription = HabitDescription;
            this._habitLastChangeDate = DateTime.Now;
        }
        public void SetAuthorID(string AuthorID)
        {
            if (AuthorID.Length < 9)
            {
                throw new ArgumentException("Author ID must be greater than 9 characters long. \"user_ID_<unique_ID>\"");
            }
            this._authorID = AuthorID;
            this._habitLastChangeDate = DateTime.Now;
        }
        public void SetArchived()
        {
            this._archived = true;
            this._habitLastChangeDate = DateTime.Now;
        }
        public void SetUnarchived()
        {
            this._archived = false;
            this._habitLastChangeDate = DateTime.Now;
        }
    }
}
