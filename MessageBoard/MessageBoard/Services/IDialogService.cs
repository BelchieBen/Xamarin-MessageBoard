using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard.Services
{
    public interface IDialogService
    {
        Task ShowDialogAsync(string message, string title, string button);
        void ShowToastMessage(string message);
    }
}
