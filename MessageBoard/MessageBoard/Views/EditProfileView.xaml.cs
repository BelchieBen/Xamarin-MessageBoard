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
    public partial class EditProfileView : ContentPage
    {
        public EditProfileView()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.EditProfileViewModel;
        }
    }
}