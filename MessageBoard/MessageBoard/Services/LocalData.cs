using MessageBoard.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MessageBoard.Services
{
    public static class LocalData
    {
        static SQLiteAsyncConnection db;
        public static async Task InitializatizeDb()
        {
            if (db != null)
            {
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MessageBoardUsers.db");
                db = new SQLiteAsyncConnection(databasePath);
                await db.CreateTableAsync<User>();
            }
        }
    }
}
