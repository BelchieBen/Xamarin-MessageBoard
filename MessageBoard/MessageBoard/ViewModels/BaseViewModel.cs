using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MessageBoard.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Initialize(object parameter)
        {

        }
        public BaseViewModel()
        {

        }
    }
}
