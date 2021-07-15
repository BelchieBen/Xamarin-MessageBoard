using System;
using System.Collections.Generic;
using System.Text;
using MessageBoard.Models;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using MessageBoard.ViewModels;
using System.Threading.Tasks;
using MessageBoard.Utility;
using Xamarin.Essentials;
using MessageBoard.Views;
using System.Globalization;
using System.Reflection;

namespace MessageBoard.Services
{
    public class NavigationService: INavigationService
    {
        private Dictionary<string, Type> pages { get; } = new Dictionary<string, Type>();

        public Page MainPage => Application.Current.MainPage;

        public void Configure(string key, Type pageType) => pages[key] = pageType;

        public void GoBack() => MainPage.Navigation.PopAsync();

        public async Task GoTo(string pageKey)
        {
            if (pages.TryGetValue(pageKey, out Type pageType))
            {
                var page = (Page)Activator.CreateInstance(pageType);
                await MainPage.Navigation.PushAsync(page);
            }
        }

        public async Task NavigateTo(string pageKey, object parameter = null)
        {
            if (pages.TryGetValue(pageKey, out Type pageType))
            {
                var page = (Page)Activator.CreateInstance(pageType);
                page.SetNavigationArgs(parameter);
                await MainPage.Navigation.PushAsync(page);
                (page.BindingContext as BaseViewModel).Initialize(parameter);
            }
            else
            {
                throw new ArgumentException($"This page doesn't exist: {pageKey}.", nameof(pageKey));
            }
        }

        public void NavigteAsync(string key)
        {
            
        }

        public async void PopPage()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }

    public static class NavigationExtensions
    {
        private static ConditionalWeakTable<Page, object> arguments = new ConditionalWeakTable<Page, object>();

        public static object GetNavigationArgs(this Page page)
        {
            object argument;
            arguments.TryGetValue(page, out argument);

            return argument;
        }

        public static void SetNavigationArgs(this Page page, object args)
            => arguments.Add(page, args);
    }
}
