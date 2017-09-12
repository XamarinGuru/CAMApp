using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CamReports.Views.Home
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeAllPage : ContentPage
    {
        public HomeAllPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }
    }
}
