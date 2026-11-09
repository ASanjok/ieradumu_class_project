namespace class_project_ieradumi
{
    public class User
    {
        private string _ID;
        public string ID
        {
            get => _ID;
            set => _ID = "user_ID_" + value;
        }

        private string _username;
        public string Username { 
            get => _username; 
            set 
            { 
                _username = value; 
                _userLastChangeDate = DateTime.Now; 
            } 
        }

        private string _email;
        public string Email { 
            get => _email; 
            set 
            { 
                _email = value; 
                _userLastChangeDate = DateTime.Now; 
            } 
        }

        private string _password;
        public string Password { 
            get => _password; 
            set 
            { 
                _password = value; 
                _userLastChangeDate = DateTime.Now; 
            } 
        }

        private bool _archived = false;
        public bool Archived { 
            get => _archived; 
            set 
            { 
                _archived = value; 
                _userLastChangeDate = DateTime.Now; 
            } 
        }

        private DateTime _userCreationDate;
        public DateTime UserCreationDate { 
            get => _userCreationDate;
            init => _userCreationDate = value;
        }

        private DateTime _userLastChangeDate;
        public DateTime UserLastChangeDate
        {
            get => _userLastChangeDate;
            set => _userLastChangeDate = value; // неиспользовать из вне класса (нельзя убрать потому что sqlite'у нужен этот метод)
        }
        public User()
        {
            _userCreationDate = DateTime.Now;
            _userLastChangeDate = DateTime.Now;
        }
        public User(string ID, string username, string email, string password, bool archived, DateTime user_creation_date, DateTime user_last_change_date) // Constructor for creating object of already existing user
        {
            string start_of_ID = ID.Substring(0, 8);
            if (start_of_ID != "user_ID_")
            {
                throw new ArgumentException("ID is not valid. It must start with 'user_ID_<unique_id>'.");
            }
            this.ID = ID;
            this._username = username;
            this._email = email;
            this._password = password;
            this._archived = archived;
            this._userCreationDate = user_creation_date;
            this._userLastChangeDate = user_last_change_date;
        }
        public User(string username, string email, string password) // Constructor for creating new user object
        {
            this.ID = Program.GetUniqueIDForUser();
            this._username = username;
            this._email = email;
            this._password = password;
            this._archived = false;
            this._userCreationDate = DateTime.Now;
            this._userLastChangeDate = DateTime.Now;
        }
    }
}
