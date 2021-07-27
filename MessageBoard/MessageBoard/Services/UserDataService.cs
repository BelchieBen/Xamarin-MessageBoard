using Acr.UserDialogs;
using Firebase.Database;
using Firebase.Database.Query;
using MessageBoard.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MessageBoard.Services
{
    public class UserDataService: IUserDataService
    {
        IFirebaseAuth _auth;
        FirebaseClient _client;

        public UserDataService(IFirebaseAuth auth)
        {
            _auth = auth;
            _client = new FirebaseClient("https://xamarinauth2-fefde-default-rtdb.firebaseio.com/");
        }

        public async Task FirebaseAdUser(string Token)
        {
            var networkAccess = Connectivity.NetworkAccess;
            if (networkAccess == NetworkAccess.None)
            {
                UserDialogs.Instance.Alert("Unable to add user, no internet connection", "Connection error", "Ok");
            }
            else
            {
                var user = await _auth.GetCurrentUser();
                User usr = new User
                {
                    Id = Token,
                    email = user.email,
                    Username = user.Username,
                };
                await _client
                    .Child("Users")
                    .PostAsync(usr);
            }

        }
    }
}
