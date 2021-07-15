using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MessageBoard.Services
{
    public interface INavigationService
    {
        Page MainPage { get; }

        void Configure(string key, Type pageType);
        void GoBack();
        Task GoTo(string key);
        Task NavigateTo(string key, object parameter = null);
        void NavigteAsync(string key);
    }
}
