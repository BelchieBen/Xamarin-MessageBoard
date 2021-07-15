using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBoard.Models
{
    public class User
    {
        private string _id;
        private string _username;
        private string _email;

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
            }
        }

        public string email
        {
            get => _email;
            set
            {
                _email = value;
            }
        }
    }
}
