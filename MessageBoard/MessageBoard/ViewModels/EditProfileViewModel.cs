using MessageBoard.Models;
using MessageBoard.Services;
using MessageBoard.Utility;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MessageBoard.ViewModels
{
    public class EditProfileViewModel: BaseViewModel
    {
        private User _currentUser;
        private INavigationService _navigationService;
        private IFirebaseAuth _auth;
        private IDialogService _dialogService;

        public ICommand SaveProfileCommand { get; }
        public ICommand BackToProfileCommand { get; }
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged("CurrentUser");
            }
        }

        public EditProfileViewModel(INavigationService navigationService, IFirebaseAuth firebaseAuth, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _auth = firebaseAuth;
            _dialogService = dialogService;

            SaveProfileCommand = new AsyncCommand(() => OnSaveProfileCommand());
            BackToProfileCommand = new AsyncCommand(() => OnGoBackProfileCommand());
        }

        public override void Initialize(object parameter)
        {
            if (parameter is User user)
                CurrentUser = user;
        }

        private string _username;
        public string username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        public async Task OnSaveProfileCommand()
        {
            var usrName = _username;
            await _auth.AddDiaplayName(usrName);
            await _navigationService.GoTo(ViewNames.ProfileView);
        }

        public async Task OnGoBackProfileCommand()
        {
            await _navigationService.GoTo(ViewNames.ProfileView);
        }
    }
}
