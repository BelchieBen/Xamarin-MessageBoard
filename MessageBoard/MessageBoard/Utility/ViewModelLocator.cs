using MessageBoard.ViewModels;

namespace MessageBoard.Utility
{
    public static class ViewModelLocator
    {
        public static HomepageViewModel HomepageViewModel { get; set; } = new HomepageViewModel(App.MessageDataService,  App.NavigationService, App.FirebaseAuth, App.DialogService);
        public static MessageDetailViewModel MessageDetailViewModel { get; set; } = new MessageDetailViewModel(App.MessageDataService, App.NavigationService, App.DialogService);
        public static LoginViewModel LoginViewModel { get; set; } = new LoginViewModel(App.NavigationService, App.FirebaseAuth, App.DialogService);
        public static SignupViewModel SignupViewModel { get; set; } = new SignupViewModel(App.NavigationService, App.FirebaseAuth, App.DialogService);
        public static ProfileViewModel ProfileViewModel { get; set; } = new ProfileViewModel(App.NavigationService, App.FirebaseAuth, App.DialogService);
    }
}
