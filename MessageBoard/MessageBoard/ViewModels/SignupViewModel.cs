using MessageBoard.Services;
using MessageBoard.Utility;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MessageBoard.ViewModels
{
    public class SignupViewModel: BaseViewModel
    {
        private INavigationService _navigationService;
        IFirebaseAuth _auth;

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

        public SignupViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _auth = DependencyService.Get<IFirebaseAuth>();

            SignupCommand = new AsyncCommand(() => OnSignupCommand());

        }

        public async Task OnSignupCommand()
        {
            var email = Email;
            var password = Password;
            var user =  _auth.SignUpWithEmailPassword(email, password);
            if (user)
            {
                await Application.Current.MainPage.DisplayAlert("Congratulations!", "Account Created Successfully", "Login");
                await _navigationService.GoTo(ViewNames.LoginPage);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Signup Failed", "Something went wrong. Please Try again!", "Try again");
            }
        }
    }
}
