using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MessageBoard.Models;
using MessageBoard.Utility;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using Firebase.Database;
using Acr.UserDialogs;
using System.Collections.ObjectModel;
using Firebase.Database.Query;
using System.Linq;
using Firebase.Database.Offline;

namespace MessageBoard.Services
{
    public class MessageDataService: IMessageDataService
    {
        public event EventHandler MessageUpdated;
        SQLiteAsyncConnection db;
        IFirebaseAuth _auth;
        FirebaseClient _client;

        public MessageDataService()
        {
            _auth = DependencyService.Get<IFirebaseAuth>();
            _client = new FirebaseClient("https://xamarinauth2-fefde-default-rtdb.firebaseio.com/");
        }
        async Task Initialize()
        {
            if (db != null)
                return;
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MessageBoard.db");
            db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<Message>();
        }

        //Dont know how to get this to work?
        public void OfflineDb()
        {
            new FirebaseOptions
            {
                OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s),
            };
            var messageDb = _client.Child("Messages").AsRealtimeDatabase<Message>("", "", StreamingOptions.LatestOnly, InitialPullStrategy.MissingOnly, true);
        }

        public async Task<IList<Message>> GetMessages()
        {
            await Initialize();
            var message = await db.Table<Message>().ToListAsync();
            return message;
        }

        public async Task<List<Message>> FirebaseGetMessages()
        {
            return (await _client
                .Child("Messages")
                .OnceAsync<Message>())
                .Select(message => new Message
                {
                    MessageTitle = message.Object.MessageTitle,
                    Description = message.Object.Description,
                    PostDate = message.Object.PostDate,
                    User = message.Object.User,
                    Id = message.Object.Id,
                }).ToList();
        }

        public async Task NewFirebaseMessage(string messageTitle, string description)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.None)
            {
                UserDialogs.Instance.Alert("Unable to post message, no internet connection", "Post error", "Ok");
            }
            else
            {
                var user = await _auth.GetCurrentUser();
                Message m = new Message
                {
                    MessageTitle = messageTitle,
                    Description = description,
                    PostDate = DateTime.Now,
                    User = user.email
                };
                await _client
                    .Child("Messages")
                    .PostAsync(m);
                MessageUpdated?.Invoke(this, new EventArgs());
            }
            
        }

        public async Task UpdatedFirebaseMessage(string messageTitle, string description, int id, string user)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.None)
            {
                UserDialogs.Instance.Alert("Unable to update message, no internet connection", "Update error", "Ok");
            }
            else
            {
                var oldmessage = (await _client
                .Child("Messages")
                .OnceAsync<Message>()).FirstOrDefault
                (a => a.Object.Id == id);

                Message m = new Message()
                {
                    MessageTitle = messageTitle,
                    Description = description,
                    PostDate = DateTime.Now,
                    User = user,
                };
                await _client
                    .Child("Messages")
                    .Child(oldmessage.Key)
                    .PutAsync(m);

                MessageUpdated?.Invoke(this, new EventArgs());
            }
                
        }

        public async Task DeleteFirebaseMessage(string messageTitle,  string description, int id)
        {
            var deleteMessage = (await _client
                .Child("Messages")
                .OnceAsync<Message>()).FirstOrDefault
                (a => a.Object.Id == id);
            await _client.Child("Messages").Child(deleteMessage.Key).DeleteAsync();
        }

        public async Task NewMessage(string messageTitle, string description)
        {
            await Initialize ();
            var user = await _auth.GetCurrentUser();
            var Message = new Message
            {
                MessageTitle = messageTitle,
                Description = description,
                PostDate = DateTime.Now,
                User = user.email
                //Need to add user here
            };
            var id = await db.InsertAsync(Message);
            MessageUpdated?.Invoke(this, new EventArgs());
        }

        public async Task UpdateMessage(string messageTitle, string description, int id)
        {
            await Initialize();
            var oldmessage = await db.GetAsync<Message>(id);
            oldmessage.MessageTitle = messageTitle;
            oldmessage.Description = description;
            await db.InsertOrReplaceAsync(oldmessage);
            MessageUpdated?.Invoke(this, new EventArgs());

        }

        public async Task DeleteMessage(int id)
        {
            await Initialize ();
            await db.DeleteAsync<Message>(id);
            MessageUpdated?.Invoke(this, new EventArgs());
        }
    }
}
