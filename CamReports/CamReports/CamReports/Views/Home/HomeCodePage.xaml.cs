using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CamReports.Views.Home
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeCodePage : ContentPage
    {
        public HomeCodePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }
    }
}
