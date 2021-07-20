using MessageBoard.Models;
using MessageBoard.Services;
using MessageBoard.Styles;
using MessageBoard.Utility;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MessageBoard.ViewModels
{
    public class MessageDetailViewModel: BaseViewModel
    {
        private Message _selectedMessage;
        private IMessageDataService _messageDataService;
        private INavigationService _navigationService;
        private IDialogService _dialogService;

        public AsyncCommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteCommand { get; }

        public Message SelectedMessage
        {
            get { return _selectedMessage; }
            set
            {
                _selectedMessage = value;
                OnPropertyChanged("SelectedMessage");
            }
        }

        public MessageDetailViewModel(IMessageDataService messageDataService, INavigationService navigationService, IDialogService dialogService)
        {
            _messageDataService = messageDataService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            SelectedMessage = new Message();
            SaveCommand = new AsyncCommand(() => OnSaveCommand ());
            CancelCommand = new Xamarin.Forms.Command(OnCancelCommand);
            DeleteCommand = new AsyncCommand(() => OnDeleteCommand());


        }

        public async Task OnSaveCommand()
        {
            if (SelectedMessage.User == null)
            {
                if (string.IsNullOrWhiteSpace(SelectedMessage.MessageTitle) || string.IsNullOrWhiteSpace(SelectedMessage.Description))
                {
                    return;
                }
                await _messageDataService.NewFirebaseMessage(SelectedMessage.MessageTitle, SelectedMessage.Description);
            }
            else
            {
               await  _messageDataService.UpdatedFirebaseMessage(SelectedMessage.MessageTitle, SelectedMessage.Description, SelectedMessage.Id, SelectedMessage.User);
            }
            _dialogService.ShowToastMessage(Strings.Message_Posted);
            _navigationService.GoBack();
        }

        public void OnCancelCommand()
        {
             _navigationService.PopPage();
        }

        public async Task OnDeleteCommand()
        {
            await _messageDataService.DeleteFirebaseMessage(SelectedMessage.MessageTitle, SelectedMessage.Description, SelectedMessage.Id);
            _dialogService.ShowToastMessage(Strings.Message_Deleted);
            _navigationService.PopPage();
        }

        public override void Initialize(object parameter)
        {
            if (parameter == null)
                SelectedMessage = new Message();
            else
                SelectedMessage = parameter as Message;
        }
    }
}
