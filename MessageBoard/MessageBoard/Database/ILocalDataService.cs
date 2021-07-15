using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MessageBoard.Models;
using SQLite;

namespace MessageBoard.Database
{
    public interface ILocalDataService
    {
        void Initialize();
    }

    public class SqliteDataService: ILocalDataService
    {
        private SQLiteConnection _database;
        public void Initialize()
        {
            if (_database == null)
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MessageBoard.db");
                _database = new SQLiteConnection(dbPath);
                _database.CreateTable<Message>();
            }
           
        }
    }
}
