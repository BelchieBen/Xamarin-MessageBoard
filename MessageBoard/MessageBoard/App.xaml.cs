using MessageBoard.Services;
using MessageBoard.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MessageBoard.Utility;
using Xamarin.Essentials;

namespace MessageBoard
{
    public partial class App : Application
    {
        public static MessageDataService MessageDataService { get; } = new MessageDataService();
        public static NavigationService NavigationService { get; } = new NavigationService();
        public static IFirebaseAuth FirebaseAuth { get; } = DependencyService.Get<IFirebaseAuth>();
        public static DialogService DialogService { get; } = new DialogService();
        public static UserDataService UserDataService { get; } = new UserDataService(FirebaseAuth);

        public App()
        {
            InitializeComponent();

            NavigationService.Configure(ViewNames.HomepageView, typeof(HomepageView));
            NavigationService.Configure(ViewNames.MessageDetailView, typeof(MessageDetailView));
            NavigationService.Configure(ViewNames.LoginPage, typeof(Page1));
            NavigationService.Configure(ViewNames.SignupPage, typeof(SignupView));
            NavigationService.Configure(ViewNames.MessageDetailReadonlyPage, typeof(MessageDetailReadonlyView));
            NavigationService.Configure(ViewNames.NewMessagePage, typeof(NewMessageView));
            NavigationService.Configure(ViewNames.ProfileView, typeof(ProfileView));
            NavigationService.Configure(ViewNames.EditProfile, typeof(EditProfileView));

            string userToken = Preferences.Get(PreferenceKeys.USER_TOKEN, "");
            if (!string.IsNullOrEmpty(userToken))
            {
                MainPage = new NavigationPage(new HomepageView());
            }
            else
            {
                MainPage = new NavigationPage(new Page1());
            }

            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
