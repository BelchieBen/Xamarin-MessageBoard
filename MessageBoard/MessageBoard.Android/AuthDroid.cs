using System;
using System.Threading.Tasks;
using MessageBoard.Services;
using MessageBoard.Droid;
using Firebase.Auth;
using Xamarin.Forms;
using MessageBoard;
using Android.Gms.Extensions;
using SQLite;
using System.IO;
using Xamarin.Essentials;
using MessageBoard.Models;
using MessageBoard.Utility;
using Android.App;

[assembly: Dependency(typeof(AuthDroid))]
namespace MessageBoard.Droid
{
    public class AuthDroid : IFirebaseAuth
    {
        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = await user.User.GetIdToken(false).AsAsync<GetTokenResult>();
                if (!string.IsNullOrEmpty(token?.Token))
                {
                    SetCurrentUser();
                }
                return token?.Token;
            }
            catch
            {
                return "";
            }
        }

        public bool SignUpWithEmailPassword(string email, string password)
        {
            try
            {
                var signUpTask = FirebaseAuth.Instance.CreateUserWithEmailAndPassword(email, password);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<SQLiteAsyncConnection> InitializatizeDb()
        {
            if (db == null)
            {
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MessageBoard.db");
                db = new SQLiteAsyncConnection(databasePath);
                await db.CreateTableAsync<User>();
            }          
            return db;
        }

        private SQLiteAsyncConnection db;

        public async Task<User> GetCurrentUser()
        {
            var db = await InitializatizeDb();
            var userEmail = FirebaseAuth.Instance.CurrentUser.Email;
            var userId = FirebaseAuth.Instance.CurrentUser.Uid; 
            var User = await db.GetAsync<User>(x => x.Id == userId);
            return User;
        }

        public async void SetCurrentUser()
        {
            var db = await InitializatizeDb ();
            var userEmail = FirebaseAuth.Instance.CurrentUser.Email;
            var userId = FirebaseAuth.Instance.CurrentUser.Uid;
            
            var User = new User
            {
                Id = userId,
                email = userEmail,
            };
            //Globals.userKey = User;
            await db.InsertAsync(User);

        }

        public void Logout()
        {
            //var user = FirebaseAuth.Instance.CurrentUser;
            FirebaseAuth.Instance.SignOut();
            Preferences.Remove(PreferenceKeys.USER_TOKEN);
        }
    }
}