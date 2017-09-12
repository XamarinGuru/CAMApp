using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CamReports.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CamReports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            Appearing += IssuesPage_OnAppearing;
        }

        private void IssuesPage_OnAppearing(object sender, EventArgs e)
        {
            var viewModel = BindingContext as BaseViewModel;
            viewModel?.LoadedCommand.Execute(null);
        }
    }
}
