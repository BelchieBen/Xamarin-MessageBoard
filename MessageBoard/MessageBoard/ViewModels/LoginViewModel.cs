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

namespace MessageBoard.ViewModels
{
    public class LoginViewModel: BaseViewModel
    {
        private INavigationService _navigationService;
        private IFirebaseAuth _auth;
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

        public LoginViewModel(INavigationService navigationService, IFirebaseAuth auth)
        {
            _navigationService = navigationService;
            _auth = auth;
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
                Preferences.Set(PreferenceKeys.USER_TOKEN, Token);//Set preference but dont know where to fetch it when app is initalized
                await _navigationService.GoTo(ViewNames.HomepageView);
            }
            else
            {
                UserDialogs.Instance.Alert("Email or password is incorrect. Please Try again!", "Authentication Failed" ,"Ok");
            }
        }

        public async Task OnGoSignupCommand()
        {
            await _navigationService.GoTo(ViewNames.SignupPage);
        }

        private async void ShowError()
        {
            await Application.Current.MainPage.DisplayAlert("Authentication Failed", "Email or password is incorrect. Please Try again!", "Try again");
        }
    }
}
