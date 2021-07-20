using MessageBoard.Services;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MessageBoard.ViewModels
{
    public class ProfileViewModel
    {
        private INavigationService _navigationService;
        private IFirebaseAuth _auth;
        private IDialogService _dialogService;

        public ICommand BackToMessages { get; }

        public ProfileViewModel(INavigationService navigationService, IFirebaseAuth firebaseAuth, IDialogService dialogService)
        {
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
