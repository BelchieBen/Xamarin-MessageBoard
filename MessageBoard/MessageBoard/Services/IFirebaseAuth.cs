using MessageBoard.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard.Services
{
    public interface IFirebaseAuth
    {
        Task<string> LoginWithEmailPassword(string email, string password);
        bool SignUpWithEmailPassword(string emmail, string password);
        Task<User> GetCurrentUser();
        void SetCurrentUser();
        void Logout();
    }
}
