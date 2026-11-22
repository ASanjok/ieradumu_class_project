using System;
using System.Collections.Generic;
using System.Text;

namespace shared_object_ieradumi
{
    public class SharedUser
    {
        public class LoginUserData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        public class RegisterUserData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class FullUserData
        {
            public string ID { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public bool Archived { get; set; }
            public DateTime UserCreationDate { get; set; }
            public DateTime UserLastChangeDate { get; set; }
        }

        public class UpdateUserData
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public bool Archived { get; set; }
        }
    }
}