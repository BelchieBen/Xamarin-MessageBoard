using MessageBoard.Models;
using MessageBoard.Services;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard.ViewModels
{
    public class SearchbarViewModel: BaseViewModel
    {
        private ObservableCollection<Message> _messages;
        private IMessageDataService _messageDataService;
        private INavigationService _navigationService;

        public AsyncCommand search { get; }

        public SearchbarViewModel(IMessageDataService messageDataService, INavigationService navigationService)
        {
            _messageDataService = messageDataService;
            _navigationService = navigationService;

            search = new AsyncCommand(() => OnSearchCommand());
        }
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                OnPropertyChanged("Messages");
            }
        }

        public async Task OnSearchCommand()
        {

        }
    }
}
