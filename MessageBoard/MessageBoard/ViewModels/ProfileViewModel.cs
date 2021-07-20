using MessageBoard.Models;
using MessageBoard.Services;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MessageBoard.ViewModels
{
    public class ProfileViewModel
    {
        private ObservableCollection<Message> _messages;
        private IMessageDataService _messageDataService;
        private INavigationService _navigationService;
        private IFirebaseAuth _auth;
        private IDialogService _dialogService;

        public ICommand BackToMessages { get; }

        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
            }
        }

        // Add the current logged in user

        public ProfileViewModel(IMessageDataService messageDataService, INavigationService navigationService, IFirebaseAuth firebaseAuth, IDialogService dialogService)
        {
            // Need to only get the messages posted by the current user
            Messages = new ObservableCollection<Message>();
            _messageDataService = messageDataService;
            _navigationService = navigationService;
            _auth = firebaseAuth;
            _dialogService = dialogService;

            BackToMessages = new Command(OnBackCommand);
        }

        public void OnBackCommand()
        {
            _navigationService.GoBack();
        }

    }
}
