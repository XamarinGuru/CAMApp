using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CamReports.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeNearbyPage : ContentPage
    {
        public HomeNearbyPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }
    }
}
