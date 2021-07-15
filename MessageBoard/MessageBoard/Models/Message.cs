using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;

namespace MessageBoard.Models
{
    public class Message : INotifyPropertyChanged
    {
        private int _id;
        private string _messageTitle;
        private string _description;
        private DateTime _postDate;
        private string _user;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }

        public string MessageTitle
        {
            get => _messageTitle;
            set
            {
                _messageTitle = value;
                RaisePropertyChanged(nameof(_messageTitle));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged(nameof(_description));
            }
        }

        public DateTime PostDate
        {
            get => _postDate;
            set
            {
                _postDate = value;
                RaisePropertyChanged(nameof(_postDate));
            }
        }

        public string User
        {
            get => _user;
            set
            {
                _user = value;
                RaisePropertyChanged(nameof(_user));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
