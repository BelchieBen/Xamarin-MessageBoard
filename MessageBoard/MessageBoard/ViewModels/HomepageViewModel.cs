using System;
using System.Collections.Generic;
using System.Text;
using MessageBoard.Models;
using MessageBoard.Services;
using MessageBoard.Utility;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using MessageBoard.Views;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using Xamarin.Essentials;
using Acr.UserDialogs;
using System.Linq;
using MessageBoard.Styles;

namespace MessageBoard.ViewModels
{
    public class HomepageViewModel : BaseViewModel
    {
        private ObservableCollection<Message> _messages;
        private IMessageDataService _messageDataService;
        private INavigationService _navigationService;
        private IFirebaseAuth _auth;
        private IDialogService _dialogService;

        public ICommand RefreshCommand { get; }
        public ICommand NewCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ProfileCommand { get; }
        public AsyncCommand<Message> MessageSelectedCommand { get; }
        public AsyncCommand SearchCommand { get; }

        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                OnPropertyChanged("Messages");
            }
        }

        public HomepageViewModel(IMessageDataService messageDataService, INavigationService navigationService, IFirebaseAuth auth, IDialogService dialogService)
        {
            Messages = new ObservableCollection<Message>();
            _messageDataService = messageDataService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            RefreshCommand = new AsyncCommand(() => OnRefreshCommand());
            NewCommand = new AsyncCommand(() => OnNewCommand());
            LogoutCommand = new AsyncCommand(() => OnLogoutCommand());
            ProfileCommand = new MvvmHelpers.Commands.Command(OnProfileCommand);
            SearchCommand = new AsyncCommand(() => OnSearchCommand());
            MessageSelectedCommand = new AsyncCommand<Message>(OnMessageSelectedCommand);

            _auth = auth;
            Task.Run(async () => await FetchMessages());
            _messageDataService.MessageUpdated += OnUpdateMessage;
        }
        
        public bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }

        private async void OnUpdateMessage(object sender, EventArgs e)
        {
            await FetchMessages();
            await _auth.GetCurrentUser();
        }

        private async Task FetchMessages()
        {
            try
            {
                IsBusy = true;
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.None)
                {
                    await _dialogService.ShowDialogAsync(Strings.No_Connection, Strings.Refresh_error, Strings.Ok);
                }
                else
                {
                    Messages = new ObservableCollection<Message>( await _messageDataService.FirebaseGetMessages());
                }
            }
            catch
            {

            }
            finally
            {
                IsBusy = false;
            }          
        }

        public async Task OnRefreshCommand()
        {
            await FetchMessages();
            await _auth.GetCurrentUser();
        }

        public async Task OnNewCommand()
        {
            await _navigationService.NavigateTo(ViewNames.NewMessagePage);
        }

        public async Task OnMessageSelectedCommand(Message message)
        {
            var user = await _auth.GetCurrentUser();
            var userEmail = user.email;
            if (userEmail == message.User)
            {
                await _navigationService.NavigateTo(ViewNames.MessageDetailView, message);
            }
            else
            {
                await _navigationService.NavigateTo(ViewNames.MessageDetailReadonlyPage, message);
            }
            
        }

        public async Task OnLogoutCommand()
        {
            _auth.Logout();
            await _navigationService.GoTo(ViewNames.LoginPage);
        }

        public void OnProfileCommand()
        {
            _navigationService.GoTo(ViewNames.ProfileView);
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
            }
        }

        public async Task OnSearchCommand()
        {
            await FetchMessages();
            Messages = new ObservableCollection<Message>(Messages.Where((Message) => Message.MessageTitle.ToLower().Contains(SearchText.ToLower())));
        }
    }
}
