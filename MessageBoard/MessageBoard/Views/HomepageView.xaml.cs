using MessageBoard.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MessageBoard.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomepageView : ContentPage
    {
        public HomepageView()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.HomepageViewModel;
        }
    }
}