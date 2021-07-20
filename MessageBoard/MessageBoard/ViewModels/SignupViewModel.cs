using MessageBoard.Services;
using MessageBoard.Styles;
using MessageBoard.Utility;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MessageBoard.ViewModels
{
    public class SignupViewModel: BaseViewModel
    {
        private INavigationService _navigationService;
        private IFirebaseAuth _auth;
        private IDialogService _dialogService;

        public ICommand SignupCommand { get; }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public SignupViewModel(INavigationService navigationService, IFirebaseAuth auth, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _auth = auth;
            _dialogService = dialogService;

            SignupCommand = new AsyncCommand(() => OnSignupCommand());

        }

        public async Task OnSignupCommand()
        {
            var email = Email;
            var password = Password;
            var user =  _auth.SignUpWithEmailPassword(email, password);
            if (user)
            {
                await _dialogService.ShowDialogAsync(Strings.Congratulations, Strings.Account_Created_Success, Strings.Login);
                await _navigationService.GoTo(ViewNames.LoginPage);
            }
            else
            {
                await _dialogService.ShowDialogAsync(Strings.Signup_Failed, Strings.Something_Went_Wrong, Strings.Try_Again);
            }
        }
    }
}
