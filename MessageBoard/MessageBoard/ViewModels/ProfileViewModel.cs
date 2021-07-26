using MessageBoard.Models;
using MessageBoard.Services;
using MessageBoard.Styles;
using MessageBoard.Utility;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MessageBoard.ViewModels
{
    public class ProfileViewModel: BaseViewModel
    {
        private ObservableCollection<Message> _messages;
        private ObservableCollection<Message> _userMessages;
        private User _currentUser;
        private IMessageDataService _messageDataService;
        private INavigationService _navigationService;
        private IFirebaseAuth _auth;
        private IDialogService _dialogService;

        public AsyncCommand BackToMessagesCommand { get; }
        public AsyncCommand EditProfileCommand { get; }

        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
            }
        }

        public ObservableCollection<Message> UserMessages
        {
            get => _userMessages;
            set
            {
                _userMessages = value;
            }
        }

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
            }
        }

        public ProfileViewModel(IMessageDataService messageDataService, INavigationService navigationService, IFirebaseAuth firebaseAuth, IDialogService dialogService)
        {
            // These messages arent populated yet need investigating
            Messages = new ObservableCollection<Message>();
            _messageDataService = messageDataService;
            _navigationService = navigationService;
            _auth = firebaseAuth;
            _dialogService = dialogService;

            BackToMessagesCommand = new AsyncCommand(() => OnBackCommand());
            EditProfileCommand = new AsyncCommand(() => OnEditProfileCommand());
            Task.Run(async () => await FetchMessages());
            _auth.UserUpdated += OnUserUpdated;
        }

        private async void OnUserUpdated(object sender, EventArgs e)
        {
            await FetchMessages();
            await GetTheCurrentUser();
        }

        private async Task FetchMessages()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.None)
                {
                    await _dialogService.ShowDialogAsync(Strings.No_Connection, Strings.Refresh_error, Strings.Ok);
                }
                else
                {
                    await GetTheCurrentUser();
                    Messages = new ObservableCollection<Message>(await _messageDataService.FirebaseGetMessages());
                    UserMessages = new ObservableCollection<Message>(Messages.Where((Message) => Message.User.Contains(CurrentUser.email)));
                }
            }
            catch
            {

            }
            finally
            {
            }
        }

        private async Task GetTheCurrentUser()
        {
            CurrentUser = await _auth.GetCurrentUser();
        }



        public async Task OnBackCommand()
        {
            await _navigationService.GoTo(ViewNames.HomepageView);
        }

        public async Task OnEditProfileCommand()
        {
            await _navigationService.NavigateTo(ViewNames.EditProfile, CurrentUser);
        }

    }
}
