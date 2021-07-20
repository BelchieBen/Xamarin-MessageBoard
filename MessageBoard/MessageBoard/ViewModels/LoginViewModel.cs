using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MessageBoard.Services;
using MessageBoard.Utility;
using Xamarin.Essentials;
using Acr.UserDialogs;
using MessageBoard.Styles;

namespace MessageBoard.ViewModels
{
    public class LoginViewModel: BaseViewModel
    {
        private INavigationService _navigationService;
        private IFirebaseAuth _auth;
        private IDialogService _dialogService;
        public ICommand LoginCommand { get; }
        public ICommand GoSignupCommand { get; }

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

        public LoginViewModel(INavigationService navigationService, IFirebaseAuth auth, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _auth = auth;
            _dialogService = dialogService;
            LoginCommand = new AsyncCommand(() => OnLoginCommand());
            GoSignupCommand = new AsyncCommand(() => OnGoSignupCommand());


        }

        public async Task OnLoginCommand()
        {
            var email = Email;
            var password = Password;
            string Token = await _auth.LoginWithEmailPassword(email, password);
            if (!string.IsNullOrWhiteSpace(Token))
            {
                await _auth.GetCurrentUser();
                await _navigationService.GoTo(ViewNames.HomepageView);
                if (DeviceInfo.Platform == DevicePlatform.Unknown)
                    return;
                else
                    Preferences.Set(PreferenceKeys.USER_TOKEN, Token);//Set preference but dont know where to fetch it when app is initalized
                
            }
            else
            {
                await _dialogService.ShowDialogAsync(Strings.Incorect_Email_or_Password, Strings.Auth_Failed ,Strings.Try_Again);
            }
        }

        public async Task OnGoSignupCommand()
        {
            await _navigationService.GoTo(ViewNames.SignupPage);
        }

        private async void ShowError()
        {
            await _dialogService.ShowDialogAsync(Strings.Incorect_Email_or_Password, Strings.Auth_Failed, Strings.Try_Again);
        }
    }
}
