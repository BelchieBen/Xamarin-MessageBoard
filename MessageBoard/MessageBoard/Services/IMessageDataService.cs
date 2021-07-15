using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using MessageBoard.Models;

namespace MessageBoard.Services
{
    public interface IMessageDataService
    {

        Task NewMessage(string messageTitle, string description);
        Task<IList<Message>> GetMessages();
        Task<List<Message>> FirebaseGetMessages();
        Task DeleteMessage(int id);
        Task NewFirebaseMessage(string messageTitle, string description);
        Task UpdateMessage(string messageTitle, string description, int id);
        Task UpdatedFirebaseMessage(string messageTitle, string description, int id, string user);
        Task DeleteFirebaseMessage(string messageTitle, string description, int id);
        event EventHandler MessageUpdated;
    }
}
