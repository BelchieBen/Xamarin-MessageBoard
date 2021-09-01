using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MessageBoard.Models
{
    public class User
    {
        private string _id;
        private string _username;
        private string _email;
        private Image _profileImg;

        [PrimaryKey]
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

        public Image profileImg
        {
            get => _profileImg;
            set
            {
                _profileImg = value;
            }
        }
    }
}
