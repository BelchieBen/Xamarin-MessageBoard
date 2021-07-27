using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard.Services
{
    public interface IUserDataService
    {
        Task FirebaseAdUser(string Token);
    }
}
