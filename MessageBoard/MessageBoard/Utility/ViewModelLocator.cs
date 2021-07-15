using MessageBoard.ViewModels;

namespace MessageBoard.Utility
{
    public static class ViewModelLocator
    {
        public static HomepageViewModel HomepageViewModel { get; set; } = new HomepageViewModel(App.MessageDataService,  App.NavigationService, App.FirebaseAuth);
        public static MessageDetailViewModel MessageDetailViewModel { get; set; } = new MessageDetailViewModel(App.MessageDataService, App.NavigationService);
        public static LoginViewModel LoginViewModel { get; set; } = new LoginViewModel(App.NavigationService);
        public static SignupViewModel SignupViewModel { get; set; } = new SignupViewModel(App.NavigationService);
    }
}
