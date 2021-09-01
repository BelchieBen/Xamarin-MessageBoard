using MessageBoard.Models;
using MessageBoard.Services;
using MessageBoard.Utility;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MessageBoard.ViewModels
{
    public class EditProfileViewModel: BaseViewModel
    {
        private User _currentUser;
        private INavigationService _navigationService;
        private IFirebaseAuth _auth;
        private IDialogService _dialogService;

        // Add user choose thier profile image 
        public ICommand SaveProfileCommand { get; }
        public ICommand ImagePickerCommand { get; }
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
            ImagePickerCommand = new AsyncCommand(() => OnImagePickerCommand());
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

        private Image _image;
        public Image image
        {
            get => _image;
            set
            {
                _image = value;
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

        public async Task OnImagePickerCommand()
        {
            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                image.Source = ImageSource.FromStream(() => stream);
            }
        }
    }
}
